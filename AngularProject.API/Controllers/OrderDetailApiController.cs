using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class OrderDetailApiController:ApiController
    {
       
            private OrderDetailApiService orderDetailApiService = new OrderDetailApiService();

            [Route("api/OrderDetailApi/GetAll")]
            [HttpGet]
            public string GetAll()
            {
                return orderDetailApiService.GetAllOrderDetail();
            }

            [Route("api/OrderDetailApi/GetOrderDetailByOrderID")]
            [HttpGet]
            public string GetOrderDetailByOrderID(int id)
            {
                return orderDetailApiService.GetOrderDetailByOrderID(id);
            }


            //[Route("api/RecipeApi/GetRecipeByUserId")]
            //[HttpGet]
            //public string GetRecipeByUserId(int id, int page, int pageSize)
            //{
            //    return recipeApiService.GetRecipeByUserId(id, page, pageSize);
            //}

            [Route("api/OrderDetailApi/Get")]
            [HttpGet]
            public string Get(int id)
            {
                return orderDetailApiService.GetOrderDetailByID(id);
            }

            //[Route("api/RecipeApi/GetRecipeByRecipeName")]
            //[HttpGet]
            //public string GetRecipeByRecipeName(string recipeName)
            //{
            //    return recipeApiService.GetRecipeByRecipeName(recipeName);
            //}

            [Authorize]
            [Route("api/OrderDetailApi/Add")]
            [HttpPost]
            public Result Post(OrderDetailDto orderDetailDto)
            {

                return orderDetailApiService.AddOrderDetail(orderDetailDto);
            }

            [Authorize]
            [Route("api/OrderDetailApi/Delete")]
            [HttpDelete]
            public Result Delete(int id)
            {

                return orderDetailApiService.DeleteOrderDetail(id);
            }

            //[Authorize]
            //[Route("api/RecipeApi/Update")]
            //[HttpPut]
            //public Result Update(int id, RecipeDto recipeDto)
            //{

            //    return recipeApiService.UpdateRecipe(id, recipeDto);
            //}


            public static string DecodeEncodedFileName(string encodedFileName)
            {
                var regex = new Regex(@"=\?utf-8\?B\?(.*?)\?=");
                var match = regex.Match(encodedFileName);

                if (match.Success)
                {
                    var base64String = match.Groups[1].Value;
                    var bytes = Convert.FromBase64String(base64String);
                    return System.Text.Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    return encodedFileName;
                }
            }
    }
}
