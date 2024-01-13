using Grpc.Core;
using p2pgrpcauction.Infrastructure;
using SharedKernel.Models;


namespace p2pgrpcauction.Services;

public class AuctionRpcService : Auction.AuctionBase
{
    private readonly ILogger<AuctionRpcService> _logger;
    private readonly IAuctionCoreService _auctionCoreService;

    public AuctionRpcService(ILogger<AuctionRpcService> logger, IAuctionCoreService auctionCoreService)
    {
        _logger = logger;
        _auctionCoreService = auctionCoreService;
    }

    /// <inheritdoc />
    public override async Task<AuctionResponse> InitiateAuction(AuctionRequest request, ServerCallContext context)
    {
        var auctionRecord = new AuctionRecord(request.Userid, request.Goodid,request.Amount);
        
        await _auctionCoreService.CreateAuction(auctionRecord);
        
        _logger.LogInformation($"Auction initiated. Data {auctionRecord}");
        
        return new AuctionResponse()
        {
            Amount = auctionRecord.Amount,
            Goodid = auctionRecord.GoodId,
            Userid = auctionRecord.UserId
        };
    }

    /// <inheritdoc />
    public override async Task<BidRequest> Bid(BidRequest request, ServerCallContext context)
    {
        var bidRequest = new BidRecord(request.Userid, request.Goodid,request.Amount);
        
        await _auctionCoreService.AddBid(bidRequest);
        
        _logger.LogInformation($"Bid received. Data {bidRequest}");

        return request;
    }
}