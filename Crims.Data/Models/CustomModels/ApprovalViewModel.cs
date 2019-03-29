using System;
using System.Collections.Generic;
using Crims.Data.Models;
using Repository.Pattern.Ef6;

namespace Crims.Data.Models
{
    public partial class ApprovalViewModel
    {
        public int TableId { get; set; }
        public string ApprovalId { get; set; }
        public string Comment { get; set; }
        public string EnrollmentId { get; set; }
        public DateTime DateProcessed { get; set; }
        public string DateProcessedStr { get; set; }
        public string ProcessedById { get; set; }
        public string ProcessedByName { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalStatusStr { get; set; }
        public string ProjectPrimaryCode { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
    }

}

