using ExtensionLibrary;
using Grpc.Core;

namespace _02_GRPC_Server;

internal class TimeDisplayerService : TimeDisplayer.TimeDisplayerBase
{
    public override Task<DisplayTimeReply> DisplayTime(
        DisplayTimeRequest request, 
        ServerCallContext context)
    {
        var result = request.WantsTime
            ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            : DateTime.Now.ToString("yyyy-MM-dd");
        result.Dump();

        return Task.FromResult(new DisplayTimeReply
        {
            Message = $"I printed {result}"
        });
    }
}