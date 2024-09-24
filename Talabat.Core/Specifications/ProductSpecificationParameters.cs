using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications
{
    public class ProductSpecificationParameters
    {
        private const int MAX_PAGE_SIZE = 10;

        public string ? Sort { get; set; }
        public int ? BrandId { get; set; }
        public int ? TypeId { get; set; }
        public int ? PageIndex { get; set; } = 1;

        private int ? pageSize = 5;

        public int? PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value; }
        }

        private string ? search;

        public string ? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }


    }
}
