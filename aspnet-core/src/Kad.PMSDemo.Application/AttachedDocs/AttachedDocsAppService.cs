using Test.Requests;
using Kad.PMSDemo.Authorization.Users;
using Kad.PMSDemo.Storage;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.AttachedDocs.Exporting;
using Test.AttachedDocs.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.AttachedDocs
{
    [AbpAuthorize]
    public class AttachedDocsAppService : PMSDemoAppServiceBase, IAttachedDocsAppService
    {
        private readonly IRepository<AttachedDoc> _attachedDocRepository;
        private readonly IAttachedDocsExcelExporter _attachedDocsExcelExporter;
        private readonly IRepository<Request, int> _requestRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;


        public AttachedDocsAppService(IRepository<AttachedDoc> attachedDocRepository, IAttachedDocsExcelExporter attachedDocsExcelExporter, IRepository<Request, int> requestRepository, IRepository<User, long> userRepository, IRepository<BinaryObject, Guid> binaryObjectRepository)
        {
            _attachedDocRepository = attachedDocRepository;
            _attachedDocsExcelExporter = attachedDocsExcelExporter;
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _binaryObjectRepository = binaryObjectRepository;

        }

        public async Task<PagedResultDto<GetAttachedDocForView>> GetAll(GetAllAttachedDocsInput input)
        {
            var docOwnerTypeIdFilter = (StaffEntityType)input.DocOwnerTypeIdFilter;

            var filtered = _attachedDocRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName.ToLower() == input.FileNameFilter.ToLower().Trim())
                        .WhereIf(input.DocOwnerTypeIdFilter > -1, e => e.DocOwnerTypeId == docOwnerTypeIdFilter);

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.DocOwnerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _binaryObjectRepository.GetAll() on o.DocumentId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetAttachedDocForView()
                         {
                             AttachedDoc = ObjectMapper.Map<AttachedDocDto>(o),
                             RequestRequestNo = s1 == null ? "" : s1.RequestNo.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString(),
                             BinaryObjectTenantId = s3 == null ? "" : s3.TenantId.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestRequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjectTenantIdFilter), e => e.BinaryObjectTenantId.ToLower() == input.BinaryObjectTenantIdFilter.ToLower().Trim());

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetAttachedDocForView>(
                totalCount,
                await query.ToListAsync()
            );
        }


        public async Task<GetAttachedDocForEditOutput> GetAttachedDocForEdit(EntityDto input)
        {
            var attachedDoc = await _attachedDocRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttachedDocForEditOutput { AttachedDoc = ObjectMapper.Map<CreateOrEditAttachedDocDto>(attachedDoc) };

            if (output.AttachedDoc.RequestId != null)
            {
                var request = await _requestRepository.FirstOrDefaultAsync((int)output.AttachedDoc.RequestId);
                output.RequestRequestNo = request.RequestNo.ToString();
            }

            if (output.AttachedDoc.DocOwnerId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.AttachedDoc.DocOwnerId);
                output.UserName = user.Name.ToString();
            }

            if (output.AttachedDoc.DocumentId != null)
            {
                var binaryObject = await _binaryObjectRepository.FirstOrDefaultAsync((Guid)output.AttachedDoc.DocumentId);
                output.BinaryObjectTenantId = binaryObject.TenantId.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttachedDocDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        public async Task CreateOrEditMultipleDoc(List<CreateOrEditAttachedDocDto> input)
        {
            foreach (var _input in input)
            {
                _input.AttachmentType = AttachmentTypes.Attachment;
                await CreateOrEdit(_input);
            }

        }

        public async Task CreateOrEditMultipleReviewedFSDoc(List<CreateOrEditAttachedDocDto> input)
        {
            foreach (var _input in input)
            {
                _input.AttachmentType = AttachmentTypes.ReviewedFS;
                await CreateOrEdit(_input);
            }

        }


        private async Task Create(CreateOrEditAttachedDocDto input)
        {
            var attachedDoc = ObjectMapper.Map<AttachedDoc>(input);


            if (AbpSession.TenantId != null)
            {
                attachedDoc.TenantId = (int?)AbpSession.TenantId;
            }


            await _attachedDocRepository.InsertAsync(attachedDoc);
        }

        private async Task Update(CreateOrEditAttachedDocDto input)
        {
            var attachedDoc = await _attachedDocRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, attachedDoc);
        }


        public async Task Delete(EntityDto input)
        {
            await _attachedDocRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAttachedDocsToExcel(GetAllAttachedDocsForExcelInput input)
        {
            var docOwnerTypeIdFilter = (StaffEntityType)input.DocOwnerTypeIdFilter;

            var filteredAttachedDocs = _attachedDocRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.FileName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FileNameFilter), e => e.FileName.ToLower() == input.FileNameFilter.ToLower().Trim())
                        .WhereIf(input.DocOwnerTypeIdFilter > -1, e => e.DocOwnerTypeId == docOwnerTypeIdFilter);


            var query = (from o in filteredAttachedDocs
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.DocOwnerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _binaryObjectRepository.GetAll() on o.DocumentId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetAttachedDocForView()
                         {
                             AttachedDoc = ObjectMapper.Map<AttachedDocDto>(o),
                             RequestRequestNo = s1 == null ? "" : s1.RequestNo.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString(),
                             BinaryObjectTenantId = s3 == null ? "" : s3.TenantId.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestRequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BinaryObjectTenantIdFilter), e => e.BinaryObjectTenantId.ToLower() == input.BinaryObjectTenantIdFilter.ToLower().Trim());


            var attachedDocListDtos = await query.ToListAsync();

            return _attachedDocsExcelExporter.ExportToFile(attachedDocListDtos);
        }


        public async Task<PagedResultDto<RequestIdLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _requestRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.RequestNo.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RequestIdLookupTableDto>();
            foreach (var request in requestList)
            {
                lookupTableDtoList.Add(new RequestIdLookupTableDto
                {
                    Id = request.Id,
                    DisplayName = request.RequestNo.ToString()
                });
            }

            return new PagedResultDto<RequestIdLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<PagedResultDto<UserIdLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserIdLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new UserIdLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name.ToString()
                });
            }

            return new PagedResultDto<UserIdLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<PagedResultDto<BinaryObjectLookupTableDto>> GetAllBinaryObjectForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _binaryObjectRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TenantId.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var binaryObjectList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<BinaryObjectLookupTableDto>();
            foreach (var binaryObject in binaryObjectList)
            {
                lookupTableDtoList.Add(new BinaryObjectLookupTableDto
                {
                    Id = binaryObject.Id.ToString(),
                    DisplayName = binaryObject.TenantId.ToString()
                });
            }

            return new PagedResultDto<BinaryObjectLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}