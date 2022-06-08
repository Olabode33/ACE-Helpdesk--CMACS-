using Test;

using System;
using Abp.Application.Services.Dto;

namespace Test.AttachedDocs.Dtos
{
    public class AttachedDocDto : EntityDto
    {
        public string FileName { get; set; }

        public StaffEntityType DocOwnerTypeId { get; set; }

        public int? RequestId { get; set; }

        public long? DocOwnerId { get; set; }

        public Guid? DocumentId { get; set; }

        public AttachmentTypes AttachmentType { get; set; }
        public string FileFormat { get; set; }
    }
}