namespace Test.Requests.Dtos
{
    public class GetRequestForView
    {
		public RequestDto Request { get; set; }

		public string RequestAreaRequestAreaName { get; set;}

		public string RequestDomainDomainName { get; set;}

		public string RequestorName { get; set;}

		public string PartnerName { get; set;}

		public string ManagerName { get; set;}

		public string ClientListClientName { get; set;}

		public string AssigneeName { get; set;}

        public string NextAction { get; set; }
        public double PercentageComplete { get; set; }

    }
}