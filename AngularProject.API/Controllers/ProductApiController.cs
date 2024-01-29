using AngularProject.API.Services;
using AngularProject.CORE.Result;
using AngularProject.DTO.Dtos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AngularProject.EF.Models;
using AngularProject.CORE.UnitOfWork;
using AngularProject.CORE.Helper;

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
        
       

        [Authorize]
        [Route("api/ProductApi/Add")]
        [HttpPost]
        public Result Post(ProductDto productDto)
        {

            return _productApiService.AddProduct(productDto);
        }

        [Authorize]
        [Route("api/ProductApi/Delete")]
        [HttpDelete]
        public Result Delete(int id)
        {

            return _productApiService.DeleteProduct(id);
        }

        [Authorize]
        [Route("api/ProductApi/Update")]
        [HttpPut]
        public Result Update(int id, ProductDto productDto)
        {

            return _productApiService.UpdateProduct(id, productDto);
        }

        [Authorize]
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


        [HttpPost]
        [Route("api/ProductApi/Update")]
        public async Task<HttpResponseMessage> Upload()
        {
            string uploadPath = HttpContext.Current.Server.MapPath("~/MainFile");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartFormDataStreamProvider(uploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);

            UpdateProductDTO model = new UpdateProductDTO();
            foreach (var key in provider.FormData.AllKeys)
            {
                var value = provider.FormData.Get(key);
                switch (key)
                {
                    case "productID":
                        model.productID = Convert.ToInt32(value);
                        break;
                    case "productName":
                        model.productName = value;
                        break;
                    case "productDescription":
                        model.productDescription = value;
                        break;
                    case "price":
                        model.price = Convert.ToDecimal(value);
                        break;
                    case "base64":
                        model.base64 = value;
                        break;
                    case "fileName":
                        model.fileName = value;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(model.base64) && !string.IsNullOrEmpty(model.fileName))
            {
                string filename = Guid.NewGuid().ToString() + model.fileName;
                string fullPath = Path.Combine(uploadPath, filename);

                byte[] imageBytes = Convert.FromBase64String(model.base64);
                File.WriteAllBytes(fullPath, imageBytes);

                model.fileName = filename;
            }
            else
            {
                foreach (var file in provider.FileData)
                {
                    // FileData'dan FileInfo nesnesini alıyoruz
                    var fileInfo = new FileInfo(file.LocalFileName);

                    // ContentDisposition'dan orijinal dosya adını alıyoruz
                    var originalFileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var extension = Path.GetExtension(originalFileName); // Burada dosyanın orijinal uzantısını alıyoruz

                    var guid = Guid.NewGuid().ToString();
                    var filename = guid + extension; // Yeni dosya adı GUID + orijinal uzantı

                    var fullPath = Path.Combine(uploadPath, filename);
                    File.Move(file.LocalFileName, fullPath); // Dosyayı yeni adıyla kaydediyoruz

                    model.fileName = filename;
                }
            }

            var _productDto = new ProductDto {
                productID = model.productID,
                productName = model.productName,
                productDescription = model.productDescription,
                price = model.price,
                imageURL = model.fileName
            };
            ShoppingProjectEntities shoppingProjectEntities= new ShoppingProjectEntities();
             EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);


            var mapped = _productDto.MapTo<ProductDto>();
            int productId = mapped.productID;
            var product = efUnitOfWork.ProductTemplate.GetById(productId).MapTo<ProductTBL>();

            product.productName = _productDto.productName;
            product.productDescription = _productDto.productDescription;
            product.price = _productDto.price;
            if (_productDto.imageURL != null)
            {
                product.imageURL = _productDto.imageURL;
            }

            efUnitOfWork.ProductTemplate.Update(product);
            efUnitOfWork.SaveChanges();


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public class UpdateProductDTO
        {
            public int productID { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public decimal price { get; set; }
            public string base64 { get; set; }
            public string fileName { get; set; }
        }


    }
}