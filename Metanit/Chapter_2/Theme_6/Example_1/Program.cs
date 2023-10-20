using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";

string sqlExpression = "SELECT * FROM Clients";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows) // если есть данные
    {
        // выводим названия столбцов
        Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

        while (reader.Read()) // построчно считываем данные
        {
            object id = reader.GetValue(0);
            object status = reader.GetValue(1);
            object name = reader.GetValue(2);

            //object id = reader["id"];
            //object status = reader["status"];
            //object name = reader["name"];

            Console.WriteLine("{0} \t{1} \t{2}", id, status, name);
        }
    }

    reader.Close();
}

Console.Read();

//static void Main(string[] args)
//{
//    ReadDataAsync().GetAwaiter();

//    Console.Read();
//}

//private static async Task ReadDataAsync()
//{
//    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";

//    string sqlExpression = "SELECT * FROM Clients";
//    using (SqlConnection connection = new SqlConnection(connectionString))
//    {
//        await connection.OpenAsync();
//        SqlCommand command = new SqlCommand(sqlExpression, connection);
//        SqlDataReader reader = await command.ExecuteReaderAsync();

//        if (reader.HasRows)
//        {
//            // выводим названия столбцов
//            Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

//            while (await reader.ReadAsync())
//            {
//                object id = reader.GetValue(0);
//                object status = reader.GetValue(1);
//                object name = reader.GetValue(2);
//                Console.WriteLine("{0} \t{1} \t{2}", id, status, name);
//            }
//        }
//        reader.Close();
//    }
//}