using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";
;
using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();
    SqlCommand command = new SqlCommand();
    command.CommandText = "SELECT * FROM Clients";
    command.Connection = connection;
}

//string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
//string sqlExpression = "SELECT * FROM Users";
//using (SqlConnection connection = new SqlConnection(connectionString))
//{
//    connection.Open();
//    SqlCommand command = new SqlCommand(sqlExpression, connection);
//}