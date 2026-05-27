using MediatR;
using ZestTrainingFlow.Application.CQRS.Queries;
using ZestTrainingFlow.Domain.Entities;
using ZestTrainingFlow.Application.CQRS.Handlers;
//using ZestTrainingFlow.Application.CQRS.Queries;



namespace ZestTrainingFlow.Application.CQRS.Queries
{
    
    public class GetUsersByIdQuery : IRequest<List<UserCQRS>>
    {

    }

}
