using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

static class Cryptography {

    internal static SecurePassword GetSaltHash(string password) {

        Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, 20);
        byte[] salt = deriveBytes.Salt;
        byte[] pass = deriveBytes.GetBytes(20);
        return new SecurePassword(Convert.ToBase64String(pass), Convert.ToBase64String(salt));
    }
}

class SecurePassword {
    public string Hash { get; private set; }
    public string Salt { get; private set; }
    public SecurePassword(string hash, string salt) {
        Hash = hash;
        Salt = salt;
    }

}
