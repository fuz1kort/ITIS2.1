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
            int id = reader.GetInt32(0);
            int status = reader.GetInt32(1);
            string name = reader.GetString(2);

            Console.WriteLine("{0} \t{1} \t{2}", id, status, name);
        }
    }

    reader.Close();
}

Console.Read();