using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.UI;
using Kad.PMSDemo.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Test.ClientLists;

namespace Test.PwcReferenceEntity.Workers
{
    public class ETL_ReferenceTableWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public ETL_ReferenceTableWorker(AbpTimer timer,
            IWebHostEnvironment env)
        : base(timer)
        {
            _appConfiguration = env.GetAppConfiguration();

            Timer.Period = 24 * 60 * 60 * 1000; //5 seconds (good for tests, but normally will be more)
            //Timer.Period = 5 * 1000; //5 seconds (good for tests, but normally will be more)
        }

        // [UnitOfWork]
        protected override void DoWork()
        {
            

            try
            {
                //Get data
                DataTable dtClients = FetchClientDetails();
                DataTable dtStaffs = FetchStaffDetails();
                DataTable dtProjects = FetchProjectDetails();

                TruncateTables();
                ExecuteBulkCopy(dtClients, "BOS_Customers");
                ExecuteBulkCopy(dtStaffs, "BOS_Resources");
                ExecuteBulkCopy(dtProjects, "BOS_Projects");
                UpdateClientListTable();
            }

            catch (Exception e)
            {
                Logger.Debug("Error fetching data: " + e.Message);
            }
        }

        private void TruncateTables()
        {
            ExecuteQuery("Truncate table BOS_Customers");
            ExecuteQuery("Truncate table BOS_Resources");
            ExecuteQuery("Truncate table BOS_Projects");
        }

        private void UpdateClientListTable()
        {
            var queryString = @"insert into ClientLists (TenantId, ClientName, ClientAddress, FinancialYearEnd, SecRegistered)
                                Select 1, ClientName, Address1 + ', ' + Address2 + ', ' + Address3, 0, 0
                                from BOS_Customers
                                where trim(ClientName) COLLATE DATABASE_DEFAULT not in (Select trim(ifrs.ClientName) COLLATE DATABASE_DEFAULT  from ClientLists ifrs where ClientName is not null)";

            ExecuteQuery(queryString);
        }

        private DataTable FetchClientDetails()
        {
            var queryString = @"SELECT [ClientCode],[ClientName],[UpdateDateTime],[Address1],[Address2],[Address3],[Country],[City],[PostalCode],[Status] 
                                FROM  [dbo].[BOS_Customers]";

            return GetData(queryString);
        }

        private DataTable FetchStaffDetails()
        {
            var queryString = @"SELECT [ResourceID],[EmployeeID],[LegacyResourceID],[ResourceFirstName],[ResourceMiddleName],[ResourceLastName],
                                       [Designation],[CostCenterCode],[Costcenter],[EmailID],[CountryCode],[Country],[InternalBUDescription],[LoginID],[UpdateDateTime]
                                  FROM [dbo].[BOS_Resources]";

            return GetData(queryString);
        }

        private DataTable FetchProjectDetails()
        {
            var queryString = @"SELECT [JobCode],[JobDescription],[DepartmentCode],[DepartmentName],[UpdateDateTime],[ProductCode],[ProductName],[Country],[CountryCode] 
                                  FROM [dbo].[BOS_Projects]";

            return GetData(queryString);
        }


        [UnitOfWork]
        private void ExecuteBulkCopy(DataTable dt, string tablename)
        {
            var sqlConnection = _appConfiguration["ConnectionStrings:Default"];
            using (SqlConnection connection = new SqlConnection(sqlConnection))
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                // set the destination table name
                bulkCopy.DestinationTableName = tablename;
                bulkCopy.BatchSize = 10000;

                foreach (DataColumn dc in dt.Columns)
                {
                    bulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }

                connection.Open();

                // write the data in the "dataTable"
                bulkCopy.WriteToServer(dt);
                connection.Close();
            }
            // reset
            dt.Clear();
        }

        [UnitOfWork]
        public DataTable GetData(string qry)
        {
            var dt = new DataTable();
            var serverName = _appConfiguration["PwCReference:Server"];
            var dbName = _appConfiguration["PwCReference:Database"];
            var username = _appConfiguration["PwCReference:UserId"];
            var password = _appConfiguration["PwCReference:Password"];

            try
            {
                var con = new SqlConnection("Data Source=" + serverName + ";Initial Catalog=" + dbName + ";User ID=" + username + ";Password=" + password);
                var da = new SqlDataAdapter(qry, con);

                con.Open();
                da.Fill(dt);
                con.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

            return dt;
        }

        [UnitOfWork]
        public void ExecuteQuery(string qry)
        {
            var sqlConnection = _appConfiguration["ConnectionStrings:Default"];
            try
            {

                var con = new SqlConnection(sqlConnection);
                var com = new SqlCommand(qry, con);

                con.Open();
                var i = com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }

        }
    }
}
