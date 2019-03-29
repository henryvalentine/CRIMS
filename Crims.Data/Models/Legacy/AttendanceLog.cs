namespace Crims.Core.Models.Legacy
{
    public class AttendanceLog
    {
        public int Id { get; set; }
        public string TransactionCode { get; set; }
        public string TerminalId { get; set; }
        public string BaseDataId { get; set; }
        public string UserPrimaryCode { get; set; }
        public string ClockDate { get; set; }
        public string ClockTime { get; set; }
        public string TransactionDateTime { get; set; }
        public int TempleteId { get; set; }
        public int MatchingScore  { get; set; }
        public string LastUpdated { get; set; }
        public int ClockStatus { get; set; }
    }
}