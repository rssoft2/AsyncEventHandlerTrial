using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AsyncEventHandlerTrial
{
    [Explicit]
    public class AsyncVoidEventHandlerTests
    {
        private static readonly Random Random = new Random();

        [Test]
        public async Task Firing10Events_ThreeAsyncVoidSubscribersWithDifferentExecutionTime_ShowsFireAndForgetBehaviour()
        {
            var myEventSource = new MyEventSource();
            myEventSource.MyEvent += (sender, i) => MyEventHandler("A", i);
            myEventSource.MyEvent += (sender, i) => MyEventHandler("B", i);
            myEventSource.MyEvent += (sender, i) => MyEventHandler("C", i);

            Console.WriteLine("Start firing 10 events");
            for (var i = 0; i < 10; i++)
                myEventSource.RaiseMyEvent(i);
            Console.WriteLine("Finished firing 10 events");

            await Task.Delay(5000);
        }

        private static async void MyEventHandler(string context, int i)
        {
            await Task.Delay(Random.Next(100));
            Console.WriteLine($"Handler {context}: event #{i + 1} done");
        }

        private class MyEventSource
        {
            public event EventHandler<int> MyEvent;

            public void RaiseMyEvent(int eventArgs)
            {
                MyEvent?.Invoke(this, eventArgs);
            }
        }
    }
}