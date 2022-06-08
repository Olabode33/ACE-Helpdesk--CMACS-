using Test;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Test.AttachedDocs.Dtos
{
    public class CreateOrEditAttachedDocDto : EntityDto<int?>
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