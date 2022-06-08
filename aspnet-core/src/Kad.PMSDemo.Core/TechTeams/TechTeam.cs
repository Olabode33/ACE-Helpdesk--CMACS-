using Test;
using Test.Requests;
using Kad.PMSDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.TechTeams
{
    [Table("TechTeams")]
    public class TechTeam : FullAuditedEntity, IMayHaveTenant
    {
        public int? TenantId { get; set; }


        public virtual int TimeCharge { get; set; }

        public virtual StaffCategory Role { get; set; }


        public virtual int? RequestId { get; set; }
        public Request Request { get; set; }

        public virtual long? CMACSUserId { get; set; }
        public User CMACSUser { get; set; }

    }
}