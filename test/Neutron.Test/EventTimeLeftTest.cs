using System;
using Neutron.Core;
using Xunit;

namespace Neutron.Test
{
    public class EventTimeLeftTest
    {
        [Fact]
        public void Should_Return_Zero_When_The_Target_Has_Arrived()
        {
            DateTime yesterday = DateTime.Today.AddDays(-1);

            Event @event = new Event(Guid.NewGuid(), "Yesterday", yesterday);

            Assert.Equal(TimeSpan.Zero, @event.TimeLeft);
        }

        [Fact]
        public void Should_Return_Exactly_TimeLeft_When_The_Target_Has_Not_Arrived()
        {
            DateTime tomorrow = DateTime.Today.AddDays(1);

            Event @event = new Event(Guid.NewGuid(), "Tomorrow", tomorrow);

            Assert.NotEqual(TimeSpan.Zero, @event.TimeLeft);
        }
    }
}
