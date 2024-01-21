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
    [Authorize]
    public class OrderApiController:ApiController
    {

        private OrderApiService orderApiService = new OrderApiService();

        [Route("api/OrderApi/GetAll")]
        [HttpGet]
        public string GetAll()
        {
            return orderApiService.GetAllOrder();
        }

        [Route("api/OrderApi/GetOrdersByUserID")]
        [HttpGet]
        public string GetOrdersByUserID(int id)
        {
            return orderApiService.GetOrdersByUserID(id);
        }


        [Route("api/OrderApi/Get")]
        [HttpGet]
        public string Get(int id)
        {
            return orderApiService.GetOrderByID(id);
        }


        //[Route("api/OrderApi/GetRecipeByRecipeName")]
        //[HttpGet]
        //public string GetRecipeByRecipeName(string recipeName)
        //{
        //    return recipeApiService.GetRecipeByRecipeName(recipeName);
        //}


        [Authorize]
        [Route("api/OrderApi/Add")]
        [HttpPost]
        public Result Post(OrderDto orderDto)
        {

            return orderApiService.AddOrder(orderDto);
        }

        [Authorize]
        [Route("api/OrderApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return orderApiService.DeleteOrder(id);
        }

        [Authorize]
        [Route("api/OrderApi/Update")]
        [HttpPut]
        public Result Update(int id, OrderDto orderDto)
        {

            return orderApiService.UpdateOrder(id, orderDto);
        }


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

        //[Authorize]
        //[Route("api/OrderApi/UploadRecipeVideo")]
        //[HttpPost]
        //public Result UploadRecipeVideo(int id)
        //{
        //    // isteği al.
        //    var httpRequest = HttpContext.Current.Request;

        //    // istek içine bak dosya var mı ?
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        // Serverda dosyaları saklayacağım dizini belirt
        //        string path = HttpContext.Current.Server.MapPath("~/Uploads/RecipeVideos");

        //        // eğer dizin yoksa oluştur
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        // istek içinde bulunan dosyaları al
        //        foreach (string file in httpRequest.Files)
        //        {
        //            //dosyayı değişkende tut
        //            var postedFile = httpRequest.Files[file];
        //            string decodedString = DecodeEncodedFileName(postedFile.FileName);

        //            //dosyaya random isim hazırla.
        //            string fNAme = Guid.NewGuid().ToString();

        //            // dosyanın uzantısını al
        //            string fExt = Path.GetExtension(decodedString);

        //            // oluşturulan path içinde verdiğin isimle dosyayı yerleştir.Dosya yolunu değişkende tut
        //            var filePath = Path.Combine(path, fNAme + fExt);

        //            //dosyayı servera kaydet.
        //            postedFile.SaveAs(filePath);

        //            //ilgili kullanıcının id ' si ile dosyanın adını servise gönder dbye kaydetmesi için.
        //            return recipeApiService.UploadRecipeVideo(id, fNAme + fExt);
        //        }
        //    }
        //    return Result.Instance.Warning("HATA! Yüklemek istediğiniz video yüklenemedi.");
        //}

        //[Authorize]
        //[Route("api/OrderApi/UploadRecipePicture")]
        //[HttpPost]
        //public Result UploadRecipePicture(int id)
        //{
        //    // isteği al.
        //    var httpRequest = HttpContext.Current.Request;

        //    // istek içine bak dosya var mı ?
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        // Serverda dosyaları saklayacağım dizini belirt
        //        string path = HttpContext.Current.Server.MapPath("~/Uploads/RecipePictures");

        //        // eğer dizin yoksa oluştur
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        // istek içinde bulunan dosyaları al
        //        foreach (string file in httpRequest.Files)
        //        {
        //            //dosyayı değişkende tut
        //            var postedFile = httpRequest.Files[file];

        //            //dosyaya random isim hazırla.
        //            string fNAme = Guid.NewGuid().ToString();

        //            // dosyanın uzantısını al
        //            string fExt = Path.GetExtension(postedFile.FileName);

        //            // oluşturulan path içinde verdiğin isimle dosyayı yerleştir.Dosya yolunu değişkende tut
        //            var filePath = Path.Combine(path, fNAme + fExt);

        //            //dosyayı servera kaydet.
        //            postedFile.SaveAs(filePath);

        //            //ilgili kullanıcının id ' si ile dosyanın adını servise gönder dbye kaydetmesi için.
        //            return recipeApiService.UploadRecipePicture(id, fNAme + fExt);

        //        }
        //    }
        //    return Result.Instance.Warning("HATA! Yüklemek istediğiniz fotoğraf yüklenemedi.");
        //}
    }
}