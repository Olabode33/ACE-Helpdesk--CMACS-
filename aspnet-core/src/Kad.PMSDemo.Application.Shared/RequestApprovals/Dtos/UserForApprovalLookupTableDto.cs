using Abp.Application.Services.Dto;

namespace Test.RequestApprovals.Dtos
{
    public class UserForApprovalLookupTableDto
    {
		public long Id { get; set; }

		public string DisplayName { get; set; }
    }
}