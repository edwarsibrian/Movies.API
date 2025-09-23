using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.DTOs
{
    public class PaginationDTO
    {
        public int PageNumber { get; set; } = 1;
        
        private int recordsByPage = 10;
        
        private readonly int maxRecordsByPage = 50;

        public int RecordsByPage
        {
            get => recordsByPage;
            set => recordsByPage = (value > maxRecordsByPage) ? maxRecordsByPage : value;
        }
    }
}
