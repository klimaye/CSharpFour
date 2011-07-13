using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace CSharapFour
{
    public class DynamicDictionary : DynamicObject
    {
        readonly Dictionary<string,object> _dictionary
            = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var type = typeof (Dictionary<string, object>);
            try
            {
                result = type.InvokeMember(
                            name: binder.Name,
                            invokeAttr:
                            BindingFlags.InvokeMethod |
                            BindingFlags.Public |
                            BindingFlags.Instance,
                            binder: null, 
                            target:  _dictionary, 
                            args: args);

                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }
    }
}