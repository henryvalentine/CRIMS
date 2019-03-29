using System.Data.Entity.Infrastructure;
using System.IO;
using Crims.Core.Logging;
using Crims.Data.Models;

namespace Crims.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CrimsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
        }

        //protected override void Seed(Crims.Data.CrimsDbContext context)
        //{
               
        //    try
        //    {
        //        context.CustomFieldTypes.AddOrUpdate(
        //         p => p.FieldTypeName,
        //         new CustomFieldType { FieldTypeName = "Text", FieldTypeId = "f8b7baa5068c466783212ab425087bdc" },
        //         new CustomFieldType { FieldTypeName = "List", FieldTypeId = "df5ec50d2980440ab3563db9cc26f730" },
        //         new CustomFieldType { FieldTypeName = "Date", FieldTypeId = "d393ac8623e6401085d63930a187de95" },
        //         new CustomFieldType { FieldTypeName = "Number", FieldTypeId = "307c1994d77740b2ba0d44ac698affff"}
        //       );
              
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.LoggError(ex.StackTrace, ex.Source, ex.Message);
        //    }
            
        //}
    }
}
