using MediatR;
using Microsoft.EntityFrameworkCore;
using PMS.Common.BaseHandlers;
using PMS.Common.Data.Enums;
using PMS.Common.Views;
using PMS.Data.Repositories;
using PMS.Models;

namespace PMS.Features.UserManagement.ChangeStatus.Command
{
    public record ChangeStatusCommand(Guid id) : IRequest<RequestResult<bool>>;

    public class BlockUserCommandHandler : BaseRequestHandler<ChangeStatusCommand, RequestResult<bool>>
    {
        private readonly IRepository<User> _repository;
        public BlockUserCommandHandler(BaseRequestHandlerParameters parameters, IRepository<User> repository) : base(parameters)
        {
            _repository = repository;
        }

        public override async Task<RequestResult<bool>> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var checkActivtion = await  _repository 
               .Get(c => c.ID == request.id)
               .Select(c=> new  { ID = c.ID ,  IsActive =c.IsActive })
               .FirstOrDefaultAsync();

            if (checkActivtion == null) return RequestResult<bool>.Failure(ErrorCode.NoUsersFound, "this user not found");

         

            var changeStatus = !checkActivtion.IsActive;   
                 

            var user = new User { ID = checkActivtion.ID  , IsActive = changeStatus };


          await  _repository.SaveIncludeAsync(user , nameof(user.IsActive));

          await  _repository.SaveChangesAsync();

         return RequestResult<bool>.Success(true);  
        }
    }



}
