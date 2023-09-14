using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace CSharpCrudApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;Database=clientinfo;User ID=root;Password=;CharSet=utf8;";
            using (MySqlConnection connection = new(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                MySqlCommand cmd;
                MySqlDataReader reader;

                int choice;
                string firstName, lastName, address, city, country;

                while (true)
                {
                    Console.WriteLine("MySQL C# CRUD App.");
                    Console.WriteLine("1. INSERT");
                    Console.WriteLine("2. UPDATE");
                    Console.WriteLine("3. DELETE");
                    Console.WriteLine("4. SELECT");
                    Console.WriteLine("5. EXIT");
                    Console.WriteLine("Enter a choice: ");
                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("1. Insert new data.");
                            Console.WriteLine("Enter first name: ");
                            firstName = Console.ReadLine();
                            Console.WriteLine("Enter last name: ");
                            lastName = Console.ReadLine();
                            Console.WriteLine("Enter address: ");
                            address = Console.ReadLine();
                            Console.WriteLine("Enter city: ");
                            city = Console.ReadLine();
                            Console.WriteLine("Enter country: ");
                            country = Console.ReadLine();

                            cmd = new MySqlCommand("INSERT INTO customer (customer_first_name, customer_last_name, address, city, country) VALUES (@firstName, @lastName, @address, @city, @country)", connection);
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@city", city);
                            cmd.Parameters.AddWithValue("@country", country);

                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Data Insert Success!");
                            break;

                        case 2:
                            Console.WriteLine("2. Update existing data.");
                            Console.WriteLine("Enter ID of the customer record you want to update: ");
                            int idToUpdate = int.Parse(Console.ReadLine());

                            Console.WriteLine("Enter new first name (leave empty to skip): ");
                            firstName = Console.ReadLine();
                            Console.WriteLine("Enter new last name (leave empty to skip): ");
                            lastName = Console.ReadLine();
                            Console.WriteLine("Enter new address (leave empty to skip): ");
                            address = Console.ReadLine();
                            Console.WriteLine("Enter new city (leave empty to skip): ");
                            city = Console.ReadLine();
                            Console.WriteLine("Enter new country (leave empty to skip): ");
                            country = Console.ReadLine();

                            StringBuilder updateQuery = new StringBuilder("UPDATE customer SET ");
                            bool first = true;

                            if (!string.IsNullOrEmpty(firstName))
                            {
                                updateQuery.Append("customer_first_name = @firstName");
                                first = false;
                            }

                            if (!string.IsNullOrEmpty(lastName))
                            {
                                if (!first) updateQuery.Append(", ");
                                updateQuery.Append("customer_last_name = @lastName");
                                first = false;
                            }

                            if (!string.IsNullOrEmpty(address))
                            {
                                if (!first) updateQuery.Append(", ");
                                updateQuery.Append("address = @address");
                                first = false;
                            }

                            if (!string.IsNullOrEmpty(city))
                            {
                                if (!first) updateQuery.Append(", ");
                                updateQuery.Append("city = @city");
                                first = false;
                            }

                            if (!string.IsNullOrEmpty(country))
                            {
                                if (!first) updateQuery.Append(", ");
                                updateQuery.Append("country = @country");
                            }

                            updateQuery.Append(" WHERE customer_id = @idToUpdate");

                            cmd = new MySqlCommand(updateQuery.ToString(), connection);
                            cmd.Parameters.AddWithValue("@firstName", firstName);
                            cmd.Parameters.AddWithValue("@lastName", lastName);
                            cmd.Parameters.AddWithValue("@address", address);
                            cmd.Parameters.AddWithValue("@city", city);
                            cmd.Parameters.AddWithValue("@country", country);
                            cmd.Parameters.AddWithValue("@idToUpdate", idToUpdate);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Data Update Success!");
                            }
                            else
                            {
                                Console.WriteLine("No records found to update.");
                            }
                            break;

                        case 3:
                            Console.WriteLine("Enter ID of the customer record you want to delete: ");
                            int idToDelete = int.Parse(Console.ReadLine());

                            cmd = new MySqlCommand("DELETE FROM customer WHERE customer_id = @idToDelete", connection);
                            cmd.Parameters.AddWithValue("@idToDelete", idToDelete);

                            int rowsDeleted = cmd.ExecuteNonQuery();
                            if (rowsDeleted > 0)
                            {
                                Console.WriteLine("Data Deletion Success!");
                            }
                            else
                            {
                                Console.WriteLine("No records found to delete.");
                            }
                            break;

                        case 4:
                            Console.WriteLine("Listing all records from the customer table:");
                            cmd = new MySqlCommand("SELECT * FROM customer", connection);
                            reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                Console.WriteLine($"ID: {reader.GetInt32("customer_id")}, First Name: {reader.GetString("customer_first_name")}, Last Name: {reader.GetString("customer_last_name")}, Address: {reader.GetString("address")}, City: {reader.GetString("city")}, Country: {reader.GetString("country")}");
                            }
                            reader.Close();
                            break;

                        case 5:
                            Console.WriteLine("Exiting the application. Goodbye!");
                            connection.Close();
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid selection. Try again.");
                            break;
                    }
                }
            }
        }
    }
}
