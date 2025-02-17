﻿using FluentValidation;
using PMS.Features.UserManagement.GetAllUsers;

namespace PMS.Features.UserManagement.ChangeStatus
{
    public record RequestUserActivtaionStatus(int id);

    public class RequestUserActivtaionStatusValidator : AbstractValidator<RequestUserActivtaionStatus>
    {
        public RequestUserActivtaionStatusValidator()
        {
          
        }
    }


}