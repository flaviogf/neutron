using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Neutron.Application;
using Neutron.Web.ViewModels;

namespace Neutron.Web.Controllers
{
    [Route("[controller]")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EventController(IMediator mediator, IMapper mapper)
        {
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
        public async Task<IActionResult> Create(CreateEventViewModel viewModel)
        {
            CreateEventInput input = _mapper.Map<CreateEventInput>(viewModel);

            Result<CreateEventOutput> result = await _mediator.Send(input);

            if (result.IsFailure)
            {
                TempData["Failure"] = result.Error;

                return RedirectToAction("Create", "Event");
            }

            TempData["Success"] = "Event has been created";

            return RedirectToAction("Create", "Event");
        }
    }
}
