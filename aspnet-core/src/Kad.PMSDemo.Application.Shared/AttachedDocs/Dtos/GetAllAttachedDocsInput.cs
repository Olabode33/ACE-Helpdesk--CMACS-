using Abp.Application.Services.Dto;
using System;

namespace Test.AttachedDocs.Dtos
{
    public class GetAllAttachedDocsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string FileNameFilter { get; set; }

		public int DocOwnerTypeIdFilter { get; set; }


		 public string RequestRequestNoFilter { get; set; }

		 		 public string UserNameFilter { get; set; }

		 		 public string BinaryObjectTenantIdFilter { get; set; }

		 
    }
}