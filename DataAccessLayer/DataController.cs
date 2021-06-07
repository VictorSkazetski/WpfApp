using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using WpfAppDB.Model;

namespace WpfAppDB.DataAccessLayer
{
    class DataController
    {
        static readonly string connectString;

        public static async Task ExecutQuery(string sqlExpression)
        {
            using (SqlConnection connection = new SqlConnection(connectString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        public static async Task<List<Employee>> GetAllData()
        {
            string sqlExpression = "select * from employees";

            List<Employee> empData = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        empData.Add(new Employee()
                        {
                            ID = (int)reader.GetValue(0),
                            Name = reader.GetValue(1).ToString(),
                            Surname = reader.GetValue(2).ToString(),
                            MiddleName = reader.GetValue(3).ToString(),
                            Birthday = Convert.ToDateTime(reader.GetValue(4).ToString()),
                            Department = reader.GetValue(5).ToString()
                        });
                    }
                }

                reader.Close();
            }

            return empData;
        }



        static DataController()
        {
            connectString = ConfigurationManager.ConnectionStrings["ConnectStr"].ConnectionString;
        }
    }
}
