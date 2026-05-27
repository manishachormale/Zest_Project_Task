using ZestTrainingFlow.Domain.Entities;
//using ZestTrainingFlow.Application.CQRS.Handlers;

namespace ZestTrainingFlow.Domain.Entities
{
    public class UserCQRS
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
