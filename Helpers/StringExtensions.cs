using System;
namespace CarSearch
{
	public static class StringExtensions
	{
		public static string Base64Encode(this string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

	}
}

