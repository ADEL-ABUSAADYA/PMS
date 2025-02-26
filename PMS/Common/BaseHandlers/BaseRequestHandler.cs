using MediatR;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Helpers;
using PMS.Models;

namespace PMS.Common.BaseHandlers
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        protected readonly IMediator _mediator;
        protected readonly TokenHelper _tokenHelper;
        protected readonly UserInfo _userInfo;
        protected readonly CancellationToken _cancellationToken;
        
        public BaseRequestHandler(BaseRequestHandlerParameters parameters)
        {
            _mediator = parameters.Mediator;
            _userInfo = parameters.UserInfo;
            _tokenHelper = parameters.TokenHelper;
            _cancellationToken = parameters.CancellationToken;
        }
        
        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}