using AngularProject.CORE.Repositories;
using AngularProject.EF.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.CORE.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {

        private readonly ShoppingProjectEntities _dbContext;

        public EFUnitOfWork(ShoppingProjectEntities dbContext)
        {
            Database.SetInitializer<ShoppingProjectEntities>(null);

            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null.");

            _dbContext = dbContext;

            // Buradan istediğiniz gibi EntityFramework'ü konfigure edebilirsiniz.
            //_dbContext.Configuration.LazyLoadingEnabled = false;
            //_dbContext.Configuration.ValidateOnSaveEnabled = false;
            //_dbContext.Configuration.ProxyCreationEnabled = false;
        }

        //WORKING WITH TABLES
        private EFRepository<CategoryTBL> _categoryRepository;
        private EFRepository<UserTBL> _userRepository;
        private EFRepository<OrderTBL> _orderRepository;
        private EFRepository<ProductTBL> _productRepository;
        private EFRepository<ShoppingCartTBL> _shoppingCartRepository;
        private EFRepository<LogTBL> _logRepository;


        public EFRepository<CategoryTBL> CategoryTemplate => _categoryRepository ?? (_categoryRepository = new EFRepository<CategoryTBL>(_dbContext));


        public EFRepository<OrderTBL> OrderTemplate => _orderRepository ?? (_orderRepository = new EFRepository<OrderTBL>(_dbContext));

        public EFRepository<ProductTBL> ProductTemplate => _productRepository ?? (_productRepository = new EFRepository<ProductTBL>(_dbContext));

        public EFRepository<ShoppingCartTBL> ShoppingCartTemplate => _shoppingCartRepository ?? (_shoppingCartRepository = new EFRepository<ShoppingCartTBL>(_dbContext));


        public EFRepository<UserTBL> UserTemplate => _userRepository ?? (_userRepository = new EFRepository<UserTBL>(_dbContext));

        public EFRepository<LogTBL> LogTemplate => _logRepository ?? (_logRepository = new EFRepository<LogTBL>(_dbContext));

        #region IUnitOfWork Members
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new EFRepository<T>(_dbContext);
        }

        public int SaveChanges()
        {
            try
            {
                // Transaction işlemleri burada ele alınabilir veya Identity Map kurumsal tasarım kalıbı kullanılarak
                // sadece değişen alanları güncellemeyide sağlayabiliriz.
                return _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
        #endregion

        #region IDisposable Members
        // Burada IUnitOfWork arayüzüne implemente ettiğimiz IDisposable arayüzünün Dispose Patternini implemente ediyoruz.
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
