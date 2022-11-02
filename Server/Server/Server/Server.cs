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
        using var streamWriter = new StreamWriter(stream);

        if (!Directory.Exists(path))
        {
            await streamWriter.WriteAsync("-1");
            await streamWriter.FlushAsync();
            return;
        }

        var directories = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path);
        var size = directories.Length + files.Length;
        await streamWriter.WriteAsync(size.ToString());
        await streamWriter.FlushAsync();

        foreach (var file in files)
        {
            await streamWriter.WriteAsync($" {file} false");
            await streamWriter.FlushAsync();
        }

        foreach (var directory in directories)
        {
            await streamWriter.WriteAsync($" {directory} true");
            await streamWriter.FlushAsync();
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
        using var streamWriter = new StreamWriter(stream);
        if (!File.Exists(path))
        {
            await streamWriter.WriteAsync("-1");
            await streamWriter.FlushAsync();
            return;
        }
        var size = (new FileInfo(path)).Length;
        await streamWriter.WriteLineAsync(size.ToString());
        await streamWriter.FlushAsync();
        using var fileStream = new FileStream(path, FileMode.Open);
        await fileStream.CopyToAsync(streamWriter.BaseStream);
        await streamWriter.FlushAsync();
    }

    /// <summary>
    /// Function to start the server (it will start listening for connections from clients)
    /// </summary>
    /// <returns></returns>
    public async Task Start(CancellationTokenSource source)
    {
        var tcpListener = new TcpListener(address, port);
        tcpListener.Start();

        while (!source.Token.IsCancellationRequested)
        {
            using var acceptedSocket = tcpListener.AcceptSocket();
            using var newtworkStream = new NetworkStream(acceptedSocket);
            using var streamReader = new StreamReader(newtworkStream);
            var strings = (streamReader.ReadLine())?.Split(' ');
            if (strings == null)
            {
                continue;
            }
            switch (strings[0])
            {
                case "list":
                {
                    await Task.Run(() => List(newtworkStream, strings[1]));
                    break;
                }
                case "get":
                {
                    await Task.Run(() => Get(newtworkStream, strings[1]));
                    break;
                }
                default:
                {
                    continue;
                }
            }
        }

        tcpListener.Stop();
    }
}