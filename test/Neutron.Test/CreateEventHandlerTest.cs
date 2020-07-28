using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Moq;
using Neutron.Application;
using Xunit;

namespace Neutron.Test
{
    public class CreateEventHandlerTest
    {
        [Fact]
        public async Task Should_Return_Success()
        {
            CreateEventInput input = new CreateEventInput(Guid.NewGuid(), "Christmas", new DateTime(year: 2020, month: 12, day: 25));

            var eventRepository = new Mock<IEventRepository>();

            var createEvent = new CreateEvent(eventRepository.Object);

            Result<CreateEventOutput> result = await createEvent.Handle(input, CancellationToken.None);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task When_Id_Is_Not_Passed_Should_Return_Failure()
        {
            CreateEventInput input = new CreateEventInput(Guid.Empty, "Christmas", new DateTime(year: 2020, month: 12, day: 25));

            var eventRepository = new Mock<IEventRepository>();

            var createEvent = new CreateEvent(eventRepository.Object);

            Result<CreateEventOutput> result = await createEvent.Handle(input, CancellationToken.None);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task When_Name_Is_Not_Passed_Should_Return_Failure()
        {
            CreateEventInput input = new CreateEventInput(Guid.NewGuid(), string.Empty, new DateTime(year: 2020, month: 12, day: 25));

            var eventRepository = new Mock<IEventRepository>();

            var createEvent = new CreateEvent(eventRepository.Object);

            Result<CreateEventOutput> result = await createEvent.Handle(input, CancellationToken.None);

            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task When_Target_Is_Not_Passed_Should_Target_Be_Equal_Midnight_Of_Today()
        {
            CreateEventInput input = new CreateEventInput(Guid.NewGuid(), "Christmas", null);

            var eventRepository = new Mock<IEventRepository>();

            var createEvent = new CreateEvent(eventRepository.Object);

            Result<CreateEventOutput> result = await createEvent.Handle(input, CancellationToken.None);

            Assert.Equal(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0), result.Value.Target);
        }
    }
}
