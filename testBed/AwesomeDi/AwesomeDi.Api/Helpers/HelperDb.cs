using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using AwesomeDi.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDi.Api.Helpers
{
    public class HelperDb
    {
        public static IQueryable<T> Sort<T>(string sortField, string sortDirection, IQueryable<T> query)
        {
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var propertyInfo = typeof(T).GetProperty(sortField,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                {
                    var sortDir = sortDirection?.ToLower() == "desc" ? "desc" : "asc";
                    query = query.OrderBy($"{sortField} {sortDir}");
                }
            }

            return query;
        }

        public static List<T> RawSqlQuery<T>(_DbContext.AwesomeDiContext context, string query,
            Func<DbDataReader, T> map)
        {
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                context.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (result.Read())
                    {
                        entities.Add(map(result));
                    }

                    return entities;
                }
            }
        }
    }
}
