using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUser;
using Application.Users.Queries.GetUserByUsername;
using Application.Common.Utilities;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CleanersNextDoor.Common;

namespace CleanersNextDoor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetByUsername/{username}")]
        public async Task<ActionResult<UserModel>> GetByUsername(string username)
        {
            return await _mediator.Send(new GetUserByUsernameQuery(username));
        }
        [HttpPost("SignIn")]
        public async Task<UserModel> SignIn(UserModel data)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(data.Username));
            if (!string.IsNullOrEmpty(user?.Password) && SecurePasswordHasher.Verify(data.Password, user.Password))
            {
                HttpContext.Session.Set(SessionHelper.CLAIM_ID, user.ID);
                return user;
            }
            return data;
        }
        [HttpPost("SignUp")]
        public async Task<UserModel> SignUp(UserModel data)
        {
            data.Password = SecurePasswordHasher.Hash(data.Password);
            var newUser = await _mediator.Send(new CreateUserCommand(data));
            HttpContext.Session.Set(SessionHelper.CLAIM_ID, newUser.ID);
            return newUser;
        }
        [HttpGet("{id}")]
        public async Task<UserModel> GetUser(int id)
        {
            var user = await _mediator.Send(new GetUserQuery(id));
            if (user != null)
            {
                return user;
            }
            return new UserModel();
        }
        [HttpGet("CheckUsernameAvailability/{username}")]
        public async Task<dynamic> CheckUsernameAvailability(string username)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(username));
            return new
            {
                isAvailable = user == null || user?.ID == 0
            };
        }
    }
}