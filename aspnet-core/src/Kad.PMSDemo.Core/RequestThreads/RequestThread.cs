
using Test.Requests;
using Kad.PMSDemo.Authorization.Users;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Test.RequestThreads
{
	[Table("RequestThreads")]
    public class RequestThread : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		public virtual string Comment { get; set; }
		
		public virtual DateTime CommentDate { get; set; }
		

		public virtual int? RequestId { get; set; }
		public Request Request { get; set; }
		
		public virtual long? CommentById { get; set; }
		public User CommentBy { get; set; }
		
    }
}