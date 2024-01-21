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
        public decimal totalAmount { get; set; }
        public DateTime orderDate { get; set; }
        public string orderStatus { get; set; }

        public virtual List<OrderDetailDto> orderDetails { get; set; }
        public virtual UserDto user { get; set; }
    }
}
