using Test;

using System;
using Abp.Application.Services.Dto;

namespace Test.RequestDocs.Dtos
{
    public class RequestDocDto : EntityDto
    {
        public string DocumentName { get; set; }

        public string DocumentLocation { get; set; }

        public StaffEntityType PreparerTypeId { get; set; }

        public Guid DocumentGUID { get; set; }


        public int? RequestId { get; set; }

        public long? PreparerId { get; set; }


    }
}