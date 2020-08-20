using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Neutron.Application;
using Neutron.Core;
using Neutron.Infrastructure;
using Neutron.Web.Hubs;
using Neutron.Web.ViewModels;

namespace Neutron.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<EventHub, IEventClient> _eventHub;
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventController(UserManager<ApplicationUser> userManager, IHubContext<EventHub, IEventClient> eventHub, IEventRepository eventRepository, IUnitOfWork uow, IMediator mediator, IMapper mapper)
        {
            _userManager = userManager;
            _eventHub = eventHub;
            _eventRepository = eventRepository;
            _uow = uow;
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Event> events = _eventRepository.FindAll();

            return View(events);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateEventViewModel viewModel)
        {
            CreateEvent createEvent = _mapper.Map<CreateEvent>(viewModel);

            Result<Event> result = await _mediator.Send(createEvent);

            if (result.IsFailure)
            {
                TempData["Failure"] = result.Error;

                return RedirectToAction("Create", "Event");
            }

            TempData["Success"] = "The countdown for the event has been started";

            _uow.Commit();

            string userId = _userManager.GetUserId(User);

            await _eventHub.Clients.User(userId).Start(result.Value);

            return RedirectToAction("Index", "Event");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Show([FromRoute] Guid id)
        {
            Maybe<Event> maybeEvent = await _eventRepository.FindById(id);

            if (maybeEvent.HasNoValue)
            {
                return RedirectToAction("Index", "Event");
            }

            Event @event = maybeEvent.Value;

            return View(@event);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Destroy(Guid id)
        {
            var destroyEvent = new DestroyEvent(id);

            Result<Event> result = await _mediator.Send(destroyEvent);

            if (result.IsFailure)
            {
                TempData["Failure"] = result.Error;

                return RedirectToAction("Index", "Event");
            }

            TempData["Success"] = "The countdown for the event has been stopped";

            _uow.Commit();

            string userId = _userManager.GetUserId(User);

            await _eventHub.Clients.User(userId).Stop(result.Value);

            return RedirectToAction("Index", "Event");
        }
    }
}
