using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.ReasourceParameters
{
    public class AuthorsResourceParameters
    {
        const int maxPageSize = 20;
        public string MainCategory { get; set; }
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        private int _PageSize { get; set; } = 10;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
