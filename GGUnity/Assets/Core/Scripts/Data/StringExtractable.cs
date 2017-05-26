using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RJWS.Core.DebugDescribable;

namespace RJWS.Core.Data
{
	public abstract class AbstractStringExtractable<T>
	{
		static protected bool DEBUG_AbstractStringExtractable = true;

		abstract protected bool DebugType( );

		abstract protected bool _extractFromString( ref string str);
		abstract protected bool _addToString( System.Text.StringBuilder sb );

		private string _sep = "{}";

		private int sepLength
		{
			get
			{
				if (_sep.Length > 2)
				{
					return _sep.Length / 2;
				}
				else
				{
					return 1;
				}
			}
		}

		protected string sep0
		{
			get
			{
				return _sep.Substring( 0, sepLength );
			}
		}

		protected string sep1
		{
			get { return _sep.Substring( _sep.Length - sepLength, sepLength ); }
		}

		protected AbstractStringExtractable( string s )
		{
			int l = s.Length;
			if (l == 1 || l == 2)
			{
				_sep = s;
			}
			else
			{
				throw new System.Exception( "string len = " + l );
			}
		}

		private string ENDS_REGEX = string.Empty;
		private void SetEndsRegex( )
		{
			ENDS_REGEX = sep0 + @"([.+])" + sep1;
		}

		protected AbstractStringExtractable( ) // using default separators
		{

		}

		public void AddToString( System.Text.StringBuilder sb )
		{
			sb.Append( sep0 );
			_addToString( sb );
			sb.Append( sep1 );
		}

		public bool ExtractOptionalFromString( ref string str)
		{
			return (ExtractFromString( ref str, false ));
		}

		public bool ExtractRequiredFromString( ref string str)
		{
			return (ExtractFromString( ref str, true));
		}

		public bool ExtractFromString( ref string str, bool required )
		{
			bool success = false;

			System.Text.RegularExpressions.Regex regex =
				new System.Text.RegularExpressions.Regex( @"^(" + ENDS_REGEX + @")" );
			System.Text.RegularExpressions.Match match = regex.Match( str );
			if (match.Success && match.Groups.Count == 3)
			{
				string matchedStr = match.Groups[1].Value;
				string contentStr = match.Groups[2].Value;

				if (_extractFromString( ref contentStr))
				{
					string prevStr = str;
					str = str.Replace( matchedStr, "" );
					if (DEBUG_AbstractStringExtractable)
					{
						string msg = GetType( ) + " Extracted " + typeof( T ) + " from '" + prevStr + "\nContent=" + match.Groups[2].Value + "\nRemainder = '" + str + "'" ;
						IDebugDescribable dd = this as IDebugDescribable;
						if (dd != null)
						{
							msg = msg + "\n" + dd.DebugDescribe( );
						}
						Debug.Log( msg );
					}
					success = true;
				}
				else
				{
					string warningStr = GetType( ) + " Failed to read " + typeof( T ) + " from \n" + match.Groups[2].Value + "\nIn\n" + matchedStr;
					if (required)
					{
						throw new System.Exception( warningStr );
					}
					else
					{
						if (DEBUG_AbstractStringExtractable || DebugType( ))
						{
							Debug.LogWarning( warningStr );
						}
					}
				}
			}
			return success;
		}

	}


	static public class StringExtractableHelpers
	{

	}
}
