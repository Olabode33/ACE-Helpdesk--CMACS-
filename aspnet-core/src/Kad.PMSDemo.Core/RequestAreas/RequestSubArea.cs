using Test.RequestAreas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.RequestAreas
{
	[Table("RequestSubAreas")]
    public class RequestSubArea : FullAuditedEntity 
    {

		public virtual string Name { get; set; }
		
		public virtual string Description { get; set; }
		

		public virtual int RequestAreaId { get; set; }
		
        [ForeignKey("RequestAreaId")]
		public RequestArea RequestAreaFk { get; set; }
		
    }
}