using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neutron.Application;
using Neutron.Core;
using Neutron.Infrastructure;
using Neutron.Web.ViewModels;

namespace Neutron.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventController(IEventRepository eventRepository, IUnitOfWork uow, IMediator mediator, IMapper mapper)
        {
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

            Result result = await _mediator.Send(createEvent);

            if (result.IsFailure)
            {
                TempData["Failure"] = result.Error;

                return RedirectToAction("Create", "Event");
            }

            TempData["Success"] = "The countdown for the event has been started";

            _uow.Commit();

            return RedirectToAction("Index", "Event");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Show([FromRoute] Guid id)
        {
            Maybe<Event> maybeEvent = await _eventRepository.FindById(id);

            if (maybeEvent.HasNoValue)
            {
                TempData["Failure"] = "Event does not exist";

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

            Result result = await _mediator.Send(destroyEvent);

            if (result.IsFailure)
            {
                TempData["Failure"] = result.Error;

                return RedirectToAction("Index", "Event");
            }

            TempData["Success"] = "The countdown for the event has been stopped";

            _uow.Commit();

            return RedirectToAction("Index", "Event");
        }
    }
}
