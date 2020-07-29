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
    public class DestroyEventHandlerTest
    {
        [Fact]
        public async Task ShouldReturnSuccess()
        {
            var eventRepository = new Mock<IEventRepository>();

            Maybe<Event> maybeEvent = new Event(Guid.NewGuid(), "Christmas", new DateTime(year: 2020, month: 6, day: 25));

            var destroyEvent = new DestroyEvent(Guid.NewGuid());

            eventRepository.Setup(it => it.FindById(It.IsAny<Guid>())).Returns(Task.FromResult(maybeEvent));

            var handler = new DestroyEventHandler(eventRepository.Object);

            Result result = await handler.Handle(destroyEvent, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task When_The_Event_Does_Not_Exist_Should_Return_Failure()
        {
            var eventRepository = new Mock<IEventRepository>();

            Maybe<Event> maybeEvent = null;

            eventRepository.Setup(it => it.FindById(It.IsAny<Guid>())).Returns(Task.FromResult(maybeEvent));

            var destroyEvent = new DestroyEvent(Guid.NewGuid());

            var handler = new DestroyEventHandler(eventRepository.Object);

            Result result = await handler.Handle(destroyEvent, CancellationToken.None);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task When_The_Event_Is_Not_Removed_Should_Return_Failure()
        {
            var eventRepository = new Mock<IEventRepository>();

            Maybe<Event> maybeEvent = new Event(Guid.NewGuid(), "Christmas", new DateTime(year: 2020, month: 6, day: 25));

            eventRepository.Setup(it => it.FindById(It.IsAny<Guid>())).Returns(Task.FromResult(maybeEvent));

            eventRepository.Setup(it => it.Remove(It.IsAny<Event>())).Returns(Task.FromResult(Result.Failure("Something has gone wrong...")));

            var destroyEvent = new DestroyEvent(Guid.NewGuid());

            var handler = new DestroyEventHandler(eventRepository.Object);

            Result result = await handler.Handle(destroyEvent, CancellationToken.None);

            Assert.True(result.IsFailure);
        }
    }
}
