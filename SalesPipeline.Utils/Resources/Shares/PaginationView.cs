using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils.Resources.Shares
{
	public class PaginationView<T>
	{
		public T Items { get; set; }
		public Pager Pager { get; set; }
	}

	public class Pager
	{
		public Pager(int totalItems, int? page, int pageSize = 10, object? condition = null)
		{
			// calculate total, start and end pages
			var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
			var currentPage = page != null ? (int)page : 1;
			var startPage = currentPage - 5;
			var endPage = currentPage + 4;
			if (startPage <= 0)
			{
				endPage -= (startPage - 1);
				startPage = 1;
			}
			if (endPage > totalPages)
			{
				endPage = totalPages;
				if (endPage > 10)
				{
					startPage = endPage - 9;
				}
			}

			Condition = condition;
			TotalItems = totalItems;
			CurrentPage = currentPage;
			PageSize = pageSize;
			TotalPages = totalPages;
			StartPage = startPage;
			EndPage = endPage;
		}

		public object? Condition { get; set; }
		public string UrlAction { get; set; }
		public int TotalItems { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public int StartPage { get; set; }
		public int EndPage { get; set; }
	}
}
