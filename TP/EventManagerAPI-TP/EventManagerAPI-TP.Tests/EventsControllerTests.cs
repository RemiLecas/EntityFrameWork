using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using EventManagerAPI_TP.Controllers;
using EventManagerAPI_TP.Core.Interfaces;
using EventManagerAPI_TP.Core.DTO;
using System.Linq;

namespace EventManagerAPI_Test.Tests
{
    public class EventsControllerTests
    {
        [Fact]
        public async Task GetEvents_ReturnsOkResult_WithEventListResult()
        {
            var mockEventListService = new Mock<IEventListService>();

            var fakeEvents = new List<EventReadDTO>
            {
                new EventReadDTO { Id = 1, Title = "Event 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(2) },
                new EventReadDTO { Id = 2, Title = "Event 2", StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(1).AddHours(2) }
            };

            var fakeResult = new EventListResult
            {
                Events = fakeEvents,
                TotalPages = 1
            };

            mockEventListService.Setup(s =>
                s.GetEventsAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(fakeResult);

            var controller = new EventsController(Mock.Of<IEventService>(), mockEventListService.Object);

            var result = await controller.GetEvents(
                startDate: DateTime.Now,
                endDate: DateTime.Now.AddDays(1),
                locationId: null,
                category: null,
                status: null,
                page: 1,
                pageSize: 10
            );

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<EventListResult>(okResult.Value);

            Assert.Equal(fakeEvents.Count, returnedValue.Events.Count());
            Assert.Equal(1, returnedValue.TotalPages);
        }

        [Fact]
        public async Task GetEvents_ReturnsEmptyResult_WhenNoEventsMatchFilters()
        {
            var mockEventListService = new Mock<IEventListService>();

            var fakeResult = new EventListResult
            {
                Events = new List<EventReadDTO>(), // No events
                TotalPages = 0
            };

            mockEventListService.Setup(s =>
                s.GetEventsAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(fakeResult);

            var controller = new EventsController(Mock.Of<IEventService>(), mockEventListService.Object);

            var result = await controller.GetEvents(
                startDate: DateTime.Now.AddDays(1),
                endDate: DateTime.Now.AddDays(2),
                locationId: null,
                category: null,
                status: null,
                page: 1,
                pageSize: 10
            );

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<EventListResult>(okResult.Value);

            Assert.Empty(returnedValue.Events);
            Assert.Equal(0, returnedValue.TotalPages);
        }

        [Fact]
        public async Task GetEvents_ReturnsPaginatedResults()
        {
            var mockEventListService = new Mock<IEventListService>();

            var fakeEvents = new List<EventReadDTO>
            {
                new EventReadDTO { Id = 1, Title = "Event 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddHours(2) },
                new EventReadDTO { Id = 2, Title = "Event 2", StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(1).AddHours(2) }
            };

            var fakeResult = new EventListResult
            {
                Events = fakeEvents,
                TotalPages = 1
            };

            mockEventListService.Setup(s =>
                s.GetEventsAsync(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(fakeResult);

            var controller = new EventsController(Mock.Of<IEventService>(), mockEventListService.Object);

            var result = await controller.GetEvents(
                startDate: null,
                endDate: null,
                locationId: null,
                category: null,
                status: null,
                page: 1,
                pageSize: 1
            );

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedValue = Assert.IsType<EventListResult>(okResult.Value);

            Assert.Equal(1, returnedValue.TotalPages);
            
        }
    
        [Fact]
        public async Task CreateEvent_ReturnsCreatedAtActionResult_WhenEventIsCreated()
        {
            var mockEventService = new Mock<IEventService>();
            
            var eventCreateDTO = new EventCreateDTO
            {
                Title = "New Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(2),
                CategoryId = 1,
                Description = "A new event"
            };

            var createdEvent = new Event
            {
                Id = 1,
                Title = "New Event",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(2),
                CategoryId = 1,
                Description = "A new event"
            };

            mockEventService.Setup(service => service.CreateEventAsync(eventCreateDTO))
                .ReturnsAsync(createdEvent);

            var controller = new EventsController(mockEventService.Object, Mock.Of<IEventListService>());

            var result = await controller.CreateEvent(eventCreateDTO);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Event>(createdAtActionResult.Value);
            Assert.Equal(createdEvent.Id, returnValue.Id);
            Assert.Equal("New Event", returnValue.Title);
        }

        [Fact]
        public async Task DeleteEvent_ReturnsNoContent_WhenEventIsDeleted()
        {
            int eventIdToDelete = 1;
            var mockEventService = new Mock<IEventService>();

            mockEventService.Setup(service => service.DeleteEventAsync(eventIdToDelete))
                .ReturnsAsync(true);

            var controller = new EventsController(mockEventService.Object, Mock.Of<IEventListService>());

            var result = await controller.DeleteEvent(eventIdToDelete);

            var noContentResult = Assert.IsType<NoContentResult>(result);
        }
    
        [Fact]
        public async Task DeleteEvent_ReturnsNotFound_WhenEventDoesNotExist()
        {
            int eventIdToDelete = 999; // Un ID inexistant
            var mockEventService = new Mock<IEventService>();

            mockEventService.Setup(service => service.DeleteEventAsync(eventIdToDelete))
                .ReturnsAsync(false);

            var controller = new EventsController(mockEventService.Object, Mock.Of<IEventListService>());

            var result = await controller.DeleteEvent(eventIdToDelete);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
