using FluentValidation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator
        : AbstractValidator<CreateUserCommand>
    {
        private readonly ICleanersNextDoorContext _context;
        public CreateUserCommandValidator(ICleanersNextDoorContext context)
        {
            _context = context;
            RuleFor(v => v.User.Username)
                .MaximumLength(50)
                .NotEmpty()
                .MustAsync(BeUniqueUsername)
                    .WithMessage("The specified username already exists.");
        }
        public async Task<bool> BeUniqueUsername(string username,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .AllAsync(l => l.Username.ToLower() != username.ToLower());
        }
    }
}
