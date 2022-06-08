using Test;

using System;
using Abp.Application.Services.Dto;

namespace Test.TechTeams.Dtos
{
    public class TechTeamDto : EntityDto
    {
        public int TimeCharge { get; set; }

        public StaffCategory Role { get; set; }

        public int? RequestId { get; set; }

        public long? CMACSUserId { get; set; }

    }
}