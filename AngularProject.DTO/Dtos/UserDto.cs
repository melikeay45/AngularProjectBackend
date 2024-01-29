using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class UserDto
    {
        public int userID { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phoneNumber { get; set; }
        public DateTime registrationDate { get; set; }
        public bool userType { get; set; }
        public DateTime birthdate { get; set; }
        public bool isDelete { get; set; }



        //Foreign Key
        public virtual List<LogDto> logs { get; set; }
        public virtual List<ShoppingCartDto> shoppingCarts { get; set; }
        public virtual List<OrderDto> orders { get; set; }
    }
}
