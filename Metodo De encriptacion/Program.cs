using System;
using System.Linq;
using System.Text;

public class SimpleSerpent
{
    private byte[] key;

    public SimpleSerpent(byte[] key)
    {
        this.key = key;
    }

    public byte[] Encrypt(byte[] plaintext)
    {
        byte[] data = new byte[plaintext.Length];
        Array.Copy(plaintext, data, plaintext.Length);

        for (int round = 0; round < 32; round++)
        {
            // Example substitution (simple XOR with round number)
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ key[i % key.Length] ^ round);
            }

            // Example permutation (swap first and last byte)
            byte temp = data[0];
            data[0] = data[data.Length - 1];
            data[data.Length - 1] = temp;
        }

        return data;
    }

    public byte[] Decrypt(byte[] ciphertext)
    {
        byte[] data = new byte[ciphertext.Length];
        Array.Copy(ciphertext, data, ciphertext.Length);

        for (int round = 31; round >= 0; round--)
        {
            // Reverse permutation
            byte temp = data[0];
            data[0] = data[data.Length - 1];
            data[data.Length - 1] = temp;

            // Reverse substitution
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ key[i % key.Length] ^ round);
            }
        }

        return data;
    }
}

public class Program
{
    public static void Main()
    {
        // Pedir la contraseña al usuario
        Console.Write("Introduce la contraseña: ");
        string password = Console.ReadLine();

        // Convertir la contraseña en bytes
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Clave de cifrado simple
        byte[] key = Encoding.UTF8.GetBytes("mysecretkey12345"); // Key must be 16 bytes for simplicity

        // Crear una instancia de SimpleSerpent
        SimpleSerpent serpent = new SimpleSerpent(key);

        // Cifrar la contraseña
        byte[] encryptedPassword = serpent.Encrypt(passwordBytes);
        Console.WriteLine($"Contraseña cifrada: {Convert.ToBase64String(encryptedPassword)}");

        // Descifrar la contraseña
        byte[] decryptedPassword = serpent.Decrypt(encryptedPassword);
        Console.WriteLine($"Contraseña descifrada: {Encoding.UTF8.GetString(decryptedPassword)}");
    }
}
