using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MyORM;

public class MyDataContext : IMyDataContext
{
    private readonly string _connectionString;

    public MyDataContext(string connectionString) => _connectionString = connectionString;
    
    public bool Add<T>(T entity)
    {
        using var connection = new SqlConnection(_connectionString);
        var type = entity?.GetType();
        var tableName = type?.Name;
        var properties = type!.GetProperties().Where(prop => !prop.Name.Equals("id"));
        // var properties = type!.GetProperties();
        connection.Open();

        var command = new SqlCommand();
        command.Connection = connection;
        var query = new StringBuilder();
        var exp = $"INSERT INTO {tableName}s (";
        query.Append(exp);
        foreach (var prop in properties)
        {
            query.Append($"{prop.Name},");
        }

        query.Length--;

        Console.WriteLine(query);
        query.Append(") VALUES (");

        foreach (var prop in properties)
        {
            query.Append($"\'{prop.GetValue(entity)}\',");
        }

        query.Length--;
        query.Append(")");
        Console.WriteLine(query.ToString());
        command.CommandText = query.ToString();
        command.ExecuteNonQuery();
        connection.Close();
        return true;
    }

    public bool Update<T>(T entity)
    {
        var type = entity?.GetType();
        var tableName = type?.Name;
        var id = type?.GetProperty("Id");
        var props = type!.GetProperties()
            .Where(x => !x.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var sqlExpression = $"SELECT * FROM {tableName}s WHERE \'id\' = {id?.GetValue(entity)}";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var adapter = new SqlDataAdapter(sqlExpression, connection);
        var dataSet = new DataSet();
        adapter.Fill(dataSet);

        var entityFromDatabase = dataSet.Tables[0];
        var rowToUpdate = entityFromDatabase.Rows[0];

        foreach (var prop in props)
        {
            var val = prop.GetValue(entity);
            rowToUpdate[prop.Name] = val ?? DBNull.Value;
        }

        // var commandBuilder = new SqlCommandBuilder(adapter);
        // adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
        adapter.Update(dataSet);

        return true;
    }

    public bool Delete<T>(int id)
    {
        var type = typeof(T);
        var tableName = type.Name;

        var sqlExpression = $"DELETE FROM {tableName}s WHERE id = \'{id}\'";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(sqlExpression, connection);
        command.ExecuteNonQuery();
        Console.WriteLine("Deleted object by id: {0}", id);
        return true;
    }

    public List<T> Select<T>(T entity)
    {
        var type = entity?.GetType();
        var tableName = type?.Name;

        var sqlExpression = $"SELECT * FROM \"{tableName}s\"";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var adapter = new SqlDataAdapter(sqlExpression, connection);
        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var listOfTableItems = dataSet.Tables[0];
        var listOfEntities = new List<T>();

        foreach (DataRow row in listOfTableItems.Rows)
        {
            var objOfEntity = Activator.CreateInstance<T>();
            foreach (DataColumn column in listOfTableItems.Columns)
            {
                var prop = type!.GetProperty(column.ColumnName);
                if (prop != null && row[column] != DBNull.Value)
                    prop.SetValue(objOfEntity, row[column]);
            }

            listOfEntities.Add(objOfEntity);
        }

        return listOfEntities;
    }

    public T SelectById<T>(int id)
    {
        var tableName = typeof(T).Name;
        var type = typeof(T);
        var sqlExpression = $"SELECT * FROM {tableName}s WHERE id = \'{id}\'";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var adapter = new SqlDataAdapter(sqlExpression, connection);
        var dataSet = new DataSet();
        adapter.Fill(dataSet);
        var listOfArgs = dataSet.Tables[0];
            
        foreach (DataRow row in listOfArgs.Rows)
        {
            var entity = Activator.CreateInstance<T>();
            foreach (DataColumn column in listOfArgs.Columns)
            {
                var prop = type.GetProperty(column.ColumnName);
                if (prop != null && row[column] != DBNull.Value)
                {
                    prop.SetValue(entity, row[column]);
                }
            }

            return entity;
        }

        return default!;
    }
}
