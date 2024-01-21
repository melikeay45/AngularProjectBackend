using AngularProject.CORE.Helper;
using AngularProject.CORE.Result;
using AngularProject.CORE.UnitOfWork;
using AngularProject.DTO.Dtos;
using AngularProject.EF.Models;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularProject.API.Services
{
    public class LogApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        public string GetAllLog()
        {
            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.

            var logs = efUnitOfWork.LogTemplate.GetAll()
            .OrderBy(o => o.logID) // gelen datayı id ye göre sırala
            .ProjectTo<LogDto>()
               .ToList();
            return JsonConvert.SerializeObject(logs);
        }

        //Verilen id değerine sahip logu veritabanında bulur ve döndürür.
        public string GetLogById(int id)
        {
            var logs = efUnitOfWork.LogTemplate.GetById(id).MapTo<LogDto>();
            return JsonConvert.SerializeObject(logs);
        }

        //Verilen id değerine sahip logu veritabanından siler.
        public Result DeleteLog(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir log yok.");
            var log = efUnitOfWork.LogTemplate.GetById(id);
            if (log != null)
            {
                efUnitOfWork.LogTemplate.Delete(log);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Log başarıyla silindi.");
                return result;
            }

            return result;
        }

        //Yeni log ekler.
        public Result AddLog(LogDto logDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (logDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedLog = logDto.MapTo<LogTBL>();
                efUnitOfWork.LogTemplate.Add(mappedLog);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Log kaydınız başarıyla yapıldı.");

                return result;
            }
            return result;
        }

        //Verilen id değerine sahip logu günceller.
        public Result UpdateLog(int id, LogDto logDto)
        {
            var result = Result.Instance.Warning("HATA! Güncelleme istediğiniz log bulunamadı.");

            // istenilen id mevcutsa güncellenecek data lgoa atanır.
            //GetById Tbl türünde veri getirip mapto yla dto ya çevirdi.
            var log = efUnitOfWork.LogTemplate.GetById(id).MapTo<LogDto>();
            if (log != null)
            {
                var mapped = logDto.MapTo<LogTBL>();
                mapped.logID = log.logID;
                efUnitOfWork.LogTemplate.Update(mapped);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Log başarıyla güncellendi.");
                return result;
            }
            return result;
        }
    }
}