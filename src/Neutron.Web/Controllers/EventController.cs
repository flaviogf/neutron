using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Neutron.Core;
using Neutron.Infrastructure;
using Neutron.Web.ViewModels;

namespace Neutron.Web.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventController(IUnitOfWork uow, IMediator mediator, IMapper mapper)
        {
            _uow = uow;
            _mediator = mediator;
            _mapper = mapper;
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

            TempData["Success"] = "Event has been created";

            _uow.Commit();

            return RedirectToAction("Create", "Event");
        }
    }
}
