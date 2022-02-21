using System.Collections.Generic;

namespace DAL.Models
{
    public class PagingModel<T>
    {
        public List<T> List { get; set; }
        public int Page { get; set; }
        public int Total { get; set; }
        public int LastPage { get; set; }
    }
}
