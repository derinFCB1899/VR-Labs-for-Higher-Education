using Microsoft.AspNetCore.Identity;

namespace VR_Labs_for_Higher_Education.Services
{
    public class AccountService
    {
        private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

        public string HashPassword(string password)
        {
            // Function used for passwordHashing
            return _passwordHasher.HashPassword(null, password);
        }

    }
}
