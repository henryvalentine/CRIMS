namespace Crims.Core.Utils
{
    public enum MessageEventEnum
    {
        New_Account = 1,
        New_User,
        Account_Confirmation,
        Password_Reset,
        Password_Reset_Successful,
        Activation_Link_Request,
        Payment_Alert
    }

    public enum MessageStatus
    {
        Sent = 1,
        Pending,
        Failed
    }
    
    public enum UserStatus
    {
        Active = 1,
        InActive,
        Black_Listed,
        Penalised,
        Suspended
        
    }

    public enum FeedbackStatus
    {
        Pending = 1,
        Suspended,
        Resolved

    }
    

}