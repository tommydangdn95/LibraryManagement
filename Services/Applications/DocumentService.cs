using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Models.Criterias;
using Services.Repositories;
using Services.Utils;
using Services.ViewModels._BranchViewModels;
using Services.ViewModels._DocumentViewModels;
using Services.ViewModels.Clients._DocumentViewModels;

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
                DocumentStatus = DocumentStatus.Available,
                PublishDate = createDocument.PublishDate,
                DocumentType = createDocument.DocumentTypeId.ToEnum<DocumentType>()!.Value,
                CreatedDate = DateTime.Now,
                CreatedBy = submitUserId
            };

            var result = await _documentRepository.CreateAsync(document);
            if (!result)
            {
                return Result.Failed($"Failed to create new document");
            }

            var documentBranch = new Models.DocumentBranch()
            {
                DocumentId = document.Id,
                BranchId = createDocument.BranchId,
                CreatedDate = DateTime.Now,
                CreatedBy = submitUserId
            };

            result = await _documentRepository.CreateDocumentBranch(documentBranch);
            if (!result)
            {
                return Result.Failed($"Failed to create new document branch");
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

            var result = await _documentRepository.UpdateAsync(document);
            if (!result)
            {
                return Result.Failed("Could not update document");
            }

            result = await _documentRepository.UpdateDocumentBranch(updateDocument.DocumenId, updateDocument.BranchId, submitUserId);
            if (!result)
            {
                return Result.Failed("Could not update document branch");
            }

            return Result.Failed("Update document successfully");
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

            result = await _documentRepository.DeleteDocumentBranch(deleteDocumentId, submitUserId);

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
        public async Task<IResultData<DocumentList>> GetListDocument(DocumentListQuery query)
        {
            var criteria = new GetDocumentItemCriteria()
            {
                BranchId = query.BranchId,
                SearchDocumentName = query.SearchDocumentName,
                DocumentType = query.DocumentType,
                DocumentStatus = query.DocumentStatus,
                BorrowStatus = query.BorrowStatus,
                StartDate = query.StartDate,
                EndDate = query.EndDate,
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
                }).ToList(),
                Paging = Paging.GetPaging(query.Page, query.RowsPerPage, pageResult.TotalCount)
            };


            return ResultData<DocumentList>.SuccessData("Get list item successfully", documentList);
        }

        #endregion
    }
}
