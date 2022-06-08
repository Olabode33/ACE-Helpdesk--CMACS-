using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Requests.Dtos
{
    public class TechTeamTmpDto
    {
        public int TimeCharge { get; set; }

        public StaffCategory Role { get; set; }

        public int? RequestId { get; set; }

        public long? CMACSUserId { get; set; }

        public string Name { get; set; }
    }
}
