using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class OrderDetailDto
    {
        public int detailID { get; set; }
        public int orderID { get; set; }
        public int productID { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }

        public virtual OrderDto order { get; set; }
        public virtual ProductDto product { get; set; }
    }
}
