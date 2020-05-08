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
            var cleanersMerchantType = new MerchantType
            {
                Name = "Cleaners",
                Description = "Cleaners Services"
            };
            var foodDeliveryMerchantType = new MerchantType
            {
                Name = "Food Delivery",
                Description = "Food Delivery"
            };
            context.MerchantTypes.Add(cleanersMerchantType);
            context.MerchantTypes.Add(foodDeliveryMerchantType);
            context.SaveChanges();

            var testMerchant1 = new Merchant
            {
                MerchantTypeID = cleanersMerchantType.ID,
                Name = "Five Star Cleaners",
                Description = "We know you are on the go and it is not easy to handle all of your day to day chores without feeling stressed! Errands are a part of life but what if you could free up this time in your routine? We can help! Let us show you our first class dry cleaning and laundry services and you'll see why we are the best laundry service around. We have been in the biz since 1994 and we know our stuff. You can trust the professionals to take care of your laundry services. Try us out today and see for yourself!",
                WebsiteUrl = "robsmitha.com",
                CallToAction = "Once you try our services, you will love the newfound free time in your routine - we guarantee it!",
                ShortDescription = "No other laundry service can even come close to impressing you like ours.",
            };
            var testMerchant2 = new Merchant
            {
                MerchantTypeID = cleanersMerchantType.ID,
                Name = "Easy Street Cleaners",
                Description = "We know you are on the go and it is not easy to handle all of your day to day chores without feeling stressed! Errands are a part of life but what if you could free up this time in your routine? We can help! Let us show you our first class dry cleaning and laundry services and you'll see why we are the best laundry service around. We have been in the biz since 1994 and we know our stuff. You can trust the professionals to take care of your laundry services. Try us out today and see for yourself!",
                WebsiteUrl = "robsmitha.com",
                CallToAction = "Once you try our services, you will love the newfound free time in your routine - we guarantee it!",
                ShortDescription = "Our services use environment friendly products so you can feel good about your impact.",
            };
            var testMerchant3 = new Merchant
            {
                MerchantTypeID = cleanersMerchantType.ID,
                Name = "Lucky Charm Cleaners",
                Description = "We know you are on the go and it is not easy to handle all of your day to day chores without feeling stressed! Errands are a part of life but what if you could free up this time in your routine? We can help! Let us show you our first class dry cleaning and laundry services and you'll see why we are the best laundry service around. We have been in the biz since 1994 and we know our stuff. You can trust the professionals to take care of your laundry services. Try us out today and see for yourself!",
                WebsiteUrl = "robsmitha.com",
                CallToAction = "Once you try our services, you will love the newfound free time in your routine - we guarantee it!",
                ShortDescription = "We'll be your good luck charm!",
            };

            var testMerchant4 = new Merchant
            {
                MerchantTypeID = cleanersMerchantType.ID,
                Name = "Unicorn Cleaners",
                Description = "We know you are on the go and it is not easy to handle all of your day to day chores without feeling stressed! Errands are a part of life but what if you could free up this time in your routine? We can help! Let us show you our first class dry cleaning and laundry services and you'll see why we are the best laundry service around. We have been in the biz since 1994 and we know our stuff. You can trust the professionals to take care of your laundry services. Try us out today and see for yourself!",
                WebsiteUrl = "robsmitha.com",
                CallToAction = "Once you try our services, you will love the newfound free time in your routine - we guarantee it!",
                ShortDescription = "A truly amazing cleaners with awesome Google reviews.",
            };

            //add test merchants
            var merchants = new List<Merchant>
            {
                testMerchant1,
                testMerchant2,
                testMerchant3,
                testMerchant4,
            };
            context.Merchants.AddRange(merchants);
            context.SaveChanges();

            var merchantImages = new List<MerchantImage>
            {
                new MerchantImage
                {
                    IsDefault = true,
                    MerchantID = testMerchant1.ID,
                    ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/work1.jpg"
                },
                new MerchantImage
                {
                    IsDefault = true,
                    MerchantID = testMerchant2.ID,
                    ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/work2.jpg"
                },
                new MerchantImage
                {
                    IsDefault = true,
                    MerchantID = testMerchant3.ID,
                    ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/work3.jpg"
                },
                new MerchantImage
                {
                    IsDefault = true,
                    MerchantID = testMerchant4.ID,
                    ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/work4.jpg"
                }
            };
            context.MerchantImages.AddRange(merchantImages);

            //add price type
            var priceTypes = new List<PriceType>
            {
                new PriceType
                {
                    Name = "Fixed",
                    Description = "Fixed Price"
                },
                new PriceType
                {
                    Name = "Variable",
                    Description = "Variable Cost Price",
                    IsVariableCost = true
                }
            };

            context.PriceTypes.AddRange(priceTypes);

            //add item types
            var itemTypes = new List<ItemType>
            {
                new ItemType
                {
                    Name = "Laundry",
                    Description = "Laundry Services"
                },
                new ItemType
                {
                    Name = "Suit",
                    Description = "Suit Services"
                },
                new ItemType
                {
                    Name = "Dress",
                    Description = "Dress Services"
                },
                new ItemType
                {
                    Name = "Alteration",
                    Description = "Alteration Services"
                },
                new ItemType
                {
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
                    Name = "Visa",
                    Description = "Visa Card"
                },
                new CardType
                {
                    Name = "America Express",
                    Description = "America Express"
                },
                new CardType
                {
                    Name = "Discover",
                    Description = "Discover"
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
                    PerUnit = "Each"
                }
            };
            context.UnitTypes.AddRange(unitTypes);

            var owner = new Role
            {
                Name = "Owner",
                Description = "Owner of the associated merchant"
            };
            var onlineSignUp = new Role
            {
                Name = "Online User",
                Description = "Online Sign up user"
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
                    CanAddPayment = true,
                    CanAddLineItem = true
               },
               new OrderStatusType
               {
                   Name = "Paid",
                    Description = "Order has been paid for"
               },
               new OrderStatusType
               {
                    Name = "Partially Paid",
                    Description = "Order has been partially paid for",
                    CanAddPayment = true
               },
               new OrderStatusType
               {
                    Name = "Pending",
                    Description = "Order is pending due to a checkout failure"
               }
            };
            context.OrderStatusTypes.AddRange(orderStatusTypes);


            var paymentStatusTypes = new List<PaymentStatusType>
            {
               new PaymentStatusType
               {
                   Name = "Pending",
                    Description = "Payment is pending"
               },
               new PaymentStatusType
               {
                   Name = "Paid",
                    Description = "Payment is paid for"
               },
               new PaymentStatusType
               {
                   Name = "Failed",
                    Description = "Payment failed"
               }
            };
            context.PaymentStatusTypes.AddRange(paymentStatusTypes);

            var paymentTypes = new List<PaymentType>
            {
               new PaymentType
               {
                   Name = "Credit Card Manual",
                    Description = "Credit Card Manually Entered"
               },
               new PaymentType
               {
                   Name = "Cash",
                    Description = "Cash",
               },
               new PaymentType
               {
                    Name = "Check",
                    Description = "Check",
               },
            };

            context.PaymentTypes.AddRange(paymentTypes);
            context.SaveChanges();

            var laundryType = itemTypes.Single(t => t.Name == "Laundry");
            var alterationType = itemTypes.Single(t => t.Name == "Alteration");
            var dressType = itemTypes.Single(t => t.Name == "Dress");
            var suitType = itemTypes.Single(t => t.Name == "Suit");

            var items = new List<Item>();
            for (int i = 0; i < merchants.Count; i++)
            {
                var m = merchants[i];
                var laundry = new Item
                {
                    MerchantID = m.ID,
                    ItemTypeID = laundryType.ID,
                    Cost = 0,
                    Price = 1.99M,
                    PriceTypeID = priceTypes.First().ID,
                    UnitTypeID = unitTypes.First().ID,
                    Name = "Laundry item",
                    Description = "A single laundry item. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    MaxAllowed = 25,
                    ShortDescription = "Easy wash and fold."
                };
                items.Add(laundry);


                var alteration = new Item
                {
                    MerchantID = m.ID,
                    ItemTypeID = alterationType.ID,
                    Cost = 0,
                    Price = 5.99M,
                    PriceTypeID = priceTypes.First().ID,
                    UnitTypeID = unitTypes.First().ID,
                    Name = "Clothing Alteration",
                    Description = "Alteration services. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                    MaxAllowed = 7,
                    ShortDescription = "Alteration serivces at a great price."
                };
                items.Add(alteration);

                var even = i == 0 || i % 2 == 0;
                if (even)
                {
                    var suit = new Item
                    {
                        MerchantID = m.ID,
                        ItemTypeID = suitType.ID,
                        Cost = 0,
                        Price = 4.99M,
                        PriceTypeID = priceTypes.First().ID,
                        UnitTypeID = unitTypes.First().ID,
                        Name = "Suit & Jacket Cleaning",
                        Description = "Suit & Jacket Cleaning services. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                        MaxAllowed = 9,
                        ShortDescription = "Go in there with a nice clean suit."
                    };
                    items.Add(suit);
                }
                else
                {
                    var dress = new Item
                    {
                        MerchantID = m.ID,
                        ItemTypeID = dressType.ID,
                        Cost = 0,
                        Price = 4.99M,
                        PriceTypeID = priceTypes.First().ID,
                        UnitTypeID = unitTypes.First().ID,
                        Name = "Dress Cleaning",
                        Description = "Dress cleaning services. Lorem ipsum dolor sit amet, consectetur adipisicing elit.",
                        MaxAllowed = 9,
                        ShortDescription = "Nothing like a clean dress."
                    };
                    items.Add(dress);
                }
            }

            context.Items.AddRange(items);
            context.SaveChanges();

            foreach (var i in items)
            {
                if(i.ItemTypeID == laundryType.ID)
                {
                    context.ItemImages.Add(new ItemImage
                    {
                        IsDefault = true,
                        ItemID = i.ID,
                        ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/laundry.jpg"
                    });
                }
                else if (i.ItemTypeID == alterationType.ID)
                {
                    context.ItemImages.Add(new ItemImage
                    {
                        IsDefault = true,
                        ItemID = i.ID,
                        ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/alterations.jpg"
                    });
                }
                else if (i.ItemTypeID == dressType.ID)
                {
                    context.ItemImages.Add(new ItemImage
                    {
                        IsDefault = true,
                        ItemID = i.ID,
                        ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/dress.jpg"
                    });
                }
                else if (i.ItemTypeID == suitType.ID)
                {
                    context.ItemImages.Add(new ItemImage
                    {
                        IsDefault = true,
                        ItemID = i.ID,
                        ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/suit.jpg"
                    });
                }
                else
                {
                    //default image
                    context.ItemImages.Add(new ItemImage
                    {
                        IsDefault = true,
                        ItemID = i.ID,
                        ImageUrl = "https://smitha-cdn.s3.us-east-2.amazonaws.com/Content/images/shirts.jpg"
                    });
                }
            }

            var confirmedServiceRequestStatusType = new ServiceRequestStatusType
            {
                Name = "Confirmed",
                Description = "We have confirmed your service request with the merchant.",
                IsActiveServiceRequest = true
            };
            var serviceRequestStatusTypes = new List<ServiceRequestStatusType>
            {
                new ServiceRequestStatusType
                {
                    Name = "Created",
                    Description = "The service request has been created and we are confirming with the merchant."
                },
                confirmedServiceRequestStatusType,
                new ServiceRequestStatusType
                {
                    Name = "Picking Up",
                    Description = "The driver is picking up your items.",
                    IsActiveServiceRequest = true
                },
                new ServiceRequestStatusType
                {
                    Name = "Merchant Recieved",
                    Description = "The merchant has recieved your service request and is making preparations.",
                    IsActiveServiceRequest = true
                },
                new ServiceRequestStatusType
                {
                    Name = "Dropping Off",
                    Description = "The driver is dropping off your items.",
                    IsActiveServiceRequest = true
                },
                new ServiceRequestStatusType
                {
                    Name = "Customer Recieved",
                    Description = "The driver has dropped off the items and the service request has been completed.",
                    IsCompleteServiceRequest = true
                },
                new ServiceRequestStatusType
                {
                    Name = "Cancelled",
                    Description = "The service request was cancelled.",
                    IsCompleteServiceRequest = true
                },
            };
            context.ServiceRequestStatusTypes.AddRange(serviceRequestStatusTypes);

            var correspondenceStatusTypes = new List<CorrespondenceStatusType>
            {
                new CorrespondenceStatusType
                {
                    Name = "Needs Confirmation",
                    Description = "The correspondence needs confirmation."
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
                Description = "Pick up from customer.",
                CustomerConfigures = true
            };
            var customerDropOff = new CorrespondenceType
            {
                Name = "Customer Drop Off",
                Description = "Drop off to customer.",
                CustomerConfigures = true
            };
            var merchantPickUp = new CorrespondenceType
            {
                Name = "Merchant Pick up",
                Description = "Pick up from merchant.",
                MerchantConfigures = true
            };
            var merchantDropOff = new CorrespondenceType
            {
                Name = "Merchant Drop Off",
                Description = "Drop off to merchant.",
                MerchantConfigures = true
            };
            var correspondenceTypes = new[]
            {
                customerPickUp,
                customerDropOff,
                merchantPickUp,
                merchantDropOff
            };
            context.CorrespondenceTypes.AddRange(correspondenceTypes);
            context.SaveChanges();


            var laundryWorkflow = new Workflow
            {
                Name = "Laundry Service",
                Description = "Laundry Service"
            };
            var foodDeliveryWorkflow = new Workflow
            {
                Name = "Food Delivery",
                Description = "Food Delivery"
            };
            context.Workflows.Add(laundryWorkflow);
            context.Workflows.Add(foodDeliveryWorkflow);
            context.SaveChanges();

            //laundry workflow
            context.WorkflowSteps.Add(new WorkflowStep
            {
                WorkflowID = laundryWorkflow.ID,
                Step = 1,
                CorrespondenceTypeID = customerPickUp.ID
            });
            context.WorkflowSteps.Add(new WorkflowStep
            {
                WorkflowID = laundryWorkflow.ID,
                Step = 2,
                CorrespondenceTypeID = merchantDropOff.ID
            });
            context.WorkflowSteps.Add(new WorkflowStep
            {
                WorkflowID = laundryWorkflow.ID,
                Step = 3,
                CorrespondenceTypeID = merchantPickUp.ID
            });
            context.WorkflowSteps.Add(new WorkflowStep
            {
                WorkflowID = laundryWorkflow.ID,
                Step = 4,
                CorrespondenceTypeID = customerDropOff.ID
            });

            //food delivery workflow
            context.WorkflowSteps.Add(new WorkflowStep
            {
                WorkflowID = foodDeliveryWorkflow.ID,
                Step = 1,
                CorrespondenceTypeID = merchantPickUp.ID
            });
            context.WorkflowSteps.Add(new WorkflowStep
            {
                WorkflowID = foodDeliveryWorkflow.ID,
                Step = 2,
                CorrespondenceTypeID = customerDropOff.ID
            });

            foreach (var m in merchants)
            {
                context.MerchantWorkflows.Add(new MerchantWorkflow
                {
                    WorkflowID = laundryWorkflow.ID,
                    MerchantID = m.ID,
                    IsDefault = true,
                    DefaultServiceRequestStatusTypeID = confirmedServiceRequestStatusType.ID
                });
                context.MerchantWorkflows.Add(new MerchantWorkflow
                {
                    WorkflowID = foodDeliveryWorkflow.ID,
                    MerchantID = m.ID
                });
            }

            var testMerchant1Locations = new List<MerchantLocation>
            {
                new MerchantLocation
                {
                    City = "Tampa",
                    StateAbbreviation = "FL",
                    Latitude = 27.950575,
                    Longitude = -82.457176,
                    MerchantID = testMerchant1.ID,
                        Phone = "(123) 456-7890",
                        ContactEmail = "name@example.com",
                        OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                },
                new MerchantLocation
                {
                    City = "Tallahassee",
                    StateAbbreviation = "FL",
                    Latitude = 30.455000,
                    Longitude = -84.253334,
                    MerchantID = testMerchant1.ID,
                        Phone = "(123) 456-7890",
                        ContactEmail = "name@example.com",
                        OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                        IsDefault = true
                },
            };

            var testMerchant2Locations = new List<MerchantLocation>
            {
                new MerchantLocation
                {
                    City = "Tallahassee",
                    StateAbbreviation = "FL",
                    Latitude = 30.455000,
                    Longitude = -84.253334,
                    MerchantID = testMerchant2.ID,
                        Phone = "(123) 456-7890",
                        ContactEmail = "name@example.com",
                        OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                        IsDefault = true
                },
                new MerchantLocation
                {
                    City = "New York",
                    StateAbbreviation = "NY",
                    Latitude = 40.712776,
                    Longitude =  -74.005974,
                    MerchantID = testMerchant2.ID,
                        Phone = "(123) 456-7890",
                        ContactEmail = "name@example.com",
                        OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                },
            };
            var merchantLocations = new List<MerchantLocation>
            {
                new MerchantLocation
                {
                    City = "Beverly Hills",
                    StateAbbreviation = "CA",
                    Latitude = 34.077200,
                    Longitude = -118.422450,
                    MerchantID = testMerchant3.ID,
                        Phone = "(123) 456-7890",
                        ContactEmail = "name@example.com",
                        OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                        IsDefault = true
                },
                new MerchantLocation
                {
                    City = "Redmond",
                    StateAbbreviation = "WA",
                    Latitude = 47.751076,
                    Longitude =  -120.740135,
                    MerchantID = testMerchant4.ID,
                    Phone = "(123) 456-7890",
                    ContactEmail = "name@example.com",
                    OperatingHours = "Monday - Friday: 9:00 AM to 5:00 PM",
                        IsDefault = true
                }
            };
            merchantLocations.AddRange(testMerchant1Locations);
            merchantLocations.AddRange(testMerchant2Locations);

            for (int i = 0; i < merchantLocations.Count; i++)
            {
                var m = merchantLocations[i];
                foreach (var ct in correspondenceTypes.Where(c => c.MerchantConfigures))
                {
                    //configuring dropoff/pickup location
                    context.MerchantLocations.Add(new MerchantLocation
                    {
                        Street1 = m.Street1,
                        City = m.City,
                        StateAbbreviation = m.StateAbbreviation,
                        Zip = m.Zip,
                        Latitude = m.Latitude,
                        Longitude = m.Longitude,
                        MerchantID = m.MerchantID,
                        IsDefault = m.IsDefault,
                        CorrespondenceTypeID = ct.ID,
                        Phone = m.Phone,
                        ContactEmail = m.ContactEmail,
                        OperatingHours = m.OperatingHours,
                    });
                }
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
