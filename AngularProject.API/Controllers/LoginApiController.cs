using AngularProject.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class LoginApiController:ApiController
    {
        private TokenApiService tokenApiService = new TokenApiService();

        //Giriş yapan kullanıcının token kimlik bilgilerini listeler.
        [Route("api/LoginApi/Get")]
        [HttpGet]
        public string Get()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return tokenApiService.TakeUserInfo(identity);
        }
    }
}