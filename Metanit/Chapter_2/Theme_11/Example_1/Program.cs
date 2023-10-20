using System.Data.SqlClient;

class Program
{
    static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
    static void Main(string[] args) 
    {
        Console.Write("Введите имя пользователя:");
        string name = Console.ReadLine();
 
        Console.Write("Введите возраст пользователя:");
        int age = Int32.Parse(Console.ReadLine());
 
        AddClient(name, age);
        Console.WriteLine();
        GetClients();
 
        Console.Read();
    }
    // добавление пользователя
    private static void AddClient(string name, int age)
    {
        // название процедуры
        string sqlExpression = "sp_InsertClients"; 
 
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            // указываем, что команда представляет хранимую процедуру
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = 6
            };
            
            command.Parameters.Add(idParam);
            // параметр для ввода имени
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = name
            };
            // добавляем параметр
            command.Parameters.Add(nameParam);
            // параметр для ввода возраста
            SqlParameter ageParam = new SqlParameter
            {
                ParameterName = "@age",
                Value = age
            };
            command.Parameters.Add(ageParam);
 
            var result = command.ExecuteScalar();
            // если нам не надо возвращать id
            //var result = command.ExecuteNonQuery();
 
            Console.WriteLine("Id добавленного объекта: {0}", result);
        }
    }
 
    // вывод всех пользователей
    private static void GetClients()
    {
        // название процедуры
        string sqlExpression = "sp_GetClients";
 
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            // указываем, что команда представляет хранимую процедуру
            command.CommandType = System.Data.CommandType.StoredProcedure;
            var reader = command.ExecuteReader();
 
            if (reader.HasRows)
            {
                Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(2), reader.GetName(3));
 
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(2);
                    int age = reader.GetInt32(3);
                    Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                }
            }
            reader.Close();
        }
    }
}