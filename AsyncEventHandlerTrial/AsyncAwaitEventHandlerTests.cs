using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncEventHandlerTrial
{
    [Explicit]
    public class AsyncAwaitEventHandlerTests
    {
        private static readonly Random Random = new Random();

        [Test]
        public async Task Firing10Events_ThreeAsyncSubscribersWithDifferentExecutionTime_ShowsAsyncAwaitBehaviour()
        {
            IEnumerable<IPlatformMessageContractHandler> handlers = new[]
            {
                GetHandler("A"),
                GetHandler("B"),
                GetHandler("C")
            };

            Console.WriteLine("Start firing 10 events");
            for (var i = 0; i < 10; i++)
                await RunHandlersAsync(handlers, i);
            Console.WriteLine("Finished firing 10 events");
        }

        private static async Task RunHandlersAsync(IEnumerable<IPlatformMessageContractHandler> handlers, int i)
        {
            foreach (var handler in handlers)
                await handler.HandleAsync(i).ConfigureAwait(false);
        }

        private static IPlatformMessageContractHandler GetHandler(string context)
        {
            return new PlatformMessageContractHandler(context);
        }

        private interface IPlatformMessageContractHandler
        {
            Task HandleAsync(int i);
        }

        private class PlatformMessageContractHandler : IPlatformMessageContractHandler
        {
            private readonly string _context;

            public PlatformMessageContractHandler(string context)
            {
                _context = context;
            }

            public async Task HandleAsync(int i)
            {
                await Task.Delay(Random.Next(100));
                Console.WriteLine($"Handler {_context}: event #{i + 1} done");
            }
        }
    }
}