using Application.Common.Interfaces;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserByUsername
{
    public class GetUserByUsernameQuery : IRequest<UserModel>
    {
        public string Username { get; set; }
        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }

    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, UserModel>
    {
        private readonly IApplicationDbContext _context;
        private IMapper _mapper;

        public GetUserByUsernameQueryHandler(
            IApplicationDbContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserModel> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.SingleOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());
            return entity != null
                ? _mapper.Map<UserModel>(entity)
                : new UserModel();
        }
    }
}
