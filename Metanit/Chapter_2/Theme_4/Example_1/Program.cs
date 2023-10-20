using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
string connectionString2 = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open(); // создается первый пул
    Console.WriteLine(connection.ClientConnectionId);
}
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open(); // подключение извлекается из первого пула
    Console.WriteLine(connection.ClientConnectionId);
}
using (SqlConnection connection = new SqlConnection(connectionString2))
{
    connection.Open(); // создается второй пул, т.к. строка подключения отличается
    Console.WriteLine(connection.ClientConnectionId);
}