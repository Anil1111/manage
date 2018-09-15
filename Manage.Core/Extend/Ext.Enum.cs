using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manage.Core.Extend
{
    /// <summary>
    /// 枚举
    /// </summary>
    public static partial class Ext
    {
        public static string GetRemark(Enum value)
        {
            Type type = value.GetType();
            FieldInfo field = type.GetField(value.ToString());
            if (field.IsDefined(typeof(RemarkAttribute), true))
            {
                RemarkAttribute remarkAttribute = (RemarkAttribute)field.GetCustomAttribute(typeof(RemarkAttribute), true);
                return remarkAttribute.GetRemark();
            }
            else
            {
                return value.ToString();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public class RemarkAttribute : Attribute
    {
        private string _Remark = null;
        public RemarkAttribute(string remark)
        {
            this._Remark = remark;
        }

        public string GetRemark()
        {
            return this._Remark;
        }
    }
}
