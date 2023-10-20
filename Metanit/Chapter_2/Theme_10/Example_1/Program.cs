using System.Data;
using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
int age = 23;
string name = "Kenny";
string sqlExpression = "INSERT INTO Clients (Id, Name, Age) VALUES (@id, @name, @age);SET @id=SCOPE_IDENTITY()"; //метод не работает, ибо ID не может быть null
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlCommand command = new SqlCommand(sqlExpression, connection);
    // создаем параметр для имени
    SqlParameter nameParam = new SqlParameter("@name", name);
    // добавляем параметр к команде
    command.Parameters.Add(nameParam);
    // создаем параметр для возраста
    SqlParameter ageParam = new SqlParameter("@age", age);
    // добавляем параметр к команде
    command.Parameters.Add(ageParam);
    // параметр для id
    SqlParameter idParam = new SqlParameter
    {
        ParameterName = "@id",
        SqlDbType = SqlDbType.Int,
        Direction = ParameterDirection.Output // параметр выходной
    };
    command.Parameters.Add(idParam);
     
    command.ExecuteNonQuery();
     
     // получим значения выходного параметра
    Console.WriteLine("Id нового объекта: {0}", idParam.Value);
}