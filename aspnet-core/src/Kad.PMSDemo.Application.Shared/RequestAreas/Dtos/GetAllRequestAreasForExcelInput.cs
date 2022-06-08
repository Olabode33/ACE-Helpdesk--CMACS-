using Abp.Application.Services.Dto;
using System;

namespace Test.RequestAreas.Dtos
{
    public class GetAllRequestAreasForExcelInput
    {
		public string Filter { get; set; }

		public string RequestAreaNameFilter { get; set; }



    }
}