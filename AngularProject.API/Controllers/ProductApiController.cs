using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AngularProject.API.Controllers
{
    public class ProductApiController:ApiController
    {
        private ProductApiService _productApiService = new ProductApiService();

        [Route("api/ProductApi/GetAll")]
        [HttpGet]
        public string Get()
        {
            return _productApiService.GetAllProduct();
        }

        [Route("api/ProductApi/Get")]
        [HttpGet]
        public string Get(int id)
        {
            return _productApiService.GetProductByID(id);
        }

        [Route("api/ProductApi/GetByCategoryID")]
        [HttpGet]
        public string GetbyCategoryID(int categoryID)
        {
            return _productApiService.GetProducsBycategoryID(categoryID);
        }

        //[Authorize]
        [Route("api/ProductApi/Add")]
        [HttpPost]
        public Result Post(ProductDto productDto)
        {

            return _productApiService.AddProduct(productDto);
        }

        //[Authorize]
        [Route("api/ProductApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return _productApiService.DeleteProduct(id);
        }

        //[Authorize]
        [Route("api/ProductApi/Update")]
        [HttpPut]
        public Result Update(int id, ProductDto productDto)
        {

            return _productApiService.UpdateProduct(id, productDto);
        }

        //C:\Users\Melike Aydın\Desktop\Angular-Bitirme-Projesi\Angular-Project\src\Uploads\ProductImages
        //[Authorize]
        [Route("api/ProductApi/UploadPicture")]
        [HttpPost]
        public Result UploadProductPicture(int id)
        {
            // isteği al.
            var httpRequest = HttpContext.Current.Request;

            // istek içine bak dosya var mı ?
            if (httpRequest.Files.Count > 0)
            {
                // Serverda dosyaları saklayacağım dizini belirt 
                string path = "C:/Users/Melike Aydın/Desktop/Angular-Bitirme-Projesi/Angular-Project/src/assets/Uploads/ProductImages";

                // eğer dizin yoksa oluştur
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // istek içinde bulunan dosyaları al
                foreach (string fileName in httpRequest.Files)
                {
                    //dosyayı değişkende tut
                    var postedFile = httpRequest.Files[fileName];

                    //dosyaya random isim hazırla.
                    string fName = Guid.NewGuid().ToString();

                    // dosyanın uzantısını al
                    string fExt = Path.GetExtension(postedFile.FileName);

                    // oluşturulan path içinde verdiğin isimle dosyayı yerleştir.Dosya yolunu değişkende tut
                    var filePath = Path.Combine(path, fName + fExt);

                    //dosyayı servera kaydet.
                    try
                    {
                        postedFile.SaveAs(filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Dosya kaydetme hatası: {ex.Message}");
                        return Result.Instance.Warning("HATA! Yüklemek istediğiniz fotoğraf kaydedilemedi.");
                    }

                    //ilgili kullanıcının id ' si ile dosyanın adını servise gönder dbye kaydetmesi için.
                    return _productApiService.UploadProductPicture(id, fName + fExt);
                }
            }
            return Result.Instance.Warning("HATA! Yüklemek istediğiniz fotoğraf yüklenemedi.");
        }

    }
}