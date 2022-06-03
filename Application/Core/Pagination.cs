using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Core
{
	public class PagedList<T> : List<T>
	{
		public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
		{
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			PageSize = pageSize;
			TotalCount = count;
			AddRange(items);
		}

		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }

		public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber,
			int pageSize)
		{
			var count = await source.CountAsync();
			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}

	public class PaginationHeader
	{
		public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
		{
			CurrentPage = currentPage;
			ItemsPerPage = itemsPerPage;
			TotalItems = totalItems;
			TotalPages = totalPages;
		}

		public int CurrentPage { get; set; }
		public int ItemsPerPage { get; set; }
		public int TotalItems { get; set; }
		public int TotalPages { get; set; }
	}

	public class PaginationParams
	{
		private const int MaxPageSize = 50;
		[FromQuery(Name = "page")]
		public int PageNumber { get; set; } = 1;
		private int _pageSize = 10;

		[FromQuery(Name = "page-size")]
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

	}

	public static class Pagination
	{
		public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
		{
			var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

			var options = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};

			response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
			response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
		}
	}
}