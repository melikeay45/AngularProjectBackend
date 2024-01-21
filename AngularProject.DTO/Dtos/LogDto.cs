using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularProject.DTO.Dtos
{
    public class LogDto
    {
        public int logID { get; set; }
        public int userID { get; set; }
        public string logActivity { get; set; }
        public DateTime logDate { get; set; }
        public string ipAddress { get; set; }
        public string logUsername { get; set; }

        public virtual UserDto user { get; set; }
    }
}
