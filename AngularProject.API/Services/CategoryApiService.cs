using AngularProject.CORE.Result;
using AngularProject.CORE.Helper;
using AngularProject.CORE.UnitOfWork;
using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using AutoMapper.QueryableExtensions;


namespace AngularProject.API.Services
{
    public class CategoryApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        //Veritabanındaki tüm kategorileri listeler.
        public string GetAllCategory()
        {

            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var categories = efUnitOfWork.CategoryTemplate.GetAll(i => i.isDelete == false).ProjectTo<CategoryDto>().ToList();

            return JsonConvert.SerializeObject(categories);
        }

        //Verilen id değerine sahip kategoriyi veritabanında bulur ve döndürür.
        public string GetCategoryById(int id)
        {
            var categories = efUnitOfWork.CategoryTemplate.GetById(id).MapTo<CategoryDto>();
            return JsonConvert.SerializeObject(categories);
        }


        //Verilen id değerine sahip kategori veritabanından siler.
        public Result DeleteCategory(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir katagori yok.");
            var category = efUnitOfWork.CategoryTemplate.GetById(id);
            if (category != null)
            {
                efUnitOfWork.CategoryTemplate.Delete(category);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Kategori başarıyla silindi.");
                return result;
            }

            return result;
        }

        //Yeni kategori ekler.
        public Result AddCategory(CategoryDto categoryDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (categoryDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedCategory = categoryDto.MapTo<CategoryTBL>();
                efUnitOfWork.CategoryTemplate.Add(mappedCategory);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Kayıt talebiniz başarıyla alındı.");

                return result;
            }
            return result;
        }

        //Verilen id değerine sahip kategoriyi günceller.
        public Result UpdateCategory(int id, CategoryDto categoryDto)
        {
            var result = Result.Instance.Warning("HATA! Güncelleme istediğiniz kategori bulunamadı.");

            // istenilen id mevcutsa güncellenecek data categorye atanır.
            var category = efUnitOfWork.CategoryTemplate.GetById(id).MapTo<CategoryDto>();
            if (category != null)
            {

                var mapped = categoryDto.MapTo<CategoryTBL>();
                mapped.categoryID = category.categoryID;
                efUnitOfWork.CategoryTemplate.Update(mapped);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Kategori başarıyla güncellendi.");
                return result;
            }
            return result;
        }

    }
}