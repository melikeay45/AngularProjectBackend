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
    public class UserApiService
    {
        // Önce Database Context dosyamın nesnesini oluşturuyorum.
        private static ShoppingProjectEntities shoppingProjectEntities = new ShoppingProjectEntities();

        // unitofwork nesnemi oluşturuyorum ve db context dosyamı parametre veriyorum.
        private EFUnitOfWork efUnitOfWork = new EFUnitOfWork(shoppingProjectEntities);

        public string GetAllUser()
        {
            //projecTo, autommaper aracı. DB varlığımı dto ya mapler.
            var users = efUnitOfWork.UserTemplate.GetAll()
            .OrderBy(o => o.userID) // gelen datayı id ye göre sırala
            .ProjectTo<UserDto>()
            .ToList();
            return JsonConvert.SerializeObject(users);
        }

        //Verilen id değerine sahip user verisini veritabanında bulur ve döndürür.
        public string GetUserById(int id)
        {
            var user = efUnitOfWork.UserTemplate.GetById(id).MapTo<UserDto>();

            if (user.isDelete == true)
            {
                return "HATA! Böyle bir kullanıcı yok.";

            }
            return JsonConvert.SerializeObject(user);

        }

        //Verilen id değerine sahip user verisini veritabanında pasif yapar.
        public Result DeleteUser(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir kullanıcı yok.");
            var user = efUnitOfWork.UserTemplate.GetById(id);
            if (user != null)
            {
                efUnitOfWork.UserTemplate.Delete(user);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Başarıyla silindi.");
                return result;
            }

            return result;
        }

        //Verilen id değerine sahip user verisini veritabanından aktif yapar.
        public Result ActiveUser(int id)
        {
            var result = Result.Instance.Warning("HATA! Böyle bir kullanıcı yok.");
            var user = efUnitOfWork.UserTemplate.GetById(id);
            if (user != null)
            {
                user.isDelete = false;
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Kullanıcı aktif edildi.");
                return result;
            }

            return result;
        }

        //Yeni kullanıcı ekler.
        public Result AddUser(UserDto userDto)
        {
            var result = Result.Instance.Warning("HATA! Girdiğiniz bilgileri kontrol ediniz.");

            if (userDto != null) //girilen bilgilerin kontrolü yapılır.
            {
                var mappedUser = userDto.MapTo<UserTBL>();
                mappedUser.registrationDate = DateTime.Now;
                efUnitOfWork.UserTemplate.Add(mappedUser);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Kullanıcı başarıyla eklendi.");

                return result;
            }
            return result;
        }

        //Verilen id değerine sahip kullanıcıyı günceller.
        public Result UpdateUser(int id, UserDto userDto)
        {
            var result = Result.Instance.Warning("HATA! Güncellemek istediğiniz kayıt bulunamadı.");

            // istenilen id mevcutsa güncellenecek data user a atanır.
            var user = efUnitOfWork.UserTemplate.GetById(id).MapTo<UserDto>();
            if (user != null)
            {
                var mapped = userDto.MapTo<UserTBL>();
                mapped.userID = user.userID;

                efUnitOfWork.UserTemplate.Update(mapped);
                efUnitOfWork.SaveChanges();

                result = Result.Instance.Success("Kullanıcı başarıyla güncellendi.");
                return result;
            }
            return result;
        }

        // şifremi unuttum
        public Result SendMailPassword(string mail)
        {
            var result = Result.Instance.Warning("HATA! Kayıtlı email adresi bulunamadı.");
            var user = efUnitOfWork.UserTemplate.Get(x => x.email == mail).MapTo<UserDto>();
            if (user != null)
            {
                Guid randomkey = Guid.NewGuid(); //32 karakterli kodu ürettik
                string Password = randomkey.ToString().Substring(0, 6); /// 6 haneli keyi password için aldık"
                MailApiService sender = new MailApiService();
                sender.sendMail(mail.ToString(), "Yeni Şifre", "Yeni Şifrenizle giriş yapabilirsiniz.\n Şifre : " + Password);
                user.password = Password;
                UpdateUser(user.userID, user);

                result = Result.Instance.Success("Yeni şifreniz email adresinize yollandı.");
                return result;
            }
            else
            {
                return result;
            }
        }

        // kullanıcı hesabını aktif edince mail atılacak.
        public Result SendMailActiveUser(string mail)
        {
            var result = Result.Instance.Warning("HATA! Kayıtlı email adresi bulunamadı.");
            var user = efUnitOfWork.UserTemplate.Get(x => x.email == mail).MapTo<UserDto>();
            if (user != null)
            {
                Guid randomkey = Guid.NewGuid(); //32 karakterli kodu ürettik
                string Password = randomkey.ToString().Substring(0, 6); /// 6 haneli keyi password için aldık"
                MailApiService sender = new MailApiService();
                sender.sendMail(mail.ToString(), "Hesabınız aktif edilmiştir.", "Hesabınıza giriş yapmanız için tek seferlik şifre belirledik. Şifrenizle giriş yapabilirsiniz. Giriş yaptıktan sonra lütfen şifrenizi değiştiriniz.\n Şifre : " + Password);
                user.password = Password;
                UpdateUser(user.userID, user);

                result = Result.Instance.Success("Yeni şifreniz email adresinize yollandı.");
                return result;
            }
            else
            {
                return result;
            }
        }


        //public Result UploadProfilePicture(int id, string filePath)
        //{
        //    var result = Result.Instance.Warning("HATA! Güncellemek istediğiniz kayıt bulunamadı.");

        //    try
        //    {
        //        // gelen id yi sorgula db de sorgula. Bu id ile kullanıcı kayıtlı ise onu UserDto entity ile maple.
        //        var user = efUnitOfWork.UserTemplate.GetById(id).MapTo<UserDto>();

        //        if (user != null)
        //        {
        //            //kullanıcının profil fotoğrafına gönderilen dosyanın yolunu gönder.
        //            user.profilePictureUrl = filePath;

        //            //kullanıcıyı veritabanı nesnesi ile maple.
        //            var mappedUser = user.MapTo<UserTBL>();
        //            // yapılan değişikliği güncelle
        //            efUnitOfWork.UserTemplate.Update(mappedUser);

        //            // değişiklikleri kaydet ve veritabanına yansıt
        //            efUnitOfWork.SaveChanges();

        //            result = Result.Instance.Success("Kullanıcı profili başarıyla güncellendi.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = Result.Instance.Fail("HATA! Bir hata oluştu: " + ex.Message);
        //    }

        //    return result;

        //}

    }
}