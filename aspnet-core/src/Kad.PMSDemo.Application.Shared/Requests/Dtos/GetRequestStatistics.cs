using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Requests.Dtos
{
    public class GetRequestStatistics
    {
        public RequestStatus RequestStatusId { get; set; }

        public RequestType RequestTypeId { get; set; }
    }
}
