using System.Collections.Generic;

namespace LFeBlog.Core.Entities
{
    public class PaginatedList<T>:List<T> where T:class
    {
        
        public int PageSize { get; set; }
        
        public int PageIndex { get; set; }

        private int _totalItemsCount;

        public int TotalItemsCount
        {
            get => _totalItemsCount;
            set => _totalItemsCount = value >=0 ? value :0;
        }


        public int PageCount => TotalItemsCount / PageSize + (TotalItemsCount % PageSize > 0 ? 1 : 0);

        public bool HasPrevious => PageIndex > 0;

        public bool HasNextPage => PageIndex < PageCount - 1;

        public PaginatedList(IEnumerable<T> collection, int totalItemsCount, int pageSize, int pageIndex) : base(collection)
        {
            TotalItemsCount = totalItemsCount;
            PageSize = pageSize;
            PageIndex = pageIndex;
            
            AddRange(collection);
        }
    }
    
    
    
}