using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

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

		public static string SecondsToMinutes(int sec) {
			int mins = sec / 60;
			int hours = mins / 60;
			int secs = sec % 60;
			if (hours > 0) {
				return hours.ToString ("00") + "h";
			} else {
				return mins.ToString ("00") + ":" + secs.ToString ("00");
			}
		}
		public static void ClearChildren(Transform parent) {
			int childCount = parent.childCount;
			for (int i = 0; i < childCount; i++) {
				GameObject.Destroy(parent.GetChild (0).gameObject);
			}
		}

		public static string TimeToNow(DateTime adate) {
			DateTime now = DateTime.Now;
			TimeSpan delta = (adate - now);
			string s = "" + delta.TotalHours.ToString ("00");
			s += ":" + Math.Abs(delta.Minutes).ToString ("00");
			s += ":" + Math.Abs(delta.Seconds).ToString ("00");
			return s;
		}

		public static DateTime StringLongToDateTime(string s) {
			long l = (long)Convert.ToInt64 (s);
			DateTime start = new DateTime(1970, 1, 1, 0, 0, 0);
			DateTime resultdate= start.AddMilliseconds(l);
			return resultdate;
		}
	}

}
