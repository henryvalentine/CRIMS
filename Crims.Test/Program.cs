using Crims.Core.BusinessObjects;
using Crims.Core.Services;
using Crims.Data;
using Crims.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TandAProject.Services;

namespace Crims.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CrimsDbContext db = new CrimsDbContext();
            Project project = new Project
            {

                ActivationCode = "",
                DateCreated = DateTime.Now,
                LicenceCode = "",
                LicenseExpiryDate = DateTime.Now.AddYears(5),
                OnlineMode = 0,
                ProjectCode = "PRC001",
                ProjectDescription = "Test Project",
                ProjectName = "A Test Project"
            };

            db.Projects.Add(project);
            db.SaveChanges();
            Console.ReadLine();
        }
    }
}
