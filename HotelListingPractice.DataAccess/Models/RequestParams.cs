using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingPractice.DataAccess.Models
{
    // This model contains details that control how many records are retrieved from the database for a request
    public class RequestParams
    {
        // The maximum number of records we will allow per request/page
        const int maxPageSize = 50;

        // The default page size
        private int _pageSize = 10;

        // The default page number
        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                // If the user requests a page size bigger than the max page size, send the max page size
                if (value > maxPageSize)
                {
                    _pageSize = maxPageSize;
                }
                // Else, set the max page size to the users value
                else
                {
                    _pageSize = value;
                }
            }
        }
    }
}
