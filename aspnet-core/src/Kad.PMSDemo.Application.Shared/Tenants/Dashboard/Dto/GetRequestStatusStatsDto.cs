using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Tenants.Dashboard.Dto
{
    public class GetRequestStatusStatsDto
    {
        public string[] Status;
        public int[] Consultation;
        public int[] Enquiry;
        public int[] FS_Review;
    }

    public class RequestGroupByStatus
    {
        public RequestStatus Status { get; set; }
        public int Count { get; set; }
    }
}
