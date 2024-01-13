namespace SharedKernel.Models;

public interface IAuctionRepository
{
    Task<AuctionRecord?> GetAuction(string userId, string goodId);
    Task<AuctionRecord?> GetAuction(string goodId);
    Task Add(AuctionRecord auction);
}