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

    /// <summary>
    /// Method for listing files
    /// </summary>
    /// <param name="stream">stream</param>
    /// <param name="path">path to directory</param>
    /// <returns></returns>
    public async Task<(int, List<(string, bool)>)> List(string pathToDiretory)
    {
        var client = new TcpClient();
        await client.ConnectAsync(address, port);
        using var stream = client.GetStream();
        using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteLineAsync($"list {pathToDiretory}");
        await streamWriter.FlushAsync();
        using var streamReader = new StreamReader(stream);
        var data = await streamReader.ReadLineAsync();
        if (data == null)
        {
            throw new InvalidDataException();
        }
        var strings = data.Split(' ');
        if (!int.TryParse(strings[0], out int size))
        {
            throw new InvalidDataException();
        }
        if (size == -1)
        {
            throw new DirectoryNotFoundException();
        }
        var list = new List<(string, bool)>();
        for (int i = 1; i < strings.Length; i++)
        {
            bool flag = true;
            if (strings[i + 1] == "false")
            {
                flag = false;
            }
            list.Add((strings[i], flag));
            i++;
        }

        return (size, list);
    }

    /// <summary>
    /// Method for get files
    /// </summary>
    /// <param name="stream">stream</param>
    /// <param name="path">path to file</param>
    /// <returns></returns>
    public async Task<(int, byte[])> Get(string pathToFile)
    {
        var client = new TcpClient();
        await client.ConnectAsync(address, port);
        using var stream = client.GetStream();
        using var streamWriter = new StreamWriter(stream) { AutoFlush = true };
        await streamWriter.WriteLineAsync($"get {pathToFile}");
        using var streamReader = new StreamReader(stream);
        var stringWithSize = (await streamReader.ReadLineAsync());
        if (!int.TryParse(stringWithSize, out int size))
        {
            throw new InvalidDataException();
        }
        if (size == -1)
        {
            throw new FileNotFoundException();
        }
        var buffer = new byte[size];
        await streamReader.BaseStream.ReadAsync(buffer, 0, size);
        return (size, buffer);
    }
}
