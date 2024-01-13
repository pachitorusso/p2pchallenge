using Grenache;
using p2pgrpcauction.Client;
using p2pgrpcauction.Infrastructure;
using p2pgrpcauction.Services;
using SharedKernel.Conf;
using SharedKernel.Models;

string userId = null;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton<IAuctionCoreService, AuctionCoreService>();
builder.Services.AddSingleton<IAuctionRepository, AuctionInMemoryRepository>();
builder.Services.AddSingleton<ClientBroadcast>();
builder.Services.AddSingleton<IServerConf,ServerConf>();
builder.Services.AddSingleton<User>(provider =>
{
    return new User(userId);
});
builder.Services.AddSingleton<Link>(provider => { return new Link("http://127.0.0.1:30001"); });

var app = builder.Build();

// 
app.MapGrpcService<AuctionRpcService>();

var serverConf = app.Services.GetRequiredService<IServerConf>();

app.RunAsync(serverConf.Url);

// Initiliaze a link to Grenache network
var link = app.Services.GetRequiredService<Link>();

_ = new Timer(async (_) =>
{
    await link.Announce("auction", serverConf.Port);
}, null, 0, 10000);

Console.WriteLine("Enter user id please:....");
userId = Console.ReadLine() ?? Guid.NewGuid().ToString();

Console.WriteLine("Enter an action: (initauct, bid, close) or Q to exit");

while (true)
{
    try
    {
        var inputstring = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(inputstring))
        {
            Console.WriteLine("Please enter a valid command or Q to exit");
            continue;
        }

        if (inputstring.ToUpper() == "Q") break;

        // Parse the message
        var splittedMessage = inputstring.Split(" ");
        
        var auctionService = app.Services.GetRequiredService<IAuctionCoreService>();
        var user = app.Services.GetRequiredService<User>();

        switch (splittedMessage[0])
        {
            case "initauct":
                var actionRecord = new AuctionRecord(user.Id, splittedMessage[1], Convert.ToInt32(splittedMessage[2]));
                await auctionService.CreateAuction(actionRecord);
                break;
            case "bid":
                var bidRecord = new BidRecord(user.Id, splittedMessage[1], Convert.ToInt32(splittedMessage[2]));
                await auctionService.Bid(bidRecord);
                break;
            case "close":
                // TODO: Implement the close use case
                break;
        }
        Console.WriteLine("Enter an action: (initauct, bid, close) or Q to exit");
    }
    catch (Exception e)
    {
        var logger = app.Services.GetRequiredService<ILogger>();
        logger.LogError(e.Message);
    }
}