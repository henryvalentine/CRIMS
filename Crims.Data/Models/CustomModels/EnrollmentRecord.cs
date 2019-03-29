using Crims.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Crims.Data.Models
{
    public class EnrollmentRecord
    {
        public string ProjectCode { get; set; }
        public string EnrollmentId { get; set; }
        public BaseData BaseData { get; set; }
        public IList<CustomData> CustomDatas { get; set; }
        public IList<FingerprintReason> FingerprintReasons { get; set; }
        public IList<FingerprintImage> FingerprintImages { get; set; }
        public FingerprintTemplate FingerprintTemplate { get; set; }
        public Photograph Photograph { get; set; }
        public Signature Signature { get; set; }
    }

    public class EnrolleeRecord
    {
        public string ProjectCode { get; set; }
        public string EnrollmentId { get; set; }
        public BaseData BaseData { get; set; }
        public IList<CustomData> CustomDatas { get; set; }
        public HttpPostedFileBase UserRecord { get; set; }
    }

    public class EnrolleeDataFrorm
    {
        public string ProjectCode { get; set; }
        public List<DataFrorm> DataFrorms { get; set; }
    }

    public class DataFrorm
    {
        public string EnrollmentId { get; set; }
        public FileStream EnrollmentForm { get; set; }
    }
}
 