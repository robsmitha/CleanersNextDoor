using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CleanersNextDoorContext context)
        {
            if (!context.Database.EnsureCreated())
            {
                return; //db was already created or error occurred
            }

        }
    }
}
