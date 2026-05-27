using MediatR;
//using ZestTrainingFlow.API.Controllers;
namespace ZestTrainingFlow.Application.CQRS.Commands
{
    public class AddUserCommands:IRequest<string>
    {
        //public class AddUserCommand:IRequest<string>//IRequest returns the string response
        
            public string Name {  get; set; }
            public string Email { get; set; }
        
    }
}
