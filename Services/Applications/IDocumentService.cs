using Services.Dtos;
using Services.ViewModels._DocumentViewModels;

namespace Services.Applications
{
    public interface IDocumentService
    {
        public Task<IResult> CreateAsync(CreateDocument createDocument, Guid submitUserId);
    }
}
