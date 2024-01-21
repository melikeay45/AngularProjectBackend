using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class CategoryDto
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string description { get; set; }
        public bool isDelete { get; set; }

        public virtual List<ProductDto> products { get; set; }
    }
}
