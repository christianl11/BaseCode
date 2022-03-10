using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using BaseCode.Models.Requests;
using BaseCode.Models.Responses;
using BaseCode.Models;

namespace BaseCode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseCodeController : Controller
    {
        private DBContext db;
        private readonly IWebHostEnvironment hostingEnvironment;
        private IHttpContextAccessor _IPAccess;

        private static readonly string[] Summaries = new[]
       {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public BaseCodeController(DBContext context, IWebHostEnvironment environment, IHttpContextAccessor accessor)
        {
            _IPAccess = accessor;
            db = context;
            hostingEnvironment = environment;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] CreateUserRequest r)
        {
            CreateUserResponse resp = new CreateUserResponse();

            if (string.IsNullOrEmpty(r.FirstName))
            {
                resp.Message = "Please specify Firstname.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.LastName))
            {
                resp.Message = "Please specify lastname.";
                return BadRequest(resp);
            }


            resp = db.CreateUserUsingSqlScript(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }

        [HttpPut("UpdateUser")]
        public IActionResult UpdateUser([FromBody] CreateUserRequest r)
        {
            CreateUserResponse resp = new CreateUserResponse();

            if (string.IsNullOrEmpty(r.UserId.ToString()))
            {
                resp.Message = "Please specify UserId.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.FirstName.ToString()))
            {
                resp.Message = "Please specify FirstName.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.UserName.ToString()))
            {
                resp.Message = "Please specify UserName.";
                return BadRequest(resp);
            }

            resp = db.UpdateUser(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpGet("GetUserList")]
        public IActionResult GetUserList()
        {
            GetUserListResponse resp = new GetUserListResponse();

            resp = db.GetUserList();

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest r)
        {
            GetUserListResponse resp = new GetUserListResponse();

            if (string.IsNullOrEmpty(r.UserName))
            {
                resp.Message = "Please specify Username.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Password))
            {
                resp.Message = "Please specify Password.";
                return BadRequest(resp);
            }

            resp = db.Login(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpPost("CreateUserType")]
        public IActionResult CreateUserType([FromBody] CreateUserTypeRequest r)
        {
            CreateUserTypeResponse resp = new CreateUserTypeResponse();

            if (string.IsNullOrEmpty(r.UserTypeDesc))
            {
                resp.Message = "Please specify Description.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Status))
            {
                resp.Message = "Please specify Status.";
                return BadRequest(resp);
            }

            resp = db.CreateUserTypes(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpPut("UpdateUserType/{UserTypeID}")]
        
        public IActionResult UpdateUserT(int userTypeId,[FromBody] CreateUserTypeRequest r)
        {
            CreateUserTypeResponse resp = new CreateUserTypeResponse();

         

            if (string.IsNullOrEmpty(r.UserTypeId.ToString()))  
            {
                resp.Message = "Please specify User Type Id.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.UserTypeDesc.ToString()))
            {
                resp.Message = "Please specify User Type Description.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Status.ToString()))
            {
                resp.Message = "Please specify Status.";
                return BadRequest(resp);
            }
            r.UserTypeId = userTypeId;  
            resp = db.UpdateUserType(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] CreateUserTypeRequest r)
        {
            CreateUserTypeResponse resp = new CreateUserTypeResponse();

            if (string.IsNullOrEmpty(r.UserTypeId.ToString()))
            {
                resp.Message = "Please specify UserTypeID.";
                return BadRequest(resp);
            }
            resp = db.Delete(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpGet("GetUserType")]
        public IActionResult GetUserTList()
        {
            GetUserTypeListResponse resp = new GetUserTypeListResponse();

            resp = db.GetUserTypeList();

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpPost("CreateModule")]
        public IActionResult CreateModule([FromBody] CreateModuleRequest r)
        {
            CreateModuleResponse resp = new CreateModuleResponse();

            if (string.IsNullOrEmpty(r.ModuleID.ToString()))
            {
                resp.Message = "Please specify MODULE ID.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Description.ToString()))
            {
                resp.Message = "Please specify Description.";
                return BadRequest(resp);
            }
           

            resp = db.CreateModule(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpPut("UpdateModule")]
        public IActionResult UpdateModule([FromBody] CreateModuleRequest r)
        {
            CreateModuleResponse resp = new CreateModuleResponse();

            if (string.IsNullOrEmpty(r.ModuleID.ToString()))
            {
                resp.Message = "Please specify Module Id.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Description.ToString()))
            {
                resp.Message = "Please specify User Type Description.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Status.ToString()))
            {
                resp.Message = "Please specify Status.";
                return BadRequest(resp);
            }
            resp = db.UpdateModule(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpDelete("DeleteModule")]
        public IActionResult DeleteModule([FromBody] CreateModuleRequest r)
        {
            CreateModuleResponse resp = new CreateModuleResponse();

            if (string.IsNullOrEmpty(r.ModuleID.ToString()))
            {
                resp.Message = "Please specify UserTypeID.";
                return BadRequest(resp);
            }
            resp = db.DeleteModule(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpGet("GetModuleList")]
        public IActionResult GetModuleList()
        {
            GetModuleListResponse resp = new GetModuleListResponse();

            resp = db.GetModuleList();

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }

        [HttpPost("CreateModuleType")]
        public IActionResult CreateModuleType([FromBody] CreateModuleTypeAccessRequest r)
        {
            CreateModuleTypeResponse resp = new CreateModuleTypeResponse();

            if (string.IsNullOrEmpty(r.ModuleID.ToString()))
            {
                resp.Message = "Please specify MODULE ID.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.UserTypeID.ToString()))
            {
                resp.Message = "Please specify User Type ID.";
                return BadRequest(resp);
            }


            resp = db.CreateModuleType(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpPut("UpdateModuleType")]
        public IActionResult UpdateModuleType([FromBody] CreateModuleTypeAccessRequest r)
        {
            CreateModuleTypeResponse resp = new CreateModuleTypeResponse();

            if (string.IsNullOrEmpty(r.UtmaID.ToString()))
            {
                resp.Message = "Please specify Utma Id.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.UserTypeID.ToString()))
            {
                resp.Message = "Please specify User Type User Type ID.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.ModuleID.ToString()))
            {
                resp.Message = "Please specify ModuleID.";
                return BadRequest(resp);
            }
            if (string.IsNullOrEmpty(r.Status.ToString()))
            {
                resp.Message = "Please specify Status.";
                return BadRequest(resp);
            }
            resp = db.UpdateModuleType(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpDelete("DeleteModuleType")]
        public IActionResult DeleteModuleType([FromBody] CreateModuleTypeAccessRequest r)
        {
            CreateModuleTypeResponse resp = new CreateModuleTypeResponse();

            if (string.IsNullOrEmpty(r.UtmaID.ToString()))
            {
                resp.Message = "Please specify UtmaID.";
                return BadRequest(resp);
            }
            resp = db.DeleteModuleType(r);

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpGet("GetModuleListType")]
        public IActionResult GetModuleListType()
        {
            GetModuleTypeListResponse resp = new GetModuleTypeListResponse();

            resp = db.GetModuleListType();

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
        [HttpGet("GetUserAccessModule")]
        public IActionResult GetuserAccess()
        {
            GetUserAccessResponse resp = new GetUserAccessResponse();

            resp = db.GetUserAccess();

            if (resp.isSuccess)
                return Ok(resp);
            else
                return BadRequest(resp);
        }
    }
}

        