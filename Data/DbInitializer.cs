using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covaciu_Carla_Proiect.Models;

namespace Covaciu_Carla_Proiect.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();
            if (context.Phones.Any())
            {
                return; // BD a fost creata anterior
            }
            var phones = new Phone[]
            {
                new Phone{Model="IphoneX",Company="Apple",Price=Decimal.Parse("3200")},
                new Phone{Model="GalaxyS21",Company="Samsung",Price=Decimal.Parse("4000")},
                new Phone{Model="Iphone12PRO",Company="Apple",Price=Decimal.Parse("5600")},
                new Phone{Model="GalaxySE",Company="Samsung",Price=Decimal.Parse("4500")},
                new Phone{Model="LenovoX",Company="Lenovo",Price=Decimal.Parse("2400")},
                new Phone{Model="SpaceNokia",Company="Nokia",Price=Decimal.Parse("2000")}
            };
            foreach (Phone b in phones)
            {
                context.Phones.Add(b);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {
                new Customer{CustomerID=1050,Name="Istrate Andreea",BirthDate=DateTime.Parse("1979-09-01")},
                new Customer{CustomerID=1045,Name="Ileana Bianca",BirthDate=DateTime.Parse("1969-07-08")},
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
                new Order{PhoneID=1,CustomerID=1050, OrderDate=DateTime.Parse("02-25-2021")},
                new Order{PhoneID=3,CustomerID=1045, OrderDate=DateTime.Parse("09-28-2021")},
                new Order{PhoneID=1,CustomerID=1045, OrderDate=DateTime.Parse("10-28-2021")},
                new Order{PhoneID=2,CustomerID=1050, OrderDate=DateTime.Parse("09-28-2021")},
                new Order{PhoneID=4,CustomerID=1050, OrderDate=DateTime.Parse("09-28-2021")},
                new Order{PhoneID=6,CustomerID=1050, OrderDate=DateTime.Parse("09-28-2021")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();
            var distributors = new Distributor[]
            {
                new Distributor{DistributorName="Altex",Adress="Str. Aviatorilor, nr. 40, Bucuresti"},
                new Distributor{DistributorName="MediaGalaxy",Adress="Str. Fagurelui, nr. 4, Bucuresti"},
                new Distributor{DistributorName="Emag",Adress="Str. Dorobantilor, nr. 32, Bucuresti"},
            };

            foreach (Distributor p in distributors)
            {
                context.Distributors.Add(p);
            }
            context.SaveChanges();

            var distributedPhones = new DistributedPhone[]
            {
                new DistributedPhone {PhoneID = phones.Single(c => c.Model == "IphoneX" ).ID, DistributorID = distributors.Single(i => i.DistributorName == "Emag").ID},
                new DistributedPhone {PhoneID = phones.Single(c => c.Model == "SpaceNokia" ).ID, DistributorID = distributors.Single(i => i.DistributorName == "MediaGalaxy").ID},
                new DistributedPhone {PhoneID = phones.Single(c => c.Model == "LenovoX" ).ID, DistributorID = distributors.Single(i => i.DistributorName == "Altex").ID},
                new DistributedPhone {PhoneID = phones.Single(c => c.Model == "GalaxySE" ).ID, DistributorID = distributors.Single(i => i.DistributorName == "Emag").ID},
                new DistributedPhone {PhoneID = phones.Single(c => c.Model == "Iphone12PRO" ).ID, DistributorID = distributors.Single(i => i.DistributorName == "MediaGalaxy").ID},
                new DistributedPhone {PhoneID = phones.Single(c => c.Model == "GalaxyS21" ).ID, DistributorID = distributors.Single(i => i.DistributorName == "Altex").ID},
            };

            foreach (DistributedPhone pb in distributedPhones)
            {
                context.DistributedPhones.Add(pb);
            }
            context.SaveChanges();
        }
    }
}
