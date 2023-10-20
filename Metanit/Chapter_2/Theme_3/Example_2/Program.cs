using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    Console.WriteLine("Подключение открыто");

    // Вывод информации о подключении
    Console.WriteLine("Свойства подключения:");
    Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
    Console.WriteLine("\tБаза данных: {0}", connection.Database);
    Console.WriteLine("\tСервер: {0}", connection.DataSource);
    Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
    Console.WriteLine("\tСостояние: {0}", connection.State);
    Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);
}

Console.WriteLine("Подключение закрыто...");