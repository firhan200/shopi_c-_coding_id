using MySql.Data.MySqlClient;
using ShopiApi.Models;
using System.Data;
using System.Xml.Linq;

namespace ShopiApi.Repositories
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public User? GetByEmailAndPassword(string email, string password)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM users WHERE email=@Email and password=@Password", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User();
                    user.Id = reader.GetInt32("id");
                    user.Email = reader.GetString("email");
                    user.Role = reader.GetString("role");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //required
            conn.Close();

            return user;
        }
    }
}
