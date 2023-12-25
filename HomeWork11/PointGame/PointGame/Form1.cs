using PointGame.Paths;
using System.Net.Sockets;
using System.Text.Json;

namespace PointGame
{
    public partial class Form1 : Form
    {
        private TcpClient _client = null!;
        private StreamReader _reader = null!;
        private StreamWriter _writer = null!;

        public Form1()
        { 
            InitializeComponent();
            listOfUsers.Visible = false;
            testLabel.Visible = false;
            color.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btn_signIn_Click(object sender, EventArgs e)
        {
            const string host = "127.0.0.1";
            const int port = 8888;
            var userName = enterName.Text;

            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(host, port);

                _reader = new StreamReader(_client.GetStream());
                _writer = new StreamWriter(_client.GetStream()) { AutoFlush = true };

                // запускаем новый поток для получения данных
                Task.Run(() => ReceiveMessageAsync(_reader));

                // отправляем имя пользователя
                await EnterUserAsync(_writer, userName);

                // обновляем интерфейс
                testLabel.Text = userName;
                label1.Visible = false;
                enterName.Visible = false;
                btn_signIn.Visible = false;
                listOfUsers.Visible = true;
                testLabel.Visible = true;
                color.Visible = true;

                var rand = new Random();
                var user = new AddUser(enterName.Text);

                var jsoncolor = JsonSerializer.Serialize(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($@"Ошибка подключения: {ex.Message}");
            }
        }

        private async Task ReceiveMessageAsync(StreamReader reader)
        {
            while (true)
            {
                try
                {
                    // считываем ответ в виде строки
                    var message = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(message)) continue;

                    // обновляем интерфейс с использованием Invoke, так как это происходит в отдельном потоке
                    Invoke((MethodInvoker)delegate
                    {
                        Print(message);
                    });
                }
                catch (IOException)
                {
                    // Исключение возникает, если считывание из закрытого потока
                    // может произойти, если сервер отключил клиента
                    MessageBox.Show(@"Сервер отключил клиента.");
                    break;
                }
                catch (Exception)
                {
                    // Любые другие исключения, которые могут возникнуть при считывании
                }
            }
        }

        // чтобы полученное сообщение не накладывалось на ввод нового сообщения
        private void Print(string message)
        {
            var users = JsonSerializer.Deserialize<List<string>>(message) 
            ?? throw new ArgumentNullException(nameof(message));

            listOfUsers.Items.Clear();
            foreach (var user in users)
                listOfUsers.Items.Add(user);
            
        }

        private static async Task EnterUserAsync(StreamWriter writer, string userName)
        {
            // сначала отправляем имя
            await writer.WriteLineAsync(userName);
            await writer.FlushAsync();
            //Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

            //while (true)
            //{
            //    string? message = Console.ReadLine();
            //    await writer.WriteLineAsync(message);
            //    await writer.FlushAsync();
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _reader.Close();
            _writer.Close();
            _client.Close();
        }
    }
}