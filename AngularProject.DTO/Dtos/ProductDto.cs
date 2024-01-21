using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class ProductDto
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public decimal price { get; set; }
        public int stockQuantity { get; set; }
        public int categoryID { get; set; }
        public string imageURL { get; set; }
        public bool isDelete { get; set; }

        public virtual CategoryDto category { get; set; }
        public virtual List<OrderDetailDto> orderDetails { get; set; }
        public virtual List<ShoppingCartDto> shoppingCarts { get; set; }
    }
}
