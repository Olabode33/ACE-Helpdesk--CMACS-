

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.PwcReferenceEntity
{
	[Table("BOS_Resources")]
    public class BosResource : Entity
    {
        public virtual string ResourceID { get; set; }
        public virtual string EmployeeID { get; set; }
        public virtual string LegacyResourceID { get; set; }
        public virtual string ResourceFirstName { get; set; }
        public virtual string ResourceMiddleName { get; set; }
        public virtual string ResourceLastName { get; set; }
        public virtual string Designation { get; set; }
        public virtual string CostCenterCode { get; set; }
        public virtual string Costcenter { get; set; }
        public virtual string EmailID { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual string Country { get; set; }
        public virtual string InternalBUDescription { get; set; }
        public virtual string LoginID { get; set; }
        public virtual string UpdateDateTime { get; set; }

    }
}