using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Models;
using Services.Models.Criterias;
using Services.Repositories;
using Services.Utils;
using Services.ViewModels._BranchViewModels;
using Services.ViewModels._DocumentViewModels;
using Services.ViewModels.Clients._DocumentViewModels;
using System.Net.NetworkInformation;

namespace Services.Applications
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        public DocumentService(IDocumentRepository documentRepository)
        {
            this._documentRepository = documentRepository;
        }

        public async Task<IResult> CreateAsync(CreateDocument createDocument, Guid submitUserId)
        {
            var document = new Models.Document()
            {
                Title = createDocument.Title,
                Description = createDocument.Description,
                DocumentStatus = DocumentStatus.Good,
                PublishDate = createDocument.PublishDate,
                DocumentType = createDocument.DocumentTypeId.ToEnum<DocumentType>()!.Value,
                CreatedDate = DateTime.Now,
                CreatedBy = submitUserId,
                BranchId = createDocument.BranchId,
            };

            var result = await _documentRepository.CreateAsync(document);
            if (!result)
            {
                return Result.Failed($"Failed to create new document");
            }

            return Result.Success($"Create new document branch successfully");
        }


        public async Task<IResult> UpdateAsync(UpdateDocument updateDocument, Guid submitUserId)
        {
            var document = await _documentRepository.GetByIdAsync(updateDocument.DocumenId);
            if (document == null)
            {
                return Result.Failed("Document not found");
            }

            document.Title = updateDocument.Title;
            document.Description = updateDocument.Description;
            document.CoverImageUrl = updateDocument.CoverImageUrl;
            document.PublishDate = updateDocument.PublishDate;
            document.UpdatedDate = DateTime.Now;
            document.UpdatedBy = submitUserId;
            document.BranchId = updateDocument.BranchId;

            if (updateDocument.DocumentStatusId.HasValue)
            {
                document.DocumentStatus = updateDocument.DocumentStatusId.Value.ToEnum<DocumentStatus>().Value;
            }

            document.DocumentType = updateDocument.DocumentTypeId.ToEnum<DocumentType>().Value;
            var result = await _documentRepository.UpdateAsync(document);
            if (!result)
            {
                return Result.Failed("Could not update document");
            }

            return Result.Success("Update document successfully");
        }

        public async Task<IResult> DeleteAsync(Guid deleteDocumentId, Guid submitUserId)
        {
            var deleteDocument = await _documentRepository.GetByIdAsync(deleteDocumentId);
            if (deleteDocument == null)
            {
                return Result.Failed("Document not found");
            }

            var result = await _documentRepository.DeleteAsync(deleteDocumentId, submitUserId);
            if (!result)
            {
                return Result.Failed($"Failed to delete document");
            }

            return Result.Success($"Delete document successfully");
        }

        public async Task<IResultData<DocumentItemAdmin>> GetByIdAsync(Guid documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                return ResultData<DocumentItemAdmin>.Failed("Not found document");
            }

            var item = new DocumentItemAdmin()
            {
                Id = document.Id,
                BranchId = document.BranchId,
                Title = document.Title,
                DocumentStatus = document.DocumentStatus,
                DocumentType = document.DocumentType,
                Description = document.Description,
                PublishDate = document.PublishDate,
                CoverImageUrl = document.CoverImageUrl,
            };

            return ResultData<DocumentItemAdmin>.SuccessData("Get document successfully", item);
        }

        public async Task<IResultData<DocumentListAdmin>> GetListDocumentAdmin(DocumentListAdminQuery query)
        {
            var criteria = new GetDocumentListCriteria()
            {
                BranchId = query.BranchId,
                DocumentTypeId = query.DocumentTypeId,
                SearchName = query.SearchName,
                Page = query.Page,
                RowsPerPage = query.RowsPerPage
            };

            var pageResult = await _documentRepository.GetAllAsync(criteria);
            var documentList = new DocumentListAdmin()
            {
                Items = pageResult.Items.Select(x => new DocumentItemAdmin()
                {
                    Id = x.DocumentId,
                    Branch = x.BranchName,
                    Title = x.DocumentTitle,
                    BranchId = x.BranchId,
                    Description = x.DocumentDescription,
                    DocumentStatus = x.DocumentStatus,
                    CoverImageUrl = x.CoverImageUrl,
                    DocumentType = x.DocumentType,
                    PublishDate = x.PublishDate,
                    
                }).ToList(),
                Paging = Paging.GetPaging(query.Page, query.RowsPerPage, pageResult.TotalCount)
            };

            return ResultData<DocumentListAdmin>.SuccessData("Get list item successfully", documentList);

        }



        #region Client
        public async Task<IResultData<DocumentList>> GetListDocument(GetDocumentListQuery query)
        {
            var parseStartDate = !string.IsNullOrEmpty(query.StartDate) ? DateTime.Parse(query.StartDate) : (DateTime?)null;
            var parseEndDate = !string.IsNullOrEmpty(query.EndDate) ? DateTime.Parse(query.EndDate) : (DateTime?)null;
            var branchId = !string.IsNullOrEmpty(query.BranchId) ? Guid.Parse(query.BranchId) : (Guid?)null;

            var criteria = new GetDocumentItemCriteria()
            {
                BranchId = branchId,
                SearchDocumentName = query.SearchDocumentName,
                DocumentTypes = query.DocumentTypes.Select(x => Enum.Parse<DocumentType>(x)).ToList(),
                StartDate = parseStartDate,
                EndDate = parseEndDate,
                Page = query.Page,
                RowsPerPage = query.RowsPerPage
            };

            var pageResult = await _documentRepository.GetListDocumentItem(criteria);
            var documentList = new DocumentList()
            {
                Items = pageResult.Items.Select(x => new DocumentViewItem()
                {
                    DocumentId = x.DocumentId,
                    Branch = x.BranchName,
                    DocumentTitle = x.DocumentTitle,
                    DocumentDescription = x.DocumentDescription,
                    DocumentStatus = x.DocumentStatus,
                    CoverImageUrl = x.CoverImageUrl,
                    DocumentType = x.DocumentType.ToString(),
                    BorrowStatus = x.BorrowStatus,
                    BorrowDate = x.BorrowDate,
                    PublishDate = x.PublishDate,
                    BranchId = x.BranchId,
                    ReturnDate = x.ReturnDate
                }).ToList(),
                Paging = Paging.GetPaging(query.Page, query.RowsPerPage, pageResult.TotalCount)
            };


            return ResultData<DocumentList>.SuccessData("Get list item successfully", documentList);
        }

        public async Task<IResultData<DocumentViewItem>> GetDocumentViewItem(Guid documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                return ResultData<DocumentViewItem>.Failed("Not found document");
            }

            var item = new DocumentViewItem()
            {
                BranchId = document.BranchId,
                DocumentId = document.Id,
                DocumentTitle = document.Title,
                DocumentStatus = document.DocumentStatus,
                DocumentType = document.DocumentType.ToString(),
                DocumentDescription = document.Description,
                PublishDate = document.PublishDate,
                CoverImageUrl = document.CoverImageUrl,
            };

            return ResultData<DocumentViewItem>.SuccessData("Get document successfully", item);
        }

        #endregion
    }
}
