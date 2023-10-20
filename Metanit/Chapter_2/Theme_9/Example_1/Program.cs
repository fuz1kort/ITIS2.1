using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
int age = 23;
int id = 3;
string name = "T',10);INSERT INTO Clients (Id, Name, Age) VALUES('H";
string sqlExpression = "INSERT INTO Clients (Id,Name, Age) VALUES (@id, @name, @age)";
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    // создаем параметр для id
    SqlParameter idparam = new SqlParameter(@"id", id);
    command.Parameters.Add(idparam);
    // создаем параметр для имени
    SqlParameter nameParam = new SqlParameter("@name", name);
    // добавляем параметр к команде
    command.Parameters.Add(nameParam);
    // создаем параметр для возраста
    SqlParameter ageParam = new SqlParameter("@age", age);
    // добавляем параметр к команде
    command.Parameters.Add(ageParam);
 
    int number = command.ExecuteNonQuery();
    Console.WriteLine("Добавлено объектов: {0}", number); // 1
}