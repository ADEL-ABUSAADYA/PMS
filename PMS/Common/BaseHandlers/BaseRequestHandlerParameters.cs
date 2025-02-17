using FluentValidation;
using MediatR;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Helpers;
using PMS.Models;

namespace PMS.Common.BaseHandlers
{
    public class BaseRequestHandlerParameters
    {
        private readonly IMediator _mediator;
        private readonly TokenHelper _tokenHelper;
        private readonly UserInfo _userInfo;

        public IMediator Mediator => _mediator;

        public TokenHelper TokenHelper => _tokenHelper;
        public UserInfo UserInfo => _userInfo;
        

        // Constructor accepts the generic repository type for flexibility
        public BaseRequestHandlerParameters(IMediator mediator, UserInfoProvider userInfoProvider, TokenHelper tokenHelper)
        {
            _mediator = mediator;
            _userInfo = userInfoProvider.UserInfo;
            _tokenHelper = tokenHelper;
            
        }
    }
}
