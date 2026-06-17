using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Models.Criterias;
using Services.Repositories;
using Services.Utils;
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
    }
}
