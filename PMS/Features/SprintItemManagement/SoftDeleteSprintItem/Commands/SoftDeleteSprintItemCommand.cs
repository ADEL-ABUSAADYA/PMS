// using MediatR;
// using PMS.Common.BaseHandlers;
// using PMS.Common.Views;
//
// namespace PMS.Features.SprintItemManagement.SoftDeleteSprintItem.Commands;
//
// public record SoftDeleteSprintItemCommand(int ProjectID) : IRequest<RequestResult<bool>>;
//
// public class SoftDeleteSprintItemCommandHandler : BaseRequestHandler<SoftDeleteSprintItemCommand, RequestResult<bool>>
// {
//     public SoftDeleteSprintItemCommandHandler(BaseRequestHandlerParameters parameters) : base(parameters)
//     {
//     }
//
//     public override Task<RequestResult<bool>> Handle(SoftDeleteSprintItemCommand request, CancellationToken cancellationToken)
//     {
//         
//     }
// }