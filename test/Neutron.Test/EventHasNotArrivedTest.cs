using System;
using Neutron.Core;
using Xunit;

namespace Neutron.Test
{
    public class EventHasNotArrivedTest
    {
        [Fact]
        public void Should_Be_True_When_Target_Has_Not_Arrived()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);

            Event @event = new Event(Guid.NewGuid(), "Tomorrow", tomorrow);

            Assert.True(@event.HasNotArrived);
        }

        [Fact]
        public void Should_Be_False_When_Target_Has_Arrived()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);

            Event @event = new Event(Guid.NewGuid(), "Yesterday", yesterday);

            Assert.False(@event.HasNotArrived);
        }
    }
}
