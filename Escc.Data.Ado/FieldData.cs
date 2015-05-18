#region Using Directives

using System;
using System.Data;

#endregion

namespace Escc.Data.Ado
{
	/// <summary>
	/// A generic class dedicated to dealing with database field data
	/// </summary>
	public class FieldData
	{
		#region Declarations (Types)
		// abbreviate the types for ease of use
		public static Type typeString = System.Type.GetType("System.String");
        public static Type typeInt = System.Type.GetType("System.Int32");
        public static Type typeDate = System.Type.GetType("System.DateTime");
        public static Type typeBool = System.Type.GetType("System.Boolean");
        public static Type typeDec = System.Type.GetType("System.Decimal");
        public static Type typeByte = System.Type.GetType("System.Byte");
		#endregion

		#region getObject
		public static object getObject(
			DataRow dr,
			string strFieldName)
		{
			if (dr == null) return null;
			object objValue = dr[strFieldName];
			if (objValue == System.DBNull.Value)
				return null;  // converts a DBNull value
			else
				return objValue;
		}

		public static object getObject(
			DataTable dt,
			string strFieldName)
		{
			if (dt == null) return null;
			if (dt.Rows.Count <= 0) return null;
			// call the core DataRow version
			return getObject(dt.Rows[0], strFieldName);
		}

		public static object getObject(
			DataRowView drv,
			string strFieldName)
		{
			if (drv == null) return null;
			if (drv.Row == null) return null;
			// call the core DataRow version
			return getObject(drv.Row, strFieldName);
		}
		#endregion

		#region getInteger
        public static int getInteger(
			DataRow dr,
			string strFieldName)
		{
			object fldValue = getObject(dr, strFieldName);
			return Convert.ToInt32(fldValue);
		}

        public static int getInteger(
			DataTable dt,
			string strFieldName)
		{
			if (dt == null) return Convert.ToInt32(null);
			if (dt.Rows.Count <= 0) return Convert.ToInt32(null);
			// call the core DataRow version
			return getInteger(dt.Rows[0], strFieldName);
		}

        public static int getInteger(
			DataRowView drv,
			string strFieldName)
		{
			if (drv == null) return Convert.ToInt32(null);
			if (drv.Row == null) return Convert.ToInt32(null);
			// call the core DataRow version
			return getInteger(drv.Row, strFieldName);
		}
		#endregion

        #region getDecimal
        public static decimal getDecimal(
			DataRow dr,
			string strFieldName)
		{
			object fldValue = getObject(dr, strFieldName);
			return Convert.ToDecimal(fldValue);
		}

        public static decimal getDecimal(
			DataTable dt,
			string strFieldName)
		{
			if (dt == null) return Convert.ToDecimal(null);
			if (dt.Rows.Count <= 0) return Convert.ToDecimal(null);
			// call the core DataRow version
			return getDecimal(dt.Rows[0], strFieldName);
		}

        public static decimal getDecimal(
			DataRowView drv,
			string strFieldName)
		{
			if (drv == null) return Convert.ToDecimal(null);
			if (drv.Row == null) return Convert.ToDecimal(null);
			// call the core DataRow version
			return getDecimal(drv.Row, strFieldName);
		}
		#endregion

		#region getString
        public static string getString(
			DataRow dr,
			string strFieldName)
		{
			object fldValue = getObject(dr, strFieldName);
			return Convert.ToString(fldValue);
		}

        public static string getString(
			DataTable dt,
			string strFieldName)
		{
			if (dt == null) return string.Empty;
			if (dt.Rows.Count <= 0) return string.Empty;
			// call the core DataRow version
			return getString(dt.Rows[0], strFieldName);
		}

        public static string getString(
			DataRowView drv,
			string strFieldName)
		{
			if (drv == null) return string.Empty;
			if (drv.Row == null) return string.Empty;
			// call the core DataRow version
			return getString(drv.Row, strFieldName);
		}
		#endregion

		#region getDateTime
        public static DateTime getDateTime(
			DataRow dr,
			string strFieldName)
		{
			object fldValue = getObject(dr, strFieldName);
			DateTime dateValue = Convert.ToDateTime(null);
			try
			{
				dateValue = Convert.ToDateTime(fldValue);
			}
			catch
			{
				dateValue = Convert.ToDateTime(null);
			}
			return dateValue;
		}

        public static DateTime getDateTime(
			DataTable dt,
			string strFieldName)
		{
			if (dt == null) return Convert.ToDateTime(null);
			if (dt.Rows.Count <= 0) return Convert.ToDateTime(null);
			// call the core DataRow version
			return getDateTime(dt.Rows[0], strFieldName);
		}

        public static DateTime getDateTime(
			DataRowView drv,
			string strFieldName)
		{
			if (drv == null) return Convert.ToDateTime(null);
			if (drv.Row == null) return Convert.ToDateTime(null);
			// call the core DataRow version
			return getDateTime(drv.Row, strFieldName);
		}
		#endregion

        #region getBoolean
        public static bool getBoolean(
			DataRow dr,
			string strFieldName)
		{
			object fldValue = getObject(dr, strFieldName);
			return Convert.ToBoolean(fldValue);
		}

        public static bool getBoolean(
			DataTable dt,
			string strFieldName)
		{
			if (dt == null) return Convert.ToBoolean(null);
			if (dt.Rows.Count <= 0) return Convert.ToBoolean(null);
			// call the core DataRow version
			return getBoolean(dt.Rows[0], strFieldName);
		}

        public static bool getBoolean(
			DataRowView drv,
			string strFieldName)
		{
			if (drv == null) return Convert.ToBoolean(null);
			if (drv.Row == null) return Convert.ToBoolean(null);
			// call the core DataRow version
			return getBoolean(drv.Row, strFieldName);
		}
		#endregion

	}
}
