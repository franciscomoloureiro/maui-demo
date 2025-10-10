using System.Threading.Tasks.Dataflow;

namespace MAUI.Demo.Services;

// this tries to mimic connection to async data source like websocket or mq
public class LiveDataService
{
    private readonly BufferBlock<int> _dataStream = new();
    private CancellationTokenSource _cts = new();

    public void Start()
    {
        Task.Run(async () =>
        {
            var rnd = new Random();
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(1000);
                int value = rnd.Next(0, 100);
                _dataStream.Post(value);
            }
        }, _cts.Token);
    }

    public async Task Subscribe(Action<int> action)
    {
        var actionBlock = new ActionBlock<int>(action);
        using (_dataStream.LinkTo(actionBlock))
        {
            await _dataStream.Completion;
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        _dataStream.Complete();
    }
}