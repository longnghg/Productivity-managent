using System;
using System.Collections.Generic;
using System.Text;

namespace TaskService.Application.Common.Models
{
    public class Pagination
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 20;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string SortBy { get; set; } = "CreatedAt";
        public bool SortDescending { get; set; } = true; // Mới nhất lên đầu
    }

    public class ressulttests<T>
    {
        public T Data{ get; set; }
    }

}
