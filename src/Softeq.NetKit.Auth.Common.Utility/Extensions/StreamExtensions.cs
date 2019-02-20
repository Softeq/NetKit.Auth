// Developed by Softeq Development Corporation
// http://www.softeq.com
using System;
using System.IO;
using System.Text;

namespace Softeq.NetKit.Auth.Common.Utility.Extensions
{
	public static class StreamExtensions
	{
		internal static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);

		public static BinaryReader CreateBinaryFileReader(this Stream stream)
		{
			return new BinaryReader(stream, DefaultEncoding, true);
		}

		public static DateTimeOffset ReadTokenCreationDate(this BinaryReader reader)
		{
			return new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
		}
	}
}