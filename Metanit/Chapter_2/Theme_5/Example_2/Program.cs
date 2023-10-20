using System.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
        string sqlExpression = "INSERT INTO Clients (Id, Status, Name, Age, Contact, IsBlocked, IsAnonymous) VALUES (2, 1, 'Marat', 19, '+79173998364', 0, 1)";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            int number = command.ExecuteNonQuery();
            Console.WriteLine("Добавлено объектов: {0}", number);
        }
        Console.Read();
    }
}