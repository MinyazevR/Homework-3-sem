namespace Server;

using System.Net.Sockets;
using System.Net;

/// <summary>
/// Class representing the server
/// </summary>
public class Server
{
    private readonly int port;
    private readonly IPAddress address;

    /// <summary>
    /// Сonstructor
    /// </summary>
    /// <param name="adress">ip adress</param>
    /// <param name="port">port</param>
    public Server(IPAddress adress, int port)
    {
        this.address = adress;
        this.port = port;
    }

    /// <summary>
    /// Method for listing files
    /// </summary>
    /// <param name="stream">stream</param>
    /// <param name="path">path to directory</param>
    /// <returns></returns>
    private static async Task List(NetworkStream stream, string path)
    {
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };

        if (!Directory.Exists(path))
        {
            await streamWriter.WriteAsync("-1");
            return;
        }

        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var size = directories.Length + files.Length;

        await streamWriter.WriteAsync(size.ToString());

        foreach (var file in files)
        {
            await streamWriter.WriteAsync($" {file} false");
        }

        foreach (var directory in directories)
        {
            await streamWriter.WriteAsync($" {directory} true");
        }
    }

    /// <summary>
    /// Method for get files
    /// </summary>
    /// <param name="stream">stream</param>
    /// <param name="path">path to file</param>
    /// <returns></returns>
    private static async Task Get(NetworkStream stream, string path)
    {
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };
        if (!File.Exists(path))
        {
            await streamWriter.WriteAsync("-1");
            return;
        }

        var size = (new FileInfo(path)).Length;
        await streamWriter.WriteLineAsync(size.ToString());
        using var fileStream = new FileStream(path, FileMode.Open);
        await fileStream.CopyToAsync(streamWriter.BaseStream);
    }

    /// <summary>
    /// Function to start the server (it will start listening for connections from clients)
    /// </summary>
    /// <returns></returns>
    public async Task Start(CancellationTokenSource source)
    {
        var tcpListener = new TcpListener(address, port);

        // Слушаем порт
        tcpListener.Start();

        while (!source.Token.IsCancellationRequested)
        {
            // Блокируем поток до установления соединения
            var acceptedSocket = await tcpListener.AcceptSocketAsync();

            // Каждый клиент обслуживается в своем потоке. Т.к. async, то не будет блокировок при чтении больших файлов
            await Task.Run(async() =>
            {
                // Поток для записи и чтения в полученный сокет
                using var newtworkStream = new NetworkStream(acceptedSocket);

                // Получаем сообщение от клиента
                using var streamReader = new StreamReader(newtworkStream);
                var strings = (streamReader.ReadLine())?.Split(' ');
                if (strings == null)
                {
                    throw new InvalidDataException();
                }

                switch (strings[0])
                {
                    case "list":
                    {
                        await List(newtworkStream, strings[1]);
                        break;
                    }
                    case "get":
                    {
                        await Get(newtworkStream, strings[1]);
                        break;
                    }
                    default:
                    {
                       throw new InvalidDataException();
                    }
                }

                acceptedSocket.Close();
            });
        }

        tcpListener.Stop();
    }
}