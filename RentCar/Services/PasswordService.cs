using Microsoft.AspNetCore.Identity;
using RentCar.Models;

namespace RentCar.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<MsCustomer>();
            return hasher.HashPassword(null, password);
        }

        public bool VerifyPassword(MsCustomer customer, string password)
        {
            var hasher = new PasswordHasher<MsCustomer>();
            var result = hasher.VerifyHashedPassword(customer, customer.password, password);
            return result == PasswordVerificationResult.Success;
        }

    }
}
