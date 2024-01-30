using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class OrderDto
    {
        public int orderID { get; set; }
        public int userID { get; set; }
        public double totalAmount { get; set; }
        public DateTime orderDate { get; set; }
        public string orderStatus { get; set; }
        public bool isDelete { get; set; }
        public string address { get; set; }
        public int phoneNumber { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public double unitPrice { get; set; }


        public virtual UserDto user { get; set; }
    }
}
