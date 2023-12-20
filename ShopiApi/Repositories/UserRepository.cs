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

        public User? GetByToken(string token)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role, verification_expired_date FROM users WHERE verification_token=@Token", conn);
                cmd.Parameters.AddWithValue("@Token", token);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    user = new User();
                    user.Id = reader.GetInt32("id");
                    user.Email = reader.GetString("email");
                    user.Role = reader.GetString("role");
                    user.VerificationExpiredDate = reader.GetDateTime("verification_expired_date");
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

        public User? GetByEmail(string email)
        {
            User? user = null;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT id, email, role FROM users WHERE email=@Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

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

        public string Register(string email, string hashedPassword, string verificationToken)
        {
            string errorMessage = string.Empty;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);

            conn.Open();

            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO users(email, password, role, verification_token, verification_expired_date) VALUES (@Email, @Password, @Role, @VerificationToken, @VerificationExpiredDate)", conn);
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", hashedPassword);
                cmd.Parameters.AddWithValue("@Role", "user");
                cmd.Parameters.AddWithValue("@VerificationToken", verificationToken);
                cmd.Parameters.AddWithValue("@VerificationExpiredDate", DateTime.Now.AddMinutes(30));
                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                errorMessage = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //required
                conn.Close();
            }

            return errorMessage;
        }

        public string Activate(int userId)
        {
            string errorMessage = string.Empty;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);

            conn.Open();

            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE users SET activation_date=@ActivationDate WHERE id=@UserId", conn);
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ActivationDate", DateTime.Now);
                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                errorMessage = ex.Message;
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                //required
                conn.Close();
            }

            return errorMessage;
        }
    }
}
