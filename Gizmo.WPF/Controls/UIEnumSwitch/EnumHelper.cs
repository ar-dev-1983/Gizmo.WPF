using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Gizmo.WPF
{
    public static class EnumHelper
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var memberInfos = enumVal.GetType().GetMember(enumVal.ToString());
            var attributes = memberInfos[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
