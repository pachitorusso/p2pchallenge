namespace SharedKernel.Models;

public class AuctionInMemoryRepository : IAuctionRepository
{
    private HashSet<AuctionRecord> _openAuctions;

    public AuctionInMemoryRepository()
    {
        _openAuctions = new HashSet<AuctionRecord>();
    }
    
    public Task<AuctionRecord?> GetAuction(string userId, string goodId)
    {
        var auction = _openAuctions.FirstOrDefault(x => x.GoodId == goodId && x.UserId == userId);
        
        return Task.FromResult(auction);
    }

    /// <inheritdoc />
    public Task<AuctionRecord?> GetAuction(string goodId)
    {
        var auction = _openAuctions.FirstOrDefault(x => x.GoodId == goodId);
        
        return Task.FromResult(auction);
    }

    /// <inheritdoc />
    public Task Add(AuctionRecord auction)
    {
        _openAuctions.Add(auction);
        return Task.CompletedTask;
    }
}