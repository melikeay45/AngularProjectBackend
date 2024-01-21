using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    [Authorize]
    public class LogApiController : ApiController
    {
        private LogApiService logApiService = new LogApiService();

        [Route("api/LogApi/GetAll")]
        [HttpGet]
        public string Get()
        {
            return logApiService.GetAllLog();
        }

        [Route("api/LogApi/Get")]
        [HttpGet]
        public string Get(int id)
        {
            return logApiService.GetLogById(id);
        }

        [Route("api/LogApi/Add")]
        [HttpPost]
        public Result Post(LogDto newLog)
        {

            return logApiService.AddLog(newLog);
        }

        [Route("api/LogApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return logApiService.DeleteLog(id);
        }


        [Route("api/LogApi/Update")]
        [HttpPut]
        public Result Update(int id, LogDto logDto)
        {

            return logApiService.UpdateLog(id, logDto);
        }
    }
}