
using MediatR;
using ZestTrainingFlow.Application.CQRS.Queries;
//using ZestTrainingFlow.Application.CQRS.Handlers;
using ZestTrainingFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ZestTrainingFlow.Infrastructure.Data;

namespace ZestTrainingFlow.Application.CQRS.Handlers

{
    public class GetAllUserHandler:IRequestHandler<GetUsersByIdQuery,List<UserCQRS>>
    {
        private readonly MyDbContext _context;

        public GetAllUserHandler(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserCQRS>>Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.User.ToListAsync();
        }

    }
}




