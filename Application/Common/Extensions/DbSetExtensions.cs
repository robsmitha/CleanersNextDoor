using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Common.Extensions
{
    public static class DbSetExtensions
    {
        /// <summary>
        /// This performance optimization turns off entity framework change tracking (NOTE: breaks foreign key mappings).
        /// For enumerables that wont be sending updates to the db.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <returns></returns>
        public static IEnumerable<TEntity> AsUntrackedEnumerable<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            if (dbSet == null) return new List<TEntity>();

            return dbSet.AsNoTracking().AsEnumerable();
        }
    }
}
