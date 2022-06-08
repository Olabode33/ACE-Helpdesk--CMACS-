using Test;

using Test.Requests;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Kad.PMSDemo.Authorization.Users;
using Kad.PMSDemo.Storage;

namespace Test.AttachedDocs
{
	[Table("AttachedDocs")]
    public class AttachedDoc : Entity , IMayHaveTenant
    {
		public int? TenantId { get; set; }


		public virtual string FileName { get; set; }
		
		public virtual StaffEntityType DocOwnerTypeId { get; set; }
		

		public virtual int? RequestId { get; set; }
		public Request Request { get; set; }
		
		public virtual long? DocOwnerId { get; set; }
		public User DocOwner { get; set; }
		
		public virtual Guid? DocumentId { get; set; }
		public BinaryObject Document { get; set; }
		
        public virtual AttachmentTypes AttachmentType { get; set; }
        public virtual string FileFormat { get; set; }
    }
}