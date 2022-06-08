using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Test
{
    public enum StaffEntityType
    {
        Requester, Consultant
    }

    public enum RequestType
    {
        Consultation, Enquiry, FS_Review
    }

    public enum RequestStatus
    {
        Requested, Assigned, Prepared, Completed, Rejected, AwaitingTOR, WIP, Accepted, CMASManagerApproved, CMASManagerRequestReview, Returned
    }

    public enum ChannelType
    {
        [Description("Channel 1")] Channel_1,
        [Description("Channel 2")] Channel_2
    }

    public enum Level
    {
        Intern,
        Associate,
        [Description("Senior Associate")] Senior_Associate,
        Manager,
        [Description("Senior Manager")] Senior_Manager,
        [Description("Associate Director")] Associate_Director,
        Partner
    }

    public enum FinYearEnd
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    public enum StaffCategory
    {
        Consultant,
        Manager,
        Partner
    }

    public enum AttachmentTypes
    {
        [Description("Signed Terms of Reference")] SignedTOR,
        [Description("Attachment")] Attachment,
        [Description("Reviewed Financial Statement")] ReviewedFS
    }

}
