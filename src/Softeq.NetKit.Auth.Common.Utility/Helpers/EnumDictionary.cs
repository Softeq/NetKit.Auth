// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.NetKit.Auth.Common.Exceptions;

namespace Softeq.NetKit.Auth.Common.Utility.Helpers
{
	public class EnumDictionary
	{
		public static Dictionary<int, string> Of<TEnum>()
		{
			return Enum.GetValues(typeof(TEnum)).Cast<ErrorCode>().ToDictionary(k => (int)k, v => v.ToString());
		}
	}
}
