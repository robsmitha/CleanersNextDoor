using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ICleanersNextDoorContext context)
        {
            if (!context.EnsureCreated())
            {
                return; //db was already created or error occurred
            }
            //add merchant types
            var merchantType = new MerchantType
            {
                Name = "Cleaner",
                Description = "Cleaner Services",
                Active = true,
                CreatedAt = DateTime.Now
            };
            context.MerchantTypes.Add(merchantType);
            context.SaveChanges();

            //add test merchants
            var merchants = new List<Merchant>
            {
                new Merchant
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    MerchantTypeID = merchantType.ID,
                    Name = "First Star Cleaners",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rem magni quas ex numquam, maxime minus quam molestias corporis quod, ea minima accusamus."
                },
                new Merchant
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    MerchantTypeID = merchantType.ID,
                    Name = "Second Base Cleaners",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rem magni quas ex numquam, maxime minus quam molestias corporis quod, ea minima accusamus."
                },
                new Merchant
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    MerchantTypeID = merchantType.ID,
                    Name = "Third Time's the Charm Cleaners",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rem magni quas ex numquam, maxime minus quam molestias corporis quod, ea minima accusamus."
                },
                new Merchant
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    MerchantTypeID = merchantType.ID,
                    Name = "Forty Oz Cleaners",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Rem magni quas ex numquam, maxime minus quam molestias corporis quod, ea minima accusamus."
                },
            };
            context.Merchants.AddRange(merchants);
            context.SaveChanges();

            //add price type
            var priceTypes = new List<PriceType>
            {
                new PriceType
                {
                    Name = "Fixed",
                    Description = "Fixed Price",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                },
                new PriceType
                {
                    Name = "Variable",
                    Description = "Variable Cost Price",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                }
            };

            context.PriceTypes.AddRange(priceTypes);

            //add item types
            var itemTypes = new List<ItemType>
            {
                new ItemType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "Laundry",
                    Description = "Laundry Services"
                },
                new ItemType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "Suit",
                    Description = "Suit Services"
                },
                new ItemType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "Dress",
                    Description = "Dress Services"
                },
                new ItemType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "Alteration",
                    Description = "Alteration Services"
                },
            };
            context.ItemTypes.AddRange(itemTypes);

            //add card types
            var cardTypes = new List<CardType>
            {
                new CardType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "Visa",
                    Description = "Visa Card"
                },
                new CardType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "America Express",
                    Description = "America Express"
                },
            };
            context.CardTypes.AddRange(cardTypes);

            //unit types
            var unitTypes = new List<UnitType>
            {
                new UnitType
                {
                    Name = "Quantity",
                    Description = "Unit is measured in quantities",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                }
            };
            context.UnitTypes.AddRange(unitTypes);

            var owner = new Role
            {
                Name = "Owner",
                Description = "Owner of the associated merchant",
                CreatedAt = DateTime.UtcNow,
                Active = true,
            };
            var onlineSignUp = new Role
            {
                Name = "Online User",
                Description = "Online Sign up user",
                CreatedAt = DateTime.UtcNow,
                Active = true,
            };
            var roles = new List<Role>
            {
                owner,
                onlineSignUp
            };
            context.Roles.AddRange(roles);

            var orderStatusTypes = new List<OrderStatusType>
            {
               new OrderStatusType
               {
                   Name = "Open",
                    Description = "Order needs to be paid for",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
               new OrderStatusType
               {
                   Name = "Paid",
                    Description = "Order has been paid for",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
               new OrderStatusType
               {
                    Name = "Partially Paid",
                    Description = "Order has been partially paid for",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               }
            };
            context.OrderStatusTypes.AddRange(orderStatusTypes);


            var paymentStatusTypes = new List<PaymentStatusType>
            {
               new PaymentStatusType
               {
                   Name = "Pending",
                    Description = "Payment is pending",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
               new PaymentStatusType
               {
                   Name = "Paid",
                    Description = "Payment is paid for",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
            };
            context.PaymentStatusTypes.AddRange(paymentStatusTypes);

            var paymentTypes = new List<PaymentType>
            {
               new PaymentType
               {
                   Name = "Credit Card Manual",
                    Description = "Credit Card Manually Entered",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
               new PaymentType
               {
                   Name = "Cash",
                    Description = "Cash",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
               new PaymentType
               {
                   Name = "Check",
                    Description = "Check",
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
               },
            };

            context.PaymentTypes.AddRange(paymentTypes);
            context.SaveChanges();
        }
    }
}
