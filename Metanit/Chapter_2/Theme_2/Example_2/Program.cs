using System.Configuration;

namespace AdoNetConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //string connectionString = @"Data Source=.\MSSQLLOCALDB;Initial Catalog=StripClub;Integrated Security=True";
            // получаем строку подключения
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Console.WriteLine(connectionString);

            Console.Read();
        }
    }
}