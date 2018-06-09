namespace Manage.Core.Pageing
{
    public class Pageination : BasePage
    {
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
