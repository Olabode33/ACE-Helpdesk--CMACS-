namespace Kad.PMSDemo.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string User = "User";
            public const string IFRS_Helpdesk = "IFRS Helpdesk";
            public const string ManagersPartners = "Managers & Partners";
            public const string RequestingTeam = "Requesting Team";
            public const string TechnicalTeam = "Technical Team";
            public const string FinalApprover = "Final Approver";
            public const string CMACsManager = "CMACs Manager";
        }
    }
}