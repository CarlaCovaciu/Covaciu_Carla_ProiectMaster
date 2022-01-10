using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covaciu_Carla_Proiect.Models.StoreViewModel
{
    public class DistributorIndexData
    {
        public IEnumerable<Distributor> Distributors { get; set; }
        public IEnumerable<Phone> Phones { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
