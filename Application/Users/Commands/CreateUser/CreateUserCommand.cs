using AutoMapper;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserModel>
    {
        public UserModel User;
        public CreateUserCommand(UserModel user)
        {
            User = user;
        }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public CreateUserCommandHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(request.User);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserModel>(entity);
        }
    }
}
