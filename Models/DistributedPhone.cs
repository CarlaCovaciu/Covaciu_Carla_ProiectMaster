using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covaciu_Carla_Proiect.Models
{
    public class DistributedPhone
    {
        public int DistributorID { get; set; }
        public int PhoneID { get; set; }
        public Distributor Distributor { get; set; }
        public Phone Phone { get; set; }
    }
}
