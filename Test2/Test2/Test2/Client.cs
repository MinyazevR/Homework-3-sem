namespace Server;

using System.Net.Sockets;
using System.Net;

/// <summary>
/// Сlass representing the client
/// </summary>
public class Client
{
    private readonly int port;
    private readonly IPAddress address;

    private readonly CancellationTokenSource source = new();

    public CancellationTokenSource GetSource => source;

    /// <summary>
    /// Сonstructor
    /// </summary>
    /// <param name="adress">ip adress</param>
    /// <param name="port">port</param>
    public Client(IPAddress adress, int port)
    {
        this.port = port;
        this.address = adress;
    }

    // Принять сообщение
    private async Task GetMessage(NetworkStream stream) {
        using var streamReader = new StreamReader(stream);
        var data = (await streamReader.ReadLineAsync());
        while (data != "exit") {
            Console.WriteLine($"{data}");
            data = (await streamReader.ReadLineAsync());

            if (source.IsCancellationRequested)
            {
                break;
            }
        }

        source.Cancel();
    }

    // Отправить сообщение
    private async Task SendMessage(NetworkStream stream)
    {
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true};
        var data = Console.ReadLine();
        while (data != "exit")
        {
            await streamWriter.WriteLineAsync($"{data}");

            if (source.IsCancellationRequested)
            {
                break;
            }
        }

        source.Cancel();
    }

    /// <summary>
    /// Запуск клиентской части
    /// </summary>
    public async void Start() {
        var client = new TcpClient();
        // Подключаемся к узлу
        await client.ConnectAsync(address, port);

        while (!source.Token.IsCancellationRequested)
        {
            // Получаем поток для записи и чтения
            using var stream = client.GetStream();
            Task a = Task.Run(() => SendMessage(stream));
            Task b = Task.Run(() => GetMessage(stream));
            if (source.IsCancellationRequested)
            {
                break;
            }
        }

        client.Close();
    }
}
