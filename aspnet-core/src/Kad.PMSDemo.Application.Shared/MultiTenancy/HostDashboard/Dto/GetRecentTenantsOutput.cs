using System;
using System.Collections.Generic;

namespace Kad.PMSDemo.MultiTenancy.HostDashboard.Dto
{
    public class GetRecentTenantsOutput
    {
        public int RecentTenantsDayCount { get; set; }

        public int MaxRecentTenantsShownCount { get; set; }

        public DateTime TenantCreationStartDate { get; set; }

        public List<RecentTenant> RecentTenants { get; set; }
    }
}