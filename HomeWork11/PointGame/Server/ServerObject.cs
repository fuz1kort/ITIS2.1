using System.Net.Sockets;
using System.Net;
using System.Text.Json;

class ServerObject
{
    readonly TcpListener tcpListener = new(IPAddress.Any, 8888); // сервер для прослушивания
    readonly List<ClientObject> clients = new(); // все подключения
    protected internal void RemoveConnection(string id)
    {
        // получаем по id закрытое подключение
        ClientObject? client = clients.FirstOrDefault(c => c.Id.Equals(id));
        // и удаляем его из списка подключений
        if (client != null) clients.Remove(client);
        client?.Close();
    }
    // прослушивание входящих подключений
    protected internal async Task ListenAsync()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();

                ClientObject clientObject = new(tcpClient, this);
                clients.Add(clientObject);
                Task.Run(clientObject.ProcessAsync);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }


    // трансляция сообщения подключенным клиентам
    protected internal async Task BroadcastMessageAsync(string message)
    {
        var usersToJson = JsonSerializer.Serialize(clients.Select(x => x.UserName).ToList());
        foreach (var client in clients)
        {
            await client.Writer.WriteLineAsync(usersToJson); //передача данных
            await client.Writer.FlushAsync();
        }
    }
    // отключение всех клиентов
    protected internal void Disconnect()
    {
        foreach (var client in clients)
        {
            client.Close(); //отключение клиента
        }
        tcpListener.Stop(); //остановка сервера
    }
}
