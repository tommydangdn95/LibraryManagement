using Microsoft.Data.SqlClient;
using Services.Dtos.ApplicationDtos._Document;
using Services.Enums;
using Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Mapper
{
    public static class DocumentItemMapper
    {
        public static DocumentItem Mapper(SqlDataReader reader)
        {
            var documentItem = new DocumentItem();
            documentItem.DocumentId = reader.GetValue<Guid>("DocumentId");
            documentItem.BranchId = reader.GetValue<Guid>("BranchId");
            documentItem.BranchName = reader.GetValue<string>("BranchName");
            documentItem.DocumentTitle = reader.GetValue<string>("DocumentTitle");
            documentItem.DocumentType = reader.GetValue<int?>("DocumentType").HasValue ? (DocumentType)reader.GetValue<int?>("DocumentType").Value : DocumentType.Book;
            documentItem.DocumentStatus = reader.GetValue<int?>("DocumentStatus").HasValue ? (DocumentStatus)reader.GetValue<int?>("DocumentStatus").Value : DocumentStatus.Good;
            documentItem.DocumentDescription = reader.GetValue<string>("DocumentDescription");
            documentItem.CoverImageUrl = reader.GetValue<string>("CoverImageUrl");
            documentItem.PublishDate = reader.GetValue<DateTime>("PublishDate");
            documentItem.BorrowStatus = reader.GetValue<int?>("BorrowStatus").HasValue ? (BorrowStatus)reader.GetValue<int?>("BorrowStatus").Value : null;
            documentItem.BorrowDate = reader.GetValue<DateTime?>("BorrowDate");
            documentItem.ReturnDate = reader.GetValue<DateTime?>("ReturnDate");
            return documentItem;
        }
    }
}
