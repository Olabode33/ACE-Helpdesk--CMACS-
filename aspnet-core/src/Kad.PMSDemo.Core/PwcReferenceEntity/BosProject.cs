

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.PwcReferenceEntity
{
	[Table("BOS_Projects")]
    public class BosProject : Entity
    {
        public virtual string JobCode { get; set; }
        public virtual string JobDescription { get; set; }
        public virtual string DepartmentCode { get; set; }
        public virtual string DepartmentName { get; set; }
        public virtual string UpdateDateTime { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string ProductName { get; set; }
        public virtual string Country { get; set; }
        public virtual string CountryCode { get; set; }

    }
}