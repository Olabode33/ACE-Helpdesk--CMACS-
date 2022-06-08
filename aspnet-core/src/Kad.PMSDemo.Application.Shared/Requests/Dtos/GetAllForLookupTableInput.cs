using Abp.Application.Services.Dto;

namespace Test.Requests.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }
    }

    public class GetUsersForRoleForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Role { get; set; }
        public string Filter { get; set; }
    }
}