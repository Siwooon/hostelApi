public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(decimal Price, string cardNumber);
    Task<bool> ValidatePaymentDetailsAsync(string cardNumber);
}