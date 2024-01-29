using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class UserApiController: ApiController
    {
        private UserApiService _userApiService = new UserApiService();

        [Authorize]
        [Route("api/UserApi/GetAll")]
        [HttpGet]
        public string GetAll()
        {
            return _userApiService.GetAllUser();
        }


        [Route("api/UserApi/Get")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            try
            {
                var user = _userApiService.GetUserById(Convert.ToInt32(User.Identity.Name));
                return Ok(new { success = true, data = user, message = "Başarılı." });
            }
            catch (Exception)
            {
                return Ok(new { success = false, data = "", message = "User getirilirken bir hata ile karşılaşıldı" });
            }

        }

        [Authorize]
        [Route("api/UserApi/ActiveUser")]
        [HttpGet]
        public Result ActiveUser(int id)
        {

            return _userApiService.ActiveUser(id);
        }

        [Route("api/UserApi/Add")]
        [HttpPost]
        public Result Post(UserDto newUser)
        {
            newUser.registrationDate = DateTime.Now;
            newUser.birthdate = DateTime.Now;
            return _userApiService.AddUser(newUser);
        }

        [Authorize]
        [Route("api/UserApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return _userApiService.DeleteUser(id);
        }

        [Authorize]
        [Route("api/UserApi/Update")]
        [HttpPut]
        public Result Update(int id, UserDto userDto)
        {
            
            return _userApiService.UpdateUser(id, userDto);
        }

        [Route("api/UserApi/SendMail")]
        [HttpPost]
        public Result SendMail(string mail)
        {
            return _userApiService.SendMailPassword(mail);
        }

        [Authorize]
        [Route("api/UserApi/SendMailActiveUser")]
        [HttpPost]
        public Result SendMailActiveUser(string mail)
        {
            return _userApiService.SendMailActiveUser(mail);
        }

       
}
}