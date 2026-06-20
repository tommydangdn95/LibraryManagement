using Services.Dtos;
using Services.ViewModels.Clients._DocumentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ViewModels.Clients._BorrowViewModels
{
    public class DocumentBorrowListItem
    {
        public List<DocumentBorrowViewItem> Items { get; set; }
        public Paging Paging { get; set; }
        public DocumentBorrowListItem()
        {
            this.Items = new List<DocumentBorrowViewItem>();
            this.Paging = Paging.DefaultPaging();
        }
    }
}
