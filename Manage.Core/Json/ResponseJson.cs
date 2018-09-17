using Manage.Core.Data;
using Manage.Core.Json;
using Manage.Core.Utility;

namespace Manage.Core.Json
{
    /// <summary>
    /// 输出JSON
    /// </summary>
    public class ResponseJson
    {
        public static string Success()
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_OK, SuperConstants.AJAX_RETURN_STATE_OK_MESSAGE);
            return JsonUtil.SerializerObject(result);
        }

        public static string Success(string message)
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_OK, message);
            return JsonUtil.SerializerObject(result);
        }

        public static string Success(object data, string message)
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_OK, message, data);
            return JsonUtil.SerializerObject(result);
        }

        public static string Success(object data)
        {
            return JsonUtil.SerializerObject(data);
        }

        public static string Error()
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_ERROR, SuperConstants.AJAX_RETURN_STATE_ERROR_MESSAGE);
            return JsonUtil.SerializerObject(result);
        }

        public static string Error(string message)
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_ERROR, message);
            return JsonUtil.SerializerObject(result);
        }

        public static string Error(int state, string message)
        {
            ReturnResult result = new ReturnResult(state, message);
            return JsonUtil.SerializerObject(result);
        }

        public static string UnLogin(string message = "未登录")
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_LOGIN, message);
            return JsonUtil.SerializerObject(result);
        }

        public static string UnPower()
        {
            ReturnResult result = new ReturnResult(SuperConstants.AJAX_RETURN_STATE_AUTH, "没有权限");
            return JsonUtil.SerializerObject(result);
        }
    }
}
