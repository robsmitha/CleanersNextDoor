using AutoMapper;
using Domain.Models;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<UserModel>
    {
        public int UserID { get; set; }
        public GetUserQuery(int userId)
        {
            UserID = userId;
        }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserModel>
    {
        private readonly ICleanersNextDoorContext _context;
        private IMapper _mapper;

        public GetUserQueryHandler(
            ICleanersNextDoorContext context,
            IMapper mapper
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserModel> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FindAsync(request.UserID);
            return entity != null
                ? _mapper.Map<UserModel>(entity)
                : new UserModel();
        }
    }
}
