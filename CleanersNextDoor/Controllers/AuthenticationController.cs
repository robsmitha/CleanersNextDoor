using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanersNextDoor.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanersNextDoor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpGet("GetClaimID")]
        public ActionResult<int> GetClaimID()
        {
            var claimId = HttpContext.Session.Get<int>(SessionHelper.CLAIM_ID);
            return Ok(claimId);
        }
        [HttpGet("ClearSession")]
        public ActionResult<bool> ClearSession()
        {
            HttpContext.Session.Clear();
            return Ok(true);
        }
    }
}