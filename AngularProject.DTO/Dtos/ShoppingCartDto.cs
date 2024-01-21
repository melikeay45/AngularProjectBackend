using AngularProject.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class ShoppingCartDto
    {
        public int cartID { get; set; }
        public int userID { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

        public virtual ProductDto product { get; set; }
        public virtual UserDto user { get; set; }
    }
}
