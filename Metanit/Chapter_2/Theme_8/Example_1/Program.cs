using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";

string sqlExpression = "SELECT COUNT(*) FROM Clients";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    object count = command.ExecuteScalar();
 
    command.CommandText = "SELECT MIN(Age) FROM Clients";
    object minAge = command.ExecuteScalar();
 
    Console.WriteLine("В таблице {0} объектов", count);
    Console.WriteLine("Минимальный возраст: {0}", minAge);
}