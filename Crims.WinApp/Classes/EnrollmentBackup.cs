using System.Collections.Generic;
using Crims.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Crims.UI.Win.Enroll.Classes
{
    public class EnrollmentBackup
    {
        [JsonProperty("approvals")]
        public Approval[] Approvals { get; set; }
        [JsonProperty("customDatas")]
        public CustomData[] CustomDatas { get; set; }
        [JsonProperty("baseData")]
        public BaseData BaseData { get; set; }
        [JsonProperty("fingerprintImages")]
        public FingerprintImage[] FingerprintImages { get; set; }
        [JsonProperty("fingerprintReasons")]
        public FingerprintReason[] FingerprintReasons { get; set; }
        [JsonProperty("fingerprintTemplate")]
        public FingerprintTemplate FingerprintTemplate { get; set; }
        [JsonProperty("photograph")]
        public Photograph Photograph { get; set; }
        [JsonProperty("signature")]
        public Signature Signature { get; set; }
        
    }

    public class FileDesc
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }

        public FileDesc(string n, string p, long s)
        {
            Name = n;
            Path = p;
            Size = s;
        }
    }
}