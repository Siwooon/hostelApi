using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController, Route("[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _service;

    public ReservationsController(IReservationService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Client,Receptionist")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMine()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized("Identifiant utilisateur manquant.");
        var userId = int.Parse(userIdStr);
        var result = await _service.GetForUserAsync(userId);
        return Ok(result);
    }

    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationDto dto)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized("Identifiant utilisateur manquant.");
        var userId = int.Parse(userIdStr);

        try
        {
            var result = await _service.AreRoomsAvailableAsync(dto.RoomIds, dto.StartDate, dto.EndDate);
            if (!result)
            {
                return BadRequest("Chambres non disponibles pour les dates sélectionnées.");
            }

            var res = await _service.CreateAsync(userId, dto);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Client,Receptionist")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id, [FromQuery] bool refund = false)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized("Identifiant utilisateur manquant.");
        var userId = int.Parse(userIdStr);
        var isReceptionist = User.IsInRole("Receptionist");

        var ok = await _service.CancelAsync(id, userId, isReceptionist, refund);
        return ok ? Ok(refund ? "Annulée et remboursée." : "Annulée.") : BadRequest("Annulation impossible.");
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id}/checkin")]
    public async Task<IActionResult> CheckIn(int id, [FromQuery] bool paid)
    {
        var result = await _service.CheckInAsync(id, paid);
        if (!result)
            return BadRequest("Check-in impossible. Réservation introuvable ou déjà arrivée.");

        return Ok("Check-in effectué avec succès.");
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id}/checkout")]
    public async Task<IActionResult> Checkout(int id)
    {
        var success = await _service.CheckOutAsync(id);
        if (!success)
            return BadRequest("Check-out impossible. Vérifiez l'arrivée, le paiement ou l'état actuel de la réservation.");

        return Ok("Check-out effectué. La chambre est maintenant marquée pour nettoyage.");
    }
}
