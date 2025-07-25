# Documentation Technique - Système de Gestion d'Hôtel

## 1. Objectifs du Projet

Le projet vise à développer une API Web ASP.NET Core pour la gestion d'un hôtel. Le système prend en charge :

* La réservation de chambres
* La gestion des arrivées (check-in) et des départs (check-out)
* La gestion du personnel de ménage
* L'attribution des rôles (Client, Réceptionniste, Personnel)

## 2. Architecture Globale

Le projet repose sur une architecture en couches inspirée du pattern Clean Architecture :

* **API (Controller)** : Expose les endpoints REST
* **Service** : Contient la logique métier
* **Repository** : Gestion de l'accès aux données (Entity Framework Core)
* **Entity / Models** : Entités persistées

Le système est conçu pour être **modulaire, testable et respectueux des principes SOLID**.

## 3. Technologies Utilisées

* **ASP.NET Core 8 Web API**
* **Entity Framework Core + PostgreSQL**
* **JWT pour l'authentification**
* **Swagger** pour la documentation automatique

## 4. Gestion des Rôles et de la Sécurité

Le système utilise JWT pour l'authentification et les autorisations par rôle sont définies via l'attribut `[Authorize(Roles = ...)]`. Les rôles principaux sont :

* `Client` : Peut consulter ses réservations, en créer et les annuler
* `Receptionist` : Gère les arrivées, départs, et peut annuler toute réservation
* `Staff` : Accède aux chambres à nettoyer, peut les marquer comme propres

### 4.1. Comptes de connexion (mots de passe chiffrés)

Voici les identifiants pour tester chaque rôle :

* **Agent d'entretien** :

```json
{
  "username": "Gégééé@gmail.com",
  "password": "HostelAPIPassword"
}
```

* **Réceptionniste** :

```json
{
  "username": "JauneDo@gmail.com",
  "password": "HostelAPIPassword"
}
```

* **Client** :

```json
{
  "username": "LaurentGina@hotmail.com",
  "password": "HostelAPIPassword"
}
```

## 5. Principaux Défis Rencontrés et Solutions

### 5.1. Boucles de sérialisation JSON (object cycle)

#### Problème

Erreur : `A possible object cycle was detected` lors de la sérialisation des entités EF.

#### Solution

* Ajout de `ReferenceHandler.IgnoreCycles` dans la configuration JSON

```csharp
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
```

### 5.2. Contrainte de clé étrangère sur ReservationRoom

#### Problème

Exception `23503` sur `ReservationRoom` lors de la création d'une réservation.

#### Solution

* Sauvegarde de la `Reservation` avant de créer les objets `ReservationRoom`
* Ajout explicite des `ReservationRoom` via `DbSet`

```csharp
_context.Reservations.Add(reservation);
await _context.SaveChangesAsync();

reservation.ReservationRooms = ...;
_context.ReservationRooms.AddRange(reservation.ReservationRooms);
await _context.SaveChangesAsync();
```

### 5.3. Gestion des annulations selon les délais

* Si un client annule **> 48h avant** : remboursement automatique
* Si annulation par réceptionniste : remboursement optionnel (paramètre bool)

## 6. Gestion des Arrivées et Départs

### Check-In

* Accessible uniquement au rôle `Receptionist`
* Vérifie que la réservation existe et n'est pas déjà marquée comme `arrivée`
* Marque `EstArrive = true` et met à jour le paiement si nécessaire

### Check-Out

* Vérifie que l'arrivée a été faite
* Marque la chambre comme à nettoyer
* Valide le paiement

## 7. Gestion du Personnel de Ménage

* Liste des chambres à nettoyer filtrée par l'état de `A Nettoyer`
* Possibilité de les marquer comme nettoyées

## 8. Démarrage du Projet

### Lancer le projet

1. Ouvrir le dossier du projet dans **Visual Studio 2022 ou ultérieur**
2. S'assurer que PostgreSQL est lancé
3. Vérifier/appuyer la chaîne de connexion dans `appsettings.json`
4. Lancer le script PowerShell suivant pour appliquer les migrations :

```powershell
./migration.ps1
```

5. Exécuter l'API (F5 ou bouton "Run")
6. Accéder à Swagger via : `https://localhost:{port}/swagger`

## 9. Points d'Amélioration Envisagés

* Envoi d'e-mails post-séjour
* Export des données statistiques (taux d'occupation, revenus)
* Interface admin pour la gestion des utilisateurs et chambres

---
