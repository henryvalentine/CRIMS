namespace Crims.UI.Win.Enroll.Helpers
{
    public class EnumManager
    {
        public enum TitleEnum
        {
            Mr = 1,
            Mrs,
            Master,
            Miss,
            Chief,
            Chief_Mrs,
            Knight,
            Lady,
            Dr,
            Dr_Mrs,
            Barr,
            SAN,
            Engr,
            Rotr
        }

        public enum GenderEnum
        {
            Male = 1,
            Female
        }

        public enum ApprovalStatus
        {
            Pending = 1,
            Approved,
            Rejected,
            LockedForApproval,
            Recycled
        }

        public enum FingerDescription
        {
            LFLittle = 1,
            LFRing = 2,
            LFMiddle = 3,
            LFIndex = 4,
            LFThumb = 5,
            RFThumb = 6,
            RFIndex = 7,
            RFMiddle = 8,
            RFRing = 9,
            RFLittle = 10,
            Unknown = 11
        }
    }
}
