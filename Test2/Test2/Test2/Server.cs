namespace Server;

using System.Net.Sockets;
using System.Net;

/// <summary>
/// Class representing the server
/// </summary>
public class Server
{
    private readonly int port;

    private readonly CancellationTokenSource source = new();

    public CancellationTokenSource GetSource => source ;

    /// <summary>
    /// Сonstructor
    /// </summary>
    /// <param name="port">port</param>
    public Server(int port)
    {
        this.port = port;
    }

    /// <summary>
    /// Function to start the server (it will start listening for connections from clients)
    /// </summary>
    /// <returns></returns>
    public async Task Start()
    {
        var tcpListener = new TcpListener(IPAddress.Any, port);

        // Слушаем порт
        tcpListener.Start();

        while (!source.Token.IsCancellationRequested)
        {
            // Блокируем поток до установления соединения
            var acceptedSocket = await tcpListener.AcceptSocketAsync();
            // Поток для записи и чтения в полученный сокет
            using var newtworkStream = new NetworkStream(acceptedSocket);
            Task a = Task.Run(() => SendMessage(newtworkStream));
            Task b = Task.Run(() => GetMessage(newtworkStream));
            if (source.IsCancellationRequested) {
                acceptedSocket.Close();
                break;
            }
        }

        tcpListener.Stop();
    }

    // Принять сообщение
    private async Task GetMessage(NetworkStream stream)
    {
        using var streamReader = new StreamReader(stream);
        var data = (await streamReader.ReadLineAsync());
        while (data != "exit")
        {
            Console.WriteLine($"{data}");
            data = (await streamReader.ReadLineAsync());

            if (source.IsCancellationRequested)
            {
                return;
            }
        }

        source.Cancel();
    }

    // Отправить сообщение
    private async Task SendMessage(NetworkStream stream)
    {
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };
        var data = Console.ReadLine();
        while (data != "exit")
        {
            await streamWriter.WriteLineAsync($"{data}");

            if (source.IsCancellationRequested) {
                return;
            }
        }

        source.Cancel();
    }
}