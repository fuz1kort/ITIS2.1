using System.Data.SqlClient;

namespace Example_1
{
    class Program
    {
        static void Main()
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=MyORMExample";

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                connection.Open();
                Console.WriteLine("Подключение открыто");
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // закрываем подключение
                connection.Close();
                Console.WriteLine("Подключение закрыто...");
            }

            Console.Read();
        }

        //static void Main(string[] args)
        //{
        //    ConnectWithDB().Wait();
        //}

        //private static async Task ConnectWithDB()
        //{
        //    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();
        //        Console.WriteLine("Подключение открыто");
        //    }
        //    Console.WriteLine("Подключение закрыто...");
        //}



        //static void Main(string[] args)
        //{
        //    string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=StripClub;Integrated Security=True;Connect Timeout=30;Encrypt=False";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        Console.WriteLine("Подключение открыто");
        //    }
        //    Console.WriteLine("Подключение закрыто...");

        //    Console.Read();
        //}


    }
}