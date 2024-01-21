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
    public class CategoryApiController:ApiController
    {
        private CategoryApiService _categoryApiService = new CategoryApiService();

        [Route("api/CategoryApi/GetAll")]
        [HttpGet]
        public string Get()
        {
            return _categoryApiService.GetAllCategory();
        }

        [Route("api/CategoryApi/Get")]
        [HttpGet]
        public string Get(int id)
        {
            return _categoryApiService.GetCategoryById(id);
        }

        //[Route("api/CategoryApi/GetByCategoryName")]
        //[HttpGet]
        //public string GetbyCategoryName(string categoryName)
        //{
        //    return _categoryApiService.GetCategoryByCategoryName(categoryName);
        //}

        //[Authorize]
        [Route("api/CategoryApi/Add")]
        [HttpPost]
        public Result Post(CategoryDto newCategory)
        {

            return _categoryApiService.AddCategory(newCategory);
        }

        //[Authorize]
        [Route("api/CategoryApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return _categoryApiService.DeleteCategory(id);
        }

        //[Authorize]
        [Route("api/CategoryApi/Update")]
        [HttpPut]
        public Result Update(int id, CategoryDto categoryDto)
        {

            return _categoryApiService.UpdateCategory(id, categoryDto);
        }
    }
}
