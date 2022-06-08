using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kad.PMSDemo.MultiTenancy.HostDashboard.Dto;

namespace Kad.PMSDemo.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}