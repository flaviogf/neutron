using System;
using Neutron.Core;
using Xunit;

namespace Neutron.Test
{
    public class EventHasArrivedTest
    {
        [Fact]
        public void Should_Be_True_When_Target_Has_Arrived()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);

            Event @event = new Event(Guid.NewGuid(), "Yesterday", yesterday);

            Assert.True(@event.HasArrived);
        }

        [Fact]
        public void Should_Be_False_When_The_Target_Has_Not_Arrived()
        {
            DateTime tomorrow = DateTime.Now.AddDays(1);

            Event @event = new Event(Guid.NewGuid(), "Tomorrow", tomorrow);

            Assert.False(@event.HasArrived);
        }
    }
}
