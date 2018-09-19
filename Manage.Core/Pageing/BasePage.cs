namespace Manage.Core.Pageing
{
    public class BasePage
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 0;

        /// <summary>
        /// 页码数量
        /// </summary>
        public int PageCount { get; set; } = 0;

        /// <summary>
        /// 总记录数量
        /// </summary>
        public int TotalRecord { get; set; }

        /// <summary>
        /// 用时
        /// </summary>
        public int UsedTime { get; set; }

        /// <summary>
        /// 要显示的表或多个表的连接
        /// </summary>
        public string StrTable { get; set; }

        /// <summary>
        /// 要查询的字段
        /// </summary>
        public string StrField { get; set; }

        /// <summary>
        /// 查询条件,不需where
        /// </summary>
        public string StrWhere { get; set; }

        /// <summary>
        /// 用于排序的主键
        /// </summary>
        public string StrSortKey { get; set; }

        /// <summary>
        /// 用于排序，如：id desc (多个id desc,dt asc)
        /// </summary>
        public string StrSortField { get; set; }

        public string StrGroupField { get; set; }

        /// <summary>
        /// 排序,0-顺序,1-倒序
        /// </summary>
        public bool StrOrderBy { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int RecordCount { get; set; }
    }
}
