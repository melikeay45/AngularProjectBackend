using AngularProject.API.Models;
using AngularProject.API.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class LoginApiController : ApiController
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

        [HttpPost]
        [Route("api/LoginApi/login")]
        public IHttpActionResult Login(loginmodel model)
        {
            try
            {

                TokenApiService tokenApiService = new TokenApiService();

                var userDto = tokenApiService.CheckLogin(model.username, model.password);

                string userType = userDto.userType ? "user" : "admin" ;

                if (userDto == null)
                    return Ok(new { success = false, data = new returnLogin(), message = "Kullanıcı adı veya şifre hatalı." });

                var jwtKey = ConfigurationManager.AppSettings["JWT:Key"];
                var jwtIssuer = ConfigurationManager.AppSettings["JWT:Issuer"];
                var jwtAudience = ConfigurationManager.AppSettings["JWT:Audience"];
                var jwtManager = new JwtManager(jwtKey, jwtIssuer, jwtAudience);

                var token = jwtManager.GenerateToken(userDto.userID, userDto.userType);
                return Ok(new { success = true, data = new returnLogin { token = token, userType = userType }, message = "Başarılı." });
            }
            catch (Exception)
            {
                return Ok(new { success = false, data = new returnLogin(), message = "Giriş işlemi sırasında bilinmeyen bir hata ile karşılaşıldı." });
            }
        }

        public class loginmodel
        {
            [Required]
            public string username { get; set; }
            [Required]
            public string password{ get; set; }
        }

        public class returnLogin
        {
            public string token { get; set; }
            public string userType { get; set; }
        }

    }
}