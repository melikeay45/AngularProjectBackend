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
using AngularProject.API.Models;
using Result = AngularProject.CORE.Result.Result;

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
        public async Task<HttpResponseMessage> Add()
        {
            string uploadPath = HttpContext.Current.Server.MapPath("~/MainFile");

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartFormDataStreamProvider(uploadPath);
            await Request.Content.ReadAsMultipartAsync(provider);

            AddProductDTO addProductDTO = new AddProductDTO();
            foreach (var key in provider.FormData.AllKeys)
            {
                var value = provider.FormData.Get(key);
                switch (key)
                {
                    case "productName":
                        addProductDTO.productName = value;
                        break;
                    case "productDescription":
                        addProductDTO.productDescription = value;
                        break;
                    case "price":
                        addProductDTO.price = Convert.ToDecimal(value);
                        break;
                    case "base64":
                        addProductDTO.base64 = value;
                        break;
                    case "fileName":
                        addProductDTO.fileName = value;
                        break;
                    case "categoryID":
                        addProductDTO.categoryID = Convert.ToInt32(value);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(addProductDTO.base64) && !string.IsNullOrEmpty(addProductDTO.fileName))
            {
                string filename = Guid.NewGuid().ToString() + addProductDTO.fileName;
                string fullPath = Path.Combine(uploadPath, filename);

                byte[] imageBytes = Convert.FromBase64String(addProductDTO.base64);
                File.WriteAllBytes(fullPath, imageBytes);

                addProductDTO.fileName = filename;
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

                    addProductDTO.fileName = filename;
                }
            }

            var productDto = new ProductDto
            {
                productName = addProductDTO.productName,
                productDescription = addProductDTO.productDescription,
                price = addProductDTO.price,
                imageURL = addProductDTO.fileName,
                stockQuantity=0,
                isDelete = false,
                categoryID=addProductDTO.categoryID,
            };
            ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();
            EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

            var mappedProduct = productDto.MapTo<ProductTBL>();
            
            efUnitOfWork.ProductTemplate.Add(mappedProduct);
            efUnitOfWork.SaveChanges();


            return Request.CreateResponse(HttpStatusCode.OK);
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
        
        public class AddProductDTO
        {
            public int categoryID { get; set; }
            public string productName { get; set; }
            public string productDescription { get; set; }
            public decimal price { get; set; }
            public string base64 { get; set; }
            public string fileName { get; set; }
        }


    }
}