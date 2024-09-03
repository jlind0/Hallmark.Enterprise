using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Utilities
{
	public static class HashCodeProvider
	{
		public static int BuildHashCode(params object[] fields)
		{
			int hash = 13;
			if (fields != null)
			{
				for (var i = 0; i < fields.Length; i++)
				{
					hash = (hash * 7) + (fields[i] != null ? fields[i].GetHashCode() : 0);
				}
			}
			return hash;
		}
	}
}
