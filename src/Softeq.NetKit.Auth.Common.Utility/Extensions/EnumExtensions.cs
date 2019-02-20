// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Softeq.NetKit.Auth.Common.Utility.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnumValue<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

		public static T GetAttribute<T>(this Enum value) where T : Attribute
		{
			var type = value.GetType();
			var memberInfo = type.GetMember(value.ToString());
			var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
			return (T)attributes.FirstOrDefault();
		}

		public static string ToDisplayName(this Enum value)
	    {
		    var attribute = value.GetAttribute<DisplayAttribute>();
		    return attribute == null ? value.ToString() : attribute.Name;
	    }
	}
}
