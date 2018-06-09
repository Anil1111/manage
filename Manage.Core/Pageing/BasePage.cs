namespace Manage.Core.Pageing
{
    public class BasePage
    {
        public int PageIndex { get; set; } = 0;
        public int page { get; set; }
        public int rows { get; set; }
        public int PageSize { get; set; } = 0;
        public int PageCount { get; set; } = 0;
        public int TotalRecord { get; set; }
        public int UsedTime { get; set; }
    }
}
