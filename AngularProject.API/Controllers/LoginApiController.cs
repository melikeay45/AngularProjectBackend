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

                var _control = tokenApiService.CheckLogin(model.username, model.password);

                if (_control == null)
                    return Ok(new { success = false, data = "", message = "Kullanıcı adı veya şifre hatalı." });

                var jwtKey = ConfigurationManager.AppSettings["JWT:Key"];
                var jwtIssuer = ConfigurationManager.AppSettings["JWT:Issuer"];
                var jwtAudience = ConfigurationManager.AppSettings["JWT:Audience"];
                var jwtManager = new JwtManager(jwtKey, jwtIssuer, jwtAudience);

                var token = jwtManager.GenerateToken(_control.username, _control.userID);
                return Ok(new { success = true, data = token, message = "Başarılı." });
            }
            catch (Exception)
            {
                return Ok(new { success = false, data = "", message = "Giriş işlemi sırasında bilinmeyen bir hata ile karşılaşıldı." });
            }
        }

        public class loginmodel
        {
            [Required]
            public string username { get; set; }
            [Required]
            public string password{ get; set; }
        }

    }
}