using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

class InsecureDotNetExample
{
    static void Main()
    {
        // Hardcoded credentials (BAD PRACTICE)
        string connectionString = "Server=myserver;Database=mydb;User Id=admin;Password=SuperSecret123;";

        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        // BAD: Uses MD5 which is insecure for password hashing
        string hashedPassword = ComputeMD5Hash(password);

        // BAD: SQL Injection risk (directly concatenating user input)
        string query = "SELECT * FROM Users WHERE Username = '" + username + "' AND PasswordHash = '" + hashedPassword + "'";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn)) // Vulnerable to SQL Injection
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Login successful!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid username or password.");
                    }
                }
            }
        }
    }

    static string ComputeMD5Hash(string input)
    {
        using (MD5 md5 = MD5.Create()) // BAD: MD5 is insecure
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
