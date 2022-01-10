using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Covaciu_Carla_Proiect.Models
{
    public class Distributor
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Distributor Name")]
        [StringLength(50)]
        public string DistributorName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }

        public ICollection<DistributedPhone> DistributedPhones { get; set; }
    }
}
