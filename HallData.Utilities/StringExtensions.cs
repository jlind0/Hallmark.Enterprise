using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HallData.Utilities
{
	public static class StringExtensions
	{
		public static int CountCharacterInstances(this string source, char c)
		{
			int count = 0;
			foreach (var ch in source)
				if (ch == c) count++;
			return count;
		}
	}
}
