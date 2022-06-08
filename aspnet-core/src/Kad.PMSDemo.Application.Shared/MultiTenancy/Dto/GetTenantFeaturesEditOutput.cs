using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Editions.Dto;

namespace Kad.PMSDemo.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}