using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator
        : AbstractValidator<CreateUserCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateUserCommandValidator(IApplicationDbContext context)
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
