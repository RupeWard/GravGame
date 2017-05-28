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
		private string sep
		{
			get { return _sep; }
			set
			{
				if (value.Length < 2)
				{
					throw new System.Exception( "Can't make seps from single character " + value );
				}
				else
				{
					_sep = value;
//					SetEndsRegex( );
				}
			}
		}

		public bool hasBeenSet
		{
			get;
			protected set;
		}

		public void Reset()
		{
			hasBeenSet = false;
		}

		private int sepLength
		{
			get
			{
				if (_sep.Length > 2)
				{
					if (_sep.Length % 1 == 0)
					{
						return _sep.Length / 2;
					}
					else
					{
						return _sep.Length;
					}
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
			get
			{
				string result = _sep.Substring( _sep.Length - sepLength, sepLength );
				if (_sep.Length == sepLength)
				{
					char[] arr = result.ToCharArray( );
					System.Array.Reverse( arr );
					result = new string( arr );
				}
				return result;
			}
		}

		protected AbstractStringExtractable( string s )
		{
			sep = s;
		}

		protected AbstractStringExtractable( ) // using default separators
		{
			sep = "{}";
		}

		/*
		private string ENDS_REGEX = string.Empty;
		
		private void HandleSepsSet( )
		{
			Sep1StartRegex = @"^"+sep0;
		}
		
		private string Sep1StartRegex;
		*/

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

			// FIXME find matching!

			if (str.Length < sep0.Length+sep1.Length)
			{
				Debug.Log( "Can't extract with seps " + sep0 + " " + sep1 + " as str too short '" + str + "'" );
			}
			else
			{
				//			string regexStr = @"^(" + ENDS_REGEX + @")";
				//            System.Text.RegularExpressions.Regex regex =
				//				new System.Text.RegularExpressions.Regex( Sep1StartRegex );
				//			System.Text.RegularExpressions.Match match = regex.Match( str );
				if (str.StartsWith( sep0 ))
				{
					string prevStr = str;

					//Found first. Now search forward for matching

					Debug.Log( "Found " + sep0 + " at start of " + str );

					int index = sep0.Length;

					string contentString = string.Empty;

					int numNestedNotMatched = 0;
					while (!success && index < (str.Length - sep1.Length + 1))
					{
						string testString = str.Substring( index, sep1.Length );
						if (testString.StartsWith( sep0 ))
						{
							Debug.Log( "Found " + sep0 + " at " + index + " (" + numNestedNotMatched + ")" );
							numNestedNotMatched++;
							index = index + sep0.Length;
						}
						else if (testString.StartsWith( sep1 ))
						{
							if (numNestedNotMatched > 0)
							{
								Debug.Log( "Found " + sep1 + " at " + index + " (" + numNestedNotMatched + ")" );
								numNestedNotMatched--;
								index = index + sep1.Length;
							}
							else
							{
								contentString = str.Substring( sep0.Length, index - sep0.Length );
								string msg = "Found matching " + sep1 + " for initial " + sep0 + " in '" + str + "'"
									+ "\nContent = '" + contentString + "'";
								int contentLength = contentString.Length;
								if (_extractFromString( ref contentString ))
								{
									int deleteLength = sep0.Length + contentLength + sep1.Length;
									if (deleteLength > str.Length)
									{
										Debug.LogError( "deleteLength " + deleteLength + " strLen=" + str.Length );
										str = string.Empty;
									}
									else if (deleteLength == str.Length)
									{
										str = string.Empty;
									}
									else
									{
										str = str.Substring( deleteLength );
									}
									msg = msg + "'\nRemaining = '" + str + "'";

									msg = msg + "\n" + GetType( ) + " Extracted from '" + prevStr;
									IDebugDescribable dd = this as IDebugDescribable;
									if (dd != null)
									{
										msg = msg + "\n" + dd.DebugDescribe( );
									}
									Debug.Log( msg );
									success = true;
								}
								else
								{
									Debug.LogWarning( "Failed to extract from " + contentString );
									index = str.Length;
								}

							}
						}
						else
						{
							index = index + 1;
						}
					}

					if (!success)
					{
						string warningStr = GetType( ) + " Failed to read " + typeof( T ) + " from \n" + str;
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
					else
					{
						string msg = GetType( ) + " Extracted  from '" + prevStr + "'";
						/*
						if (_extractFromString( ref contentString ))
						{
							//						string prevStr = str;
							//						str = str.Replace( matchedStr, "" );
							if (DEBUG_AbstractStringExtractable)
							{
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
							string warningStr = GetType( ) + " Failed to read " + typeof( T ) + " from \n" + contentString;
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
						*/
					}

					/*
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
					*/
				}
				else
				{
					Debug.LogWarning( "Doesn't start with " + sep0 + " : " + str );
				}

			}
			return success;
		}

	}

	public class TextExtractable: AbstractStringExtractable<TextExtractable>
	{
		private static readonly bool DEBUG_TextExtractable = true;

		public string Value
		{
			get;
			set;
		}

		public TextExtractable( string v, string sep ) : base( sep )
		{
			Value = v;
		}

		public TextExtractable( string v) : base( )
		{
			Value = v;
		}

		public TextExtractable( ) : base()
		{
		}

		override protected bool DebugType( )
		{
			return DEBUG_TextExtractable || DEBUG_AbstractStringExtractable;
		}
	
		override protected bool _extractFromString( ref string str)
		{
			Debug.Log( "TextExtractor.extractFromString (" + str+ ")" );
			Value = str;
//			str = string.Empty;
			Debug.Log( "TextExtractor got '" + Value + "'" );
			return true;
		}

		override protected bool _addToString( System.Text.StringBuilder sb )
		{
			sb.Append( Value );
			return true;
		}


	}

	public class FloatExtractable : AbstractStringExtractable<FloatExtractable>
	{
		private static readonly bool DEBUG_FloatExtractable = true;

		public float Value
		{
			get;
			set;
		}

		public FloatExtractable( float v, string sep ) : base( sep )
		{
			Value = v;
		}

		public FloatExtractable( float v ) : base(  )
		{
			Value = v;
		}

		public FloatExtractable( ) : base(  )
		{
		}

		override protected bool DebugType( )
		{
			return DEBUG_FloatExtractable || DEBUG_AbstractStringExtractable;
		}

		override protected bool _extractFromString( ref string str )
		{
			bool success = false;
			float parseVal;
			if (float.TryParse(str, out parseVal))
			{
				Debug.Log( "Got float "+parseVal+ " from '" + str + "'" );
				Value = parseVal;
//				str = string.Empty;
				success = true;
			}
			else
			{
				Debug.LogError( "Couldn't parse float from '" + str + "'" );
			}
			return success;
		}

		override protected bool _addToString( System.Text.StringBuilder sb )
		{
			sb.Append( Value );
			return true;
		}


	}


	static public class StringExtractableHelpers
	{

	}
}
