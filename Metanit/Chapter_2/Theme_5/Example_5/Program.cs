using System.Data.SqlClient;

Console.WriteLine("Введите имя:");
string name = Console.ReadLine();

Console.WriteLine("Введите возраст:");
int age = Int32.Parse(Console.ReadLine());

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";

string sqlExpression = String.Format("INSERT INTO Clients (Id, Name, Age) VALUES (1, '{0}', {1})", name, age);
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    // добавление
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    int number = command.ExecuteNonQuery();
    Console.WriteLine("Добавлено объектов: {0}", number);

    // обновление ранее добавленного объекта
    Console.WriteLine("Введите новое имя:");
    name = Console.ReadLine();
    sqlExpression = String.Format("UPDATE Clients SET Name='{0}' WHERE Age={1}", name, age);
    command.CommandText = sqlExpression;
    number = command.ExecuteNonQuery();
    Console.WriteLine("Обновлено объектов: {0}", number);
}
Console.Read();