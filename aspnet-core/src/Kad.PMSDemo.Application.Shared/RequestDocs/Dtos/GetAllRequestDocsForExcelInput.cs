using Abp.Application.Services.Dto;
using System;

namespace Test.RequestDocs.Dtos
{
    public class GetAllRequestDocsForExcelInput
    {
		public string Filter { get; set; }

		public string DocumentNameFilter { get; set; }

		public string DocumentLocationFilter { get; set; }

		public int PreparerTypeIdFilter { get; set; }

		public Guid DocumentGUIDFilter { get; set; }


		 public string RequestLocalChargeCodeFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 
    }
}