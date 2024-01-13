namespace SharedKernel.Conf;

public interface IServerConf
{
    int Port { get; }
    string GrenacheServerAddress { get; }
    string Url { get; }
}