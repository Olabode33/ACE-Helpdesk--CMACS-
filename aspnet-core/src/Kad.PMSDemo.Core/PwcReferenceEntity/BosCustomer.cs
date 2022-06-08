

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.PwcReferenceEntity
{
	[Table("BOS_Customers")]
    public class BosCustomer : Entity
    {
        public virtual string ClientCode { get; set; }
        public virtual string ClientName { get; set; }
        public virtual string UpdateDateTime { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string Country { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string Status { get; set; }

    }
}