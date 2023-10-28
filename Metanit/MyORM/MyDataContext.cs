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
        var properties = type?.GetProperties().Where(prop => !prop.Name.Equals("id", StringComparison.OrdinalIgnoreCase));
        connection.Open();

        var command = new SqlCommand();
        command.Connection = connection;
        var sb = new StringBuilder();
        
        sb.AppendFormat("INSERT INTO \"{0}\" (", tableName);

        foreach (var prop in properties!)
        {
            sb.Append($"\"{prop.Name}\",");
        }

        sb.Length--;
        sb.Append(") VALUES (");

        foreach (var prop in properties)
        {
            var param = new SqlParameter($"@{prop.Name}", prop.GetValue(entity));
            sb.Append($"@{prop.Name},");
            command.Parameters.Add(param);
        }

        sb.Length--;
        sb.Append(");");
        connection.Close();
        return command.ExecuteNonQuery() > 0;
    }

    public bool Update<T>(T entity)
    {
        var type = entity?.GetType();
        var tableName = type?.Name;
        var id = type?.GetProperty("id");
        var props = type!.GetProperties()
            .Where(x => !x.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var sqlExpression = $"SELECT * FROM \"{tableName}\" WHERE \"id\" = {id?.GetValue(entity)}";
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

        var commandBuilder = new SqlCommandBuilder(adapter);
        adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
        adapter.Update(dataSet);

        return true;
    }

    public bool Delete<T>(int id)
    {
        var type = typeof(T);
        var tableName = type.Name;

        var sqlExpression = $"DELETE FROM \"{tableName}\" WHERE \"id\" = {id} ";
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var command = new SqlCommand(sqlExpression, connection);
        var number = command.ExecuteNonQuery();
        Console.WriteLine("Deleted {0} object by id: {1}", number, id);
        return true;
    }

    public List<T> Select<T>(T entity)
    {
        var type = entity?.GetType();
        var tableName = type?.Name;

        var sqlExpression = $"SELECT * FROM \"{tableName}\"";
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
        var sqlExpression = $"SELECT * FROM \"{tableName}\"WHERE \"id\" = {id} ";
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
