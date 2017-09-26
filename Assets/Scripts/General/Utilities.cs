﻿using System;
using System.Text;
using System.Security.Cryptography;

namespace BidRoyale.Core
{
	public class Utilities  {

		public static Int64 GetInt64HashCode(string strText)
		{
			Int64 hashCode = 0;
			if (!string.IsNullOrEmpty(strText))
			{
				//Unicode Encode Covering all characterset
				byte[] byteContents = Encoding.Unicode.GetBytes(strText);
				SHA256 hash = new SHA256CryptoServiceProvider();
				byte[] hashText = hash.ComputeHash(byteContents);
				//32Byte hashText separate
				//hashCodeStart = 0~7  8Byte
				//hashCodeMedium = 8~23  8Byte
				//hashCodeEnd = 24~31  8Byte
				//and Fold
				Int64 hashCodeStart = BitConverter.ToInt64(hashText, 0);
				Int64 hashCodeMedium = BitConverter.ToInt64(hashText, 8);
				Int64 hashCodeEnd = BitConverter.ToInt64(hashText, 24);
				hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;
			}
			return (hashCode);
		}        
	}
}
