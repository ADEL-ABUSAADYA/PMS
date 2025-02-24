using MediatR;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.Common.Users.Queries
{
    public record HasAccessQuery(Guid ID, Feature Feature) : IRequest<bool>;

    public class HasAccessQueryHandler : BaseRequestHandler<HasAccessQuery, bool>
    {
        private readonly IRepository<UserFeature> _repository;
        public HasAccessQueryHandler(BaseRequestHandlerParameters parameters, IRepository<UserFeature> repository) : base(parameters)
        {
            _repository = repository;
        }
        public override async Task<bool> Handle(HasAccessQuery request, CancellationToken cancellationToken)
        {
            var hasFeature = await _repository.AnyAsync(
                uf => uf.UserID == request.ID && uf.Feature == request.Feature);
            return hasFeature;
        }
    }
}