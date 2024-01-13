namespace SharedKernel.Models;

public record AuctionRecord(string UserId, string GoodId, int Amount, HashSet<BidRecord>? Bids = null!)
{
    public HashSet<BidRecord> Bids { get; init; } = Bids ?? new HashSet<BidRecord>();
    public BidRecord? HighestBid => Bids.OrderByDescending(x => x.Amount).FirstOrDefault();
    
    public int MinimumBid => HighestBid?.Amount + 1 ?? Amount;
    
    public void AddBid(BidRecord bid)
    {
        Bids.Add(bid);
    }
};



