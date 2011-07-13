using System;
using System.Web.Script.Serialization;

namespace CSharapFour
{
    public static class ObjectExtentions
    {
        /// <summary>
        /// Try to convert the current object into a json object
        /// </summary>
        /// <param name="foo">this</param>
        /// <remarks>If the object cannot be converted then an exception will occur</remarks>
        /// <returns>string that is json</returns>
        public static string ToJson(this object foo)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(foo);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}