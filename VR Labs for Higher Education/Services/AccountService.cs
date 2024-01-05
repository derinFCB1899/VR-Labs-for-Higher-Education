using Microsoft.AspNetCore.Identity;

public class AccountService // Assuming this is part of your service layer
{
    private readonly PasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

    public string HashPassword(string password)
    {
        // Use an empty object as we don't have a User class
        return _passwordHasher.HashPassword(null, password);
    }

    // Other methods...
}
