using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Moq;
using Neutron.Application;
using Neutron.Core;
using Xunit;

namespace Neutron.Test
{
    public class CreateEventHandlerTest
    {
        [Fact]
        public async Task Should_Return_Success()
        {
            var createEvent = new CreateEvent(Guid.NewGuid(), "Christmas", new DateTime(year: 2020, month: 12, day: 25));

            var eventRepository = new Mock<IEventRepository>();

            var handler = new CreateEventHandler(eventRepository.Object);

            Result result = await handler.Handle(createEvent, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task When_The_Target_Of_The_Event_Passed_Is_Null_Should_Consider_Target_As_Midnight_Of_The_Next_Day()
        {
            var createEvent = new CreateEvent(Guid.NewGuid(), "Christmas", null);

            var eventRepository = new Mock<IEventRepository>();

            Event @event = null;

            eventRepository.Setup(it => it.Add(It.IsAny<Event>())).Callback((Event it) =>
            {
                @event = it;
            });

            var handler = new CreateEventHandler(eventRepository.Object);

            await handler.Handle(createEvent, CancellationToken.None);

            Assert.Equal(DateTime.Today.AddDays(1), @event.Target);
        }

        [Fact]
        public async Task When_The_Event_Is_Not_Added_Should_Return_Failure()
        {
            var createEvent = new CreateEvent(Guid.NewGuid(), "Christmas", new DateTime(year: 2020, month: 12, day: 25));

            var eventRepository = new Mock<IEventRepository>();

            eventRepository.Setup(it => it.Add(It.IsAny<Event>())).Returns(Task.FromResult(Result.Failure("Something has gone wrong...")));

            var handler = new CreateEventHandler(eventRepository.Object);

            Result result = await handler.Handle(createEvent, CancellationToken.None);

            Assert.True(result.IsFailure);
        }
    }
}
