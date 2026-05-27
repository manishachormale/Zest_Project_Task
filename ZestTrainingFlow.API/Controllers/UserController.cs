using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZestTrainingFlow.Application.CQRS.Commands;
using ZestTrainingFlow.Application.CQRS.Queries;

namespace ZestTrainingFlow.API.Controllers//tis controller is for CQRS
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ADD USER
        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserCommands command)
        {
            var result =
                await _mediator.Send(command);

            return Ok(result);
        }

        
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult>GetAllUsers()
        {
            var users = await _mediator.Send(new GetUsersByIdQuery());

            return Ok(users);
        }
    }
}
