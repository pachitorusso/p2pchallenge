using SharedKernel.Models;

namespace p2pgrpcauction.Infrastructure;

public interface IAuctionCoreService
{
    Task CreateAuction(AuctionRecord auction);
    
    Task Bid(BidRecord bidRequest);
    
    Task AddBid(BidRecord bidRequest);
}