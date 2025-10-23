using DxFeed.Graal.Net.Api;
using DxFeed.Graal.Net.Events.Market;
using System.Threading.Tasks.Dataflow;

namespace MAUI.Demo.Services;

public class DxFeedService : IDisposable
{
    private readonly BufferBlock<QuoteUpdate> _dataStream = new();
    private readonly DXEndpoint _dxEndpoint;
    private readonly DXFeedSubscription _dxFeedSubscription;
    private CancellationTokenSource _cts = new();

    public DxFeedService()
    {
        _dxEndpoint = DXEndpoint.Create().Connect("demo.dxfeed.com:7300");
        _dxFeedSubscription = _dxEndpoint.GetFeed().CreateSubscription(typeof(Quote));
    }

    public async Task Subscribe(string symbol, Action<QuoteUpdate> action)
    {
        _dxFeedSubscription.AddEventListener(events =>
        {
            foreach (var ev in events)
            {
                var quote = new QuoteUpdate(ev.EventSymbol, ((Quote)ev).AskPrice, ((Quote)ev).BidPrice);
                _dataStream.Post(quote);
            }
        });

        _dxFeedSubscription.AddSymbols([symbol]);

        var actionBlock = new ActionBlock<QuoteUpdate>(action);
        using (_dataStream.LinkTo(actionBlock))
        {
            await _dataStream.Completion;
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _dxEndpoint.Dispose();
        _dxFeedSubscription.Dispose();
        _dataStream.Complete();
    }
}

public record QuoteUpdate(string Symbol, double Ask, double Bid);