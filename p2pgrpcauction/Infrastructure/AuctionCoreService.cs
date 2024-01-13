using p2pgrpcauction.Client;
using SharedKernel.Models;

namespace p2pgrpcauction.Infrastructure;

public class AuctionCoreService : IAuctionCoreService
{
    private IAuctionRepository _auctionRepository;
    private ClientBroadcast _clientBroadcast;

    public AuctionCoreService(ClientBroadcast clientBroadcast, IAuctionRepository auctionRepository)
    {
        _clientBroadcast = clientBroadcast;
        _auctionRepository = auctionRepository;
    }

    public async Task CreateAuction(AuctionRecord auctionRecord)
    {
        var auction = await _auctionRepository.GetAuction(auctionRecord.UserId,auctionRecord.GoodId);
        if (auction is not null) return;
        
        await _auctionRepository.Add(auctionRecord);
        
        await _clientBroadcast.AnnounceAcutionAsync(auctionRecord);
    }

    /// <inheritdoc />
    public async Task Bid(BidRecord bidRequest)
    {
        await AddBid(bidRequest);
        
        await _clientBroadcast.AnnounceBidAsync(bidRequest);
    }

    /// <inheritdoc />
    public async Task AddBid(BidRecord bidRequest)
    {
        var auction = await _auctionRepository.GetAuction(bidRequest.GoodId);
        if (auction is null) throw new Exception("We cannnot find the auction");

        auction.AddBid(bidRequest);
    }
}