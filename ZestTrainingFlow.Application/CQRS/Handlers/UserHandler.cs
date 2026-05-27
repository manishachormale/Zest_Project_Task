using MediatR;

using ZestTrainingFlow.Application.CQRS.Commands;
using ZestTrainingFlow.Domain.Entities;
using ZestTrainingFlow.Infrastructure.Data;


namespace ZestTrainingFlow.Application.Handlers
{ 
    public class UserHandler : IRequestHandler<AddUserCommands, string>

    {

        private readonly MyDbContext _context;
        public UserHandler(MyDbContext context)
        {
            _context = context;
        }

        public async Task<string>Handle(AddUserCommands command, CancellationToken cancellationToken)
        {
            UserCQRS user = new UserCQRS
            {
                Name = command.Name,
                Email = command.Email
            };
            _context.User.Add(user);
            await  _context.SaveChangesAsync();
            return "User Added Successfully";
        }
    }
}
