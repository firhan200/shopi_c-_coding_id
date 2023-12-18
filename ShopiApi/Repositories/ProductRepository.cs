using MySql.Data.MySqlClient;
using ShopiApi.Models;
using System.Data;
using System.Xml.Linq;

namespace ShopiApi.Repositories
{
    public class ProductRepository
    {
        private string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public List<Product> GetAll()
        {
            List<Product> products = new List<Product>();

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();
                // able to query after open
                // Perform database operations
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM products", conn);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    string name = reader.GetString("name");
                    string description = reader.GetString("descriptions");
                    int price = reader.GetInt32("price");
                    string imagePath = reader.IsDBNull("image_path") ? string.Empty : reader.GetString("image_path");

                    products.Add(new Product
                    {
                        Id = id,
                        Name = name,
                        Description = description,
                        Price = price,
                        ImagePath = imagePath,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return products;
        }

        public List<Product> GetAllWithAdapter()
        {
            List<Product> products = new List<Product>();

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);
            try
            {
                conn.Open();

                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM products", conn);
                DataTable table = new DataTable();
                adapter.Fill(table);

                Console.WriteLine("Starting");

                foreach(DataRow perData in table.Rows)
                {
                    Console.WriteLine("Name: " + perData["name"].ToString());
                    products.Add(new Product
                    {
                        Id = Convert.ToInt32(perData["id"]),
                        Name = perData["name"].ToString(),
                        Description = perData["descriptions"].ToString(),
                        Price = Convert.ToInt32(perData["price"]),
                    });
                }
                Console.WriteLine("Finish");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //required
            conn.Close();

            return products;
        }

        public string Create(string name, string description, int price, string imagePath)
        {
            string errorMessage = string.Empty;

            //get connection to database
            MySqlConnection conn = new MySqlConnection(_connectionString);

            conn.Open();

            MySqlTransaction transaction = conn.BeginTransaction();

            try
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO products(name, descriptions, price, image_path) VALUES (@Name, @Description, @Price, @ImagePath)", conn);
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);
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
