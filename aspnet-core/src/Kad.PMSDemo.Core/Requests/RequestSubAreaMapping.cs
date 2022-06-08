using Test.Requests;
using Test.RequestAreas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.Requests
{
	[Table("RequestSubAreaMappings")]
    public class RequestSubAreaMapping : Entity 
    {


		public virtual int RequestId { get; set; }
		
        [ForeignKey("RequestId")]
		public Request RequestFk { get; set; }
		
		public virtual int RequestSubAreaId { get; set; }
		
        [ForeignKey("RequestSubAreaId")]
		public RequestSubArea RequestSubAreaFk { get; set; }
		
    }
}