using System;

namespace Iris.Crosscutting.Common.Data
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public long TotalRecords { get; set; }

        public int FirstRecordOnPage
        {
            get { return (CurrentPage - 1) * PageSize + 1; }
        }

        public long LastRecordOnPage
        {
            get { return Math.Min(CurrentPage * PageSize, TotalRecords); }
        }
    }
}