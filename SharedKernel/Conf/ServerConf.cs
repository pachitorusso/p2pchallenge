namespace SharedKernel.Conf;

public class ServerConf : IServerConf
{
    private int _port;
    public ServerConf()
    {
        _port = new Random().Next(5000, 6000);
    }

    public int Port => _port;
    public string GrenacheServerAddress => $"127.0.0.1:{Port}";
    public string Url => $"http://{GrenacheServerAddress}";
}