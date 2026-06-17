using Services.Dtos;
using Services.Dtos.ApplicationDtos._Document;
using Services.ViewModels._DocumentViewModels;
using Services.ViewModels.Clients._DocumentViewModels;

namespace Services.Applications
{
    public interface IDocumentService
    {
        public Task<IResult> CreateAsync(CreateDocument createDocument, Guid submitUserId);
        public Task<IResult> UpdateAsync(UpdateDocument updateDocument, Guid submitUserId);
        public Task<IResult> DeleteAsync(Guid deleteDocumentId, Guid submitUserId);
        public Task<IResultData<DocumentList>> GetListDocument(DocumentListQuery query);
    }
}
