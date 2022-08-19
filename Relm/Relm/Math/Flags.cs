using System;

namespace Relm.Math
{
	public static class Flags
	{
		public static bool IsFlagSet(this int self, int flag)
		{
			return (self & flag) != 0;
		}


		public static bool IsUnshiftedFlagSet(this int self, int flag)
		{
			flag = 1 << flag;
			return (self & flag) != 0;
		}


		public static void SetFlagExclusive(ref int self, int flag)
		{
			self = 1 << flag;
		}


		public static void SetFlag(ref int self, int flag)
		{
			self = (self | 1 << flag);
		}

        public static void UnsetFlag(ref int self, int flag)
		{
			flag = 1 << flag;
			self = (self & (~flag));
		}


		public static void InvertFlags(ref int self)
		{
			self = ~self;
		}


		public static string BinaryStringRepresentation(this int self, int leftPadWidth = 10)
		{
			return Convert.ToString(self, 2).PadLeft(leftPadWidth, '0');
		}
	}
}