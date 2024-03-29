﻿using ProjektSklep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjektSklep.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjektSklep.Data
{
    public static class DbInitializer
    {
        public static void AddCustomers(UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedUsers(UserManager<Customer> userManager)
        {
            if (userManager.FindByNameAsync("siemanokolano2137@gmail.com").Result == null)
            {
                Customer customer = new Customer();
                customer.UserName = "siemanokolano2137@gmail.com";
                customer.Email = "siemanokolano2137@gmail.com";
                customer.EmailConfirmed = true;
                customer.PhoneNumberConfirmed = false;
                customer.TwoFactorEnabled = false;
                customer.LockoutEnabled = false;
                customer.AccessFailedCount = 0;
                customer.AddressID = 1;
                customer.PageConfigurationID = 1;
                customer.FirstName = "Bartłomiej";
                customer.LastName = "Umiński";

                Address address = null;
                PageConfiguration pageConfiguration = null;
                using (var context = new ShopContext())
                {
                    address = new Address { CustomerID = customer.Id, Country = "Polska", Town = "Białystok", PostCode = "12-123", Street = "Wesoła", HouseNumber = 123, ApartmentNumber = 1 };
                    pageConfiguration = new PageConfiguration { CustomerID = customer.Id, SendingNewsletter = false, ShowNetPrices = true, ProductsPerPage = 4, InterfaceSkin = 0, Language = 0, Currency = 1 };
                    context.Addresses.Add(address);
                    context.PageConfigurations.Add(pageConfiguration);
                    context.SaveChanges();
                }

                customer.AddressID = address.AddressID;
                customer.PageConfigurationID = pageConfiguration.PageConfigurationID;

                IdentityResult result = userManager.CreateAsync(customer, "Admin123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(customer, "Administrator").Wait();
                }
            }

            if (userManager.FindByNameAsync("klientklientowski123@gmail.com").Result == null)
            {
                Customer customer = new Customer();
                customer.UserName = "klientklientowski123@gmail.com";
                customer.Email = "klientklientowski123@gmail.com";
                customer.EmailConfirmed = true;
                customer.PhoneNumberConfirmed = false;
                customer.TwoFactorEnabled = false;
                customer.LockoutEnabled = false;
                customer.AccessFailedCount = 0;
                customer.AddressID = 2;
                customer.PageConfigurationID = 2;
                customer.FirstName = "Klient";
                customer.LastName = "Klientowski";

                Address address = null;
                PageConfiguration pageConfiguration = null;
                using (var context = new ShopContext())
                {
                    address = new Address { CustomerID = customer.Id, Country = "Polska", Town = "Warszawa", PostCode = "23-456", Street = "Piękna", HouseNumber = 12, ApartmentNumber = 47 };
                    pageConfiguration = new PageConfiguration { CustomerID = customer.Id, SendingNewsletter = true, ShowNetPrices = false, ProductsPerPage = 3, InterfaceSkin = 1, Language = 1, Currency = 0 };
                    context.Addresses.Add(address);
                    context.PageConfigurations.Add(pageConfiguration);
                    context.SaveChanges();
                }

                customer.AddressID = address.AddressID;
                customer.PageConfigurationID = pageConfiguration.PageConfigurationID;

                IdentityResult result = userManager.CreateAsync(customer, "Klient123!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(customer, "NormalUser").Wait();
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "NormalUser";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        public static void Initialize(ShopContext context, UserManager<Customer> userManager)
        {
            context.Database.EnsureCreated();

            /* DiscountCodes */
            if (context.DiscountCodes.Any())
            {
                return;
            }
            var discountCodes = new DiscountCode[]
            {
                new DiscountCode{ DiscoundCode="123ABC", Percent=50 },
                new DiscountCode{ DiscoundCode="234BCD", Percent=30 },
                new DiscountCode{ DiscoundCode="123ABC", Percent=10 },
                new DiscountCode{ DiscoundCode="123XYZ", Percent=20 },
                new DiscountCode{ DiscoundCode="789KLM", Percent=70 }
            };
            foreach (DiscountCode discountCode in discountCodes)
            {
                context.DiscountCodes.Add(discountCode);
            }
            context.SaveChanges();

            /* Addresses */
            /*if (context.Addresses.Any())
            {
                return;
            }
            var addresses = new Address[]
            {
                new Address{ CustomerID=1, Country="Polska", Town="Białystok", PostCode="12-123", Street="Wesoła", HouseNumber=123, ApartmentNumber=1 },
                new Address{ CustomerID=2, Country="Polska", Town="Warszawa", PostCode="23-456", Street="Piękna", HouseNumber=12, ApartmentNumber=47 },
                new Address{ CustomerID=3, Country="Polska", Town="Gdańsk", PostCode="56-678", Street="Miła", HouseNumber=56, ApartmentNumber=23 },
                new Address{ CustomerID=4, Country="Polska", Town="Kraków", PostCode="78-234", Street="Diamentowa", HouseNumber=26, },
                new Address{ CustomerID=5, Country="Polska", Town="Łomża", PostCode="34-785", Street="Wiejska", HouseNumber=6, ApartmentNumber=67 }
            };
            foreach (Address address in addresses)
            {
                context.Addresses.Add(address);
            }
            context.SaveChanges();

           /* PageConfigurations */
            /*if (context.PageConfigurations.Any())
            {
                return;
            }
            var pageConfigurations = new PageConfiguration[]
            {
                new PageConfiguration{ CustomerID=1, SendingNewsletter=false, ShowNetPrices=true, ProductsPerPage=20, InterfaceSkin=0, Language=0, Currency=1 },
                new PageConfiguration{ CustomerID=2, SendingNewsletter=true, ShowNetPrices=false, ProductsPerPage=30, InterfaceSkin=1, Language=1, Currency=0 },
                new PageConfiguration{ CustomerID=3, SendingNewsletter=false, ShowNetPrices=false, ProductsPerPage=15, InterfaceSkin=0, Language=0, Currency=3 },
                new PageConfiguration{ CustomerID=4, SendingNewsletter=true, ShowNetPrices=false, ProductsPerPage=10, InterfaceSkin=1, Language=0, Currency=2 },
                new PageConfiguration{ CustomerID=5, SendingNewsletter=true, ShowNetPrices=true, ProductsPerPage=50, InterfaceSkin=0, Language=1, Currency=0 }
            };
            foreach (PageConfiguration pageConfiguration in pageConfigurations)
            {
                context.PageConfigurations.Add(pageConfiguration);
            }
            context.SaveChanges();*/

            /* Customers */
            /*if (context.Customers.Any())
            {
                return;
            }
            var customers = new Customer[]
            {
                new Customer{ AddressID=1, PageConfigurationID=1, FirstName="Bartłomiej", LastName="Umiński", Email="siemanokolano2137@gmail.com"},
                new Customer{ AddressID=2, PageConfigurationID=2, FirstName="Kacper", LastName="Siegieńczuk", Email="kacpersiegienczuk@gmail.com"},
                new Customer{ AddressID=3, PageConfigurationID=3, FirstName="Michał", LastName="Kozikowski", Email="michalkozikowski@gmail.com"},
                new Customer{ AddressID=4, PageConfigurationID=4, FirstName="Jakub", LastName="Kozłowski", Email="jakubkozlowski@gmail.com"},
                new Customer{ AddressID=5, PageConfigurationID=5, FirstName="Klient", LastName="Klientowski", Email="klientklientowski@gmail.com"}
            };
            foreach (Customer customer in customers)
            {
                context.Customers.Add(customer);
            }
            context.SaveChanges();*/

            /* ShippingMethods */
            if (context.ShippingMethods.Any())
            {
                return;
            }
            var shippingMethods = new ShippingMethod[]
            {
                new ShippingMethod{ Name="Kurier UPS" },
                new ShippingMethod{ Name="Kurier DHL" },
                new ShippingMethod{ Name="Poczta" },
                new ShippingMethod{ Name="Paczkomat Inpost" }
            };
            foreach (ShippingMethod shippingMethod in shippingMethods)
            {
                context.ShippingMethods.Add(shippingMethod);
            }
            context.SaveChanges();

            /* PaymentMethods */
            if (context.PaymentMethods.Any())
            {
                return;
            }
            var paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod{ Name="Przelew Tradycyjny" },
                new PaymentMethod{ Name="Przelew Blik" },
                new PaymentMethod{ Name="Szybki Przelew" },
                new PaymentMethod{ Name="Karta" }
            };
            foreach (PaymentMethod paymentMethod in paymentMethods)
            {
                context.PaymentMethods.Add(paymentMethod);
            }
            context.SaveChanges();

            /* Orders */
            if (context.Orders.Any())
            {
                return;
            }
            var orders = new Order[2];
            if (userManager.FindByNameAsync("siemanokolano2137@gmail.com").Result != null)
            {
                var user = userManager.FindByNameAsync("siemanokolano2137@gmail.com").Result;
                orders[0] = new Order { CustomerID = user.Id, ShippingMethodID = 1, PaymentMethodID = 3, OrderStatus = State.InProgress, Price = 23996 };
            }
            if (userManager.FindByNameAsync("klientklientowski123@gmail.com").Result != null)
            {
                var user = userManager.FindByNameAsync("klientklientowski123@gmail.com").Result;
                orders[1] = new Order { CustomerID = user.Id, ShippingMethodID = 2, PaymentMethodID = 4, OrderStatus = State.Realized, Price = 12000 };
            }
            foreach (Order order in orders)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();

            /* Experts */
            if (context.Experts.Any())
            {
                return;
            }
            var experts = new Expert[]
            {
                new Expert{ FirstName="Ekspert", LastName="EkspertowiczK", Email="klientklientowski123@gmail.com"},
                new Expert{ FirstName="Ekspert2", LastName="Ekspertowicz2A", Email="siemanokolano2137@gmail.com"},
                new Expert{ FirstName="Ekspert3", LastName="Ekspertowicz3K", Email="klientklientowski123@gmail.com"}
            };
            foreach (Expert expert in experts)
            {
                context.Experts.Add(expert);
            }
            context.SaveChanges();

            /* Categories */
            if (context.Categories.Any())
            {
                return;
            }
            var categories = new Category[]
            {
                new Category{ Name="Elektronika", Visibility=true },
                new Category{ ParentCategoryID=4, Name="Smartfony", Visibility=true },
                new Category{ ParentCategoryID=4, Name="Laptopy", Visibility=true },
                new Category{ ParentCategoryID=4, Name="Komputery", Visibility=false}
            };
            foreach (Category category in categories)
            {
                context.Categories.Add(category);
            }
            context.SaveChanges();

            /* Products */
            if (context.Products.Any())
            {
                return;
            }
            var products = new Product[]
            {
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop LENOVO", ProductDescription="Dobry laptop", Image="~/Content/Images/Products/Laptop LENOVO.jpg", DateAdded=new DateTime(2018, 3, 20), Promotion=false, VAT=23, Price=4300, Amount=20, Visibility=true, SoldProducts=101 },
                new Product{ CategoryID=3, ExpertID=2, Name="Smartfon HUAWEI P30", ProductDescription="Dobry smartfon", Image="~/Content/Images/Products/Smartfon HUAWEI P30.jpg", DateAdded=new DateTime(2019, 10, 2), Promotion=true, VAT=23, Price=2999, Amount=10, Visibility=true, SoldProducts=290 },
                new Product{ CategoryID=2, ExpertID=3, Name="Laptop HUAWEI", ProductDescription="Dobry laptop", Image="~/Content/Images/Products/Laptop HUAWEI.png", DateAdded=DateTime.Now, Promotion=true, VAT=23, Price=5000, Amount=34, Visibility=true, SoldProducts=57 },
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop APPLE", ProductDescription="Dobry laptop", Image="~/Content/Images/Products/Laptop APPLE.jpg", DateAdded=DateTime.Now.AddDays(-10), Promotion=false, VAT=23, Price=4000, Amount=56, Visibility=true, SoldProducts=1080 },
                new Product{ CategoryID=2, ExpertID=2, Name="Laptop Acer Aspire", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_AcerAspire.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=2000, Amount=250, Visibility=true, SoldProducts=0 },
                new Product{ CategoryID=2, ExpertID=3, Name="Laptop Lenovo Y540", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_LenovoY540.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=5000, Amount=250, Visibility=true, SoldProducts=0 },
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop Acer Nitro", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_AcerNitro.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=2500, Amount=250, Visibility=true, SoldProducts=0 },
                new Product{ CategoryID=2, ExpertID=2, Name="Laptop ASUS E410", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_ASUSE410.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=3000, Amount=250, Visibility=true, SoldProducts=0 },
                new Product{ CategoryID=2, ExpertID=3, Name="Laptop ASUS TUF", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_ASUSTUF.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=3500, Amount=250, Visibility=true, SoldProducts=0 },
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop ASUS X543", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_ASUSX543.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=4000, Amount=250, Visibility=true, SoldProducts=0 },
                new Product{ CategoryID=2, ExpertID=2, Name="Laptop HP15-ec", ProductDescription="Superowy laptopik", Image="~/Content/Images/Products/Laptop_HP15-ec.jpg", DateAdded=DateTime.Now, Promotion=false, VAT=23, Price=4500, Amount=250, Visibility=true, SoldProducts=0 },
            };
            foreach (Product product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();

            /* Attachments */
            if (context.Attachments.Any())
            {
                return;
            }
            var attachments = new Attachment[]
            {
                new Attachment{ ProductID=1, Path="~/Content/Attachments/Dokumentacja projektu - PS2 - Grupa2.pdf", Description="Instrukcja obsługi laptopa" },
                new Attachment{ ProductID=2, Path="~/Content/Attachments/analiza_wariantow.pdf", Description="Instrukcja obsługi smartfona" },
                new Attachment{ ProductID=3, Path="~/Content/Attachments/Dokumentacja projektu - PS2 - Grupa2.pdf", Description="Instrukcja obsługi laptopa" },
                new Attachment{ ProductID=4, Path="~/Content/Attachments/analiza_wariantow.pdf", Description="Instrukcja obsługi laptopa" }
            };
            foreach (Attachment attachment in attachments)
            {
                context.Attachments.Add(attachment);
            }
            context.SaveChanges();

            /* ProductOrders */
            if (context.ProductOrders.Any())
            {
                return;
            }
            var productOrders = new ProductOrder[]
            {
                new ProductOrder{ OrderID=1, ProductID=1, Quantity=3 },
                new ProductOrder{ OrderID=2, ProductID=2, Quantity=2 },
                new ProductOrder{ OrderID=2, ProductID=3, Quantity=4 }
            };
            foreach (ProductOrder productOrder in productOrders)
            {
                context.ProductOrders.Add(productOrder);
            }
            context.SaveChanges();
        }
    }
}
