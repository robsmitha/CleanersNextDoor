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
                    MerchantTypeID = merchantType.ID,
                    Name = "Five Star Cleaners",
                    Description = "Founded in 1987 by the Hernandez brothers, our establishment has been serving up rich coffee sourced from artisan farmers in various regions of South and Central America. We are dedicated to travelling the world, finding the best coffee, and bringing back to you here in our cafe.",
                    Street1 = "3481 Melrose Place",
                    City = "Beverly Hills",
                    StateAbbreviation = "CA",
                    Zip = "90210",
                    Phone = "(123) 456-7890",
                    ContactEmail = "name@example.com",
                    OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                    WebsiteUrl = "robsmitha.com",
                    CallToAction = "Once you try it, our coffee will be a blissful addition to your everyday morning routine - we guarantee it!",
                    ShortDescription = "Every cup of our quality artisan coffee starts with locally sourced, hand picked ingredients.",
                },
                new Merchant
                {
                    MerchantTypeID = merchantType.ID,
                    Name = "Second Base Cleaners",
                    Description = "Founded in 1987 by the Hernandez brothers, our establishment has been serving up rich coffee sourced from artisan farmers in various regions of South and Central America. We are dedicated to travelling the world, finding the best coffee, and bringing back to you here in our cafe.",
                    Street1 = "3481 Melrose Place",
                    City = "Beverly Hills",
                    StateAbbreviation = "CA",
                    Zip = "90210",
                    Phone = "(123) 456-7890",
                    ContactEmail = "name@example.com",
                    OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                    WebsiteUrl = "robsmitha.com",
                    CallToAction = "Once you try it, our coffee will be a blissful addition to your everyday morning routine - we guarantee it!",
                    ShortDescription = "Every cup of our quality artisan coffee starts with locally sourced, hand picked ingredients.",
                },
                new Merchant
                {
                    MerchantTypeID = merchantType.ID,
                    Name = "Third Time's the Charm Cleaners",
                    Description = "Founded in 1987 by the Hernandez brothers, our establishment has been serving up rich coffee sourced from artisan farmers in various regions of South and Central America. We are dedicated to travelling the world, finding the best coffee, and bringing back to you here in our cafe.",
                    Street1 = "3481 Melrose Place",
                    City = "Beverly Hills",
                    StateAbbreviation = "CA",
                    Zip = "90210",
                    Phone = "(123) 456-7890",
                    ContactEmail = "name@example.com",
                    OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                    WebsiteUrl = "robsmitha.com",
                    CallToAction = "Once you try it, our coffee will be a blissful addition to your everyday morning routine - we guarantee it!",
                    ShortDescription = "Every cup of our quality artisan coffee starts with locally sourced, hand picked ingredients.",
                },
                new Merchant
                {
                    MerchantTypeID = merchantType.ID,
                    Name = "Forty Oz Cleaners",
                    Description = "Founded in 1987 by the Hernandez brothers, our establishment has been serving up rich coffee sourced from artisan farmers in various regions of South and Central America. We are dedicated to travelling the world, finding the best coffee, and bringing back to you here in our cafe.",
                    Street1 = "3481 Melrose Place",
                    City = "Beverly Hills",
                    StateAbbreviation = "CA",
                    Zip = "90210",
                    Phone = "(123) 456-7890",
                    ContactEmail = "name@example.com",
                    OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                    WebsiteUrl = "robsmitha.com",
                    CallToAction = "Once you try it, our coffee will be a blissful addition to your everyday morning routine - we guarantee it!",
                    ShortDescription = "Every cup of our quality artisan coffee starts with locally sourced, hand picked ingredients.",
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
                new ItemType
                {
                    Active = true,
                    CreatedAt = DateTime.Now,
                    Name = "Fee",
                    Description = "Service Fee"
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

            var items = new List<Item>();
            foreach(var merchant in merchants)
            {
                items.Add(new Item
                {
                    ItemTypeID = itemTypes.Single(t => t.Name == "Laundry").ID,
                    MerchantID = merchant.ID,
                    Cost = 0,
                    Price = 1.99M,
                    PriceTypeID = priceTypes.First().ID,
                    UnitTypeID = unitTypes.First().ID,
                    Name = "Laundry item",
                    Description = "A single laundry item. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    MaxAllowed = 10
                });

                items.Add(new Item
                {
                    ItemTypeID = itemTypes.Single(t => t.Name == "Alteration").ID,
                    MerchantID = merchant.ID,
                    Cost = 0,
                    Price = 5.99M,
                    PriceTypeID = priceTypes.First().ID,
                    UnitTypeID = unitTypes.First().ID,
                    Name = "Clothing Alteration",
                    Description = "Alteration services. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    MaxAllowed = 4
                });

                items.Add(new Item
                {
                    ItemTypeID = itemTypes.Single(t => t.Name == "Dress").ID,
                    MerchantID = merchant.ID,
                    Cost = 0,
                    Price = 4.99M,
                    PriceTypeID = priceTypes.First().ID,
                    UnitTypeID = unitTypes.First().ID,
                    Name = "Dress Cleaning",
                    Description = "Dress cleaning services. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    MaxAllowed = 3
                });

                items.Add(new Item
                {
                    ItemTypeID = itemTypes.Single(t => t.Name == "Suit").ID,
                    MerchantID = merchant.ID,
                    Cost = 0,
                    Price = 4.99M,
                    PriceTypeID = priceTypes.First().ID,
                    UnitTypeID = unitTypes.First().ID,
                    Name = "Suit & Jacket Cleaning",
                    Description = "Suit & Jacket Cleaning services. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    MaxAllowed = 2
                });
            }
            context.Items.AddRange(items);
            context.SaveChanges();

            var serviceRequestStatusTypes = new List<ServiceRequestStatusType>
            {
                new ServiceRequestStatusType
                {
                    Name = "Created",
                    Description = "The service request has been created and we are confirming with the merchant."
                },
                new ServiceRequestStatusType
                {
                    Name = "Acknowledged",
                    Description = "We have recieved your service request. We are making arrangements to accomodate the request."
                },
                new ServiceRequestStatusType
                {
                    Name = "Picking Up",
                    Description = "The driver is picking up your items."
                },
                new ServiceRequestStatusType
                {
                    Name = "Merchant Recieved",
                    Description = "The merchant has recieved your service request and is making preparations."
                },
                new ServiceRequestStatusType
                {
                    Name = "Dropping Off",
                    Description = "The driver is dropping off your items."
                },
                new ServiceRequestStatusType
                {
                    Name = "Customer Recieved",
                    Description = "The driver has dropped off the items and the service request has been completed."
                },
                new ServiceRequestStatusType
                {
                    Name = "Cancelled",
                    Description = "The service request was cancelled."
                },
            };
            context.ServiceRequestStatusTypes.AddRange(serviceRequestStatusTypes);

            var correspondenceStatusTypes = new List<CorrespondenceStatusType>
            {
                new CorrespondenceStatusType
                {
                    Name = "Needs Confrimation",
                    Description = "The correspondence needs confrimation."
                },
                new CorrespondenceStatusType
                {
                    Name = "Confirmed",
                    Description = "The correspondence has been confirmed."
                },
                new CorrespondenceStatusType
                {
                    Name = "User Completing",
                    Description = "A user is completing the correspondence."
                },
                new CorrespondenceStatusType
                {
                    Name = "Completed",
                    Description = "A user has completed the correspondence."
                }
            };
            context.CorrespondenceStatusTypes.AddRange(correspondenceStatusTypes);


            var customerPickUp = new CorrespondenceType
            {
                Name = "Customer Pick up",
                Description = "Pick up from customer."
            };
            context.CorrespondenceTypes.Add(customerPickUp);
            var customerDropOff = new CorrespondenceType
            {
                Name = "Customer Drop Off",
                Description = "Drop off to customer."
            };
            context.CorrespondenceTypes.Add(customerDropOff);
            var merchantPickUp = new CorrespondenceType
            {
                Name = "Merchant Pick up",
                Description = "Pick up from merchant."
            };
            context.CorrespondenceTypes.Add(merchantPickUp);
            var merchantDropOff = new CorrespondenceType
            {
                Name = "Merchant Drop Off",
                Description = "Drop off to merchant."
            };
            context.CorrespondenceTypes.Add(merchantDropOff);
            context.SaveChanges();

            foreach (var m in merchants)
            {
                var laundryWorkflow = new Workflow
                {
                    Name = "Laundry Service",
                    MerchantID = m.ID
                };
                var foodDeliveryWorkflow = new Workflow
                {
                    Name = "Food Delivery",
                    MerchantID = m.ID
                };
                context.Workflows.Add(laundryWorkflow);
                context.Workflows.Add(foodDeliveryWorkflow);
                context.SaveChanges();

                //laundry workflow
                context.WorkflowItems.Add(new WorkflowItem
                {
                    WorkflowID = laundryWorkflow.ID,
                    WorkflowOrder = 1,
                    CorrespondenceTypeID = customerPickUp.ID
                });
                context.WorkflowItems.Add(new WorkflowItem
                {
                    WorkflowID = laundryWorkflow.ID,
                    WorkflowOrder = 2,
                    CorrespondenceTypeID = merchantDropOff.ID
                });
                context.WorkflowItems.Add(new WorkflowItem
                {
                    WorkflowID = laundryWorkflow.ID,
                    WorkflowOrder = 3,
                    CorrespondenceTypeID = merchantPickUp.ID
                });
                context.WorkflowItems.Add(new WorkflowItem
                {
                    WorkflowID = laundryWorkflow.ID,
                    WorkflowOrder = 4,
                    CorrespondenceTypeID = customerDropOff.ID
                });

                //food delivery workflow
                context.WorkflowItems.Add(new WorkflowItem
                {
                    WorkflowID = foodDeliveryWorkflow.ID,
                    WorkflowOrder = 1,
                    CorrespondenceTypeID = merchantPickUp.ID
                });
                context.WorkflowItems.Add(new WorkflowItem
                {
                    WorkflowID = foodDeliveryWorkflow.ID,
                    WorkflowOrder = 2,
                    CorrespondenceTypeID = customerDropOff.ID
                });
            }

            var userStatusTypes = new List<UserStatusType>
            {
                new UserStatusType
                {
                    Name = "Submitted / Unconfirmed",
                    Description = "The user has signed up and needs to confirm their email."
                },
                new UserStatusType
                {
                    Name = "Confirmed",
                    Description = "The user has confirmed their email and is ready for consideration."
                },
                new UserStatusType
                {
                    Name = "Under Consideration",
                    Description = "The user information is being reviewed."
                },
                new UserStatusType
                {
                    Name = "Approved",
                    Description = "The user is approved to carry out service request correspondences."
                },
                new UserStatusType
                {
                    Name = "Denied",
                    Description = "The user is not approved to carry out services."
                },
                new UserStatusType
                {
                    Name = "Terminated",
                    Description = "The user has been terminated."
                }
            };
            context.UserStatusTypes.AddRange(userStatusTypes);
            context.SaveChanges();
        }
    }
}
