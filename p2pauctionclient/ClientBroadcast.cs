using Grenache;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using SharedKernel.Conf;
using SharedKernel.Models;

namespace p2pgrpcauction.Client;

public class ClientBroadcast
{
    private Link _link;
    private readonly ILogger<ClientBroadcast> _logger;
    private IServerConf _serverConf;

    public ClientBroadcast(Link link, ILogger<ClientBroadcast> logger, IServerConf serverConf)
    {
        _link = link;
        _logger = logger;
        _serverConf = serverConf;
    }

    public async Task AnnounceAcutionAsync(AuctionRecord auctionRecord)
    {
        await foreach(var server in GetServersToBoradCast())
        {
            try
            {
                // The port number must match the port of the gRPC server.
                using var channel = GrpcChannel.ForAddress($"http://{server}");
                var client = new p2pgrpcauctionclient.Auction.AuctionClient(channel);
                var reply = await client.InitiateAuctionAsync(new p2pgrpcauctionclient.AuctionRequest()
                {
                    Amount = auctionRecord.Amount,
                    Goodid = auctionRecord.GoodId,
                    Userid = auctionRecord.UserId
                });
                _logger.LogInformation($"Auction initiated sent to server {server}");
            }
            catch (Exception e)
            {
                // ignored
                // TODO: Better manage of clients that cannot be connected;
            }
        }
    }

    public async Task AnnounceBidAsync(BidRecord bidrecord)
    {
        await foreach(var server in GetServersToBoradCast())
        {
            try
            {
                // The port number must match the port of the gRPC server.
                using var channel = GrpcChannel.ForAddress($"http://{server}");
                var client = new p2pgrpcauctionclient.Auction.AuctionClient(channel);
                var reply = await client.BidAsync(new p2pgrpcauctionclient.BidRequest()
                {
                    Amount = bidrecord.Amount,
                    Goodid = bidrecord.GoodId,
                    Userid = bidrecord.UserId
                });
                _logger.LogInformation($"Bid sent to participant server {server}");
            }
            catch (Exception e)
            {
                // ignored
                // TODO: Better manage of clients that cannot be connected;
            }
        }
    }

    private async IAsyncEnumerable<string> GetServersToBoradCast()
    {
        var announcedServers = await _link.Lookup("auction");
        
        foreach(var server in announcedServers)
        {
            if(_serverConf.GrenacheServerAddress == server) continue; // skip the server that is broadcasting
            yield return server;
        }
    }
}