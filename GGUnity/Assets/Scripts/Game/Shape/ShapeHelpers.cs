using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;

namespace RJWS.GravGame.Shape
{
	public enum EShapeType
	{
		NONE,
		Circle,
		Triangle,
		Rectangle,
		RegularPolygon,
		Polygon
	}

	public class ShapeExtractable : AbstractStringExtractable< ShapeExtractable>
	{
		protected static readonly bool DEBUG_ShapeExtractable = true;

		public EShapeType value
		{
			get;
			set;
		}

		public ShapeExtractable( EShapeType v, string sep ) : base( sep )
		{
			value = v;
		}

		public ShapeExtractable( EShapeType v ) : base( )
		{
			value = v;
		}

		public ShapeExtractable( ) : base(  )
		{
		}

		override protected bool DebugType( )
		{
			return DEBUG_ShapeExtractable || DEBUG_AbstractStringExtractable;
		}

		override protected bool _extractFromString(ref string str)
		{
			bool success = false;
			EShapeType parseVal = EShapeType.NONE;
			if (ShapeHelpers.ExtractShapeTypeFromString(ref str, ref parseVal, true))
			{
				value = parseVal;
				str = str.Replace(parseVal.ToString(),"");
				success = true;
			}
			return success;
		}

		override protected bool _addToString( System.Text.StringBuilder sb )
		{
			// TODO check if set or not
			sb.Append( value.ToString() );
			return true;
		}


	}
	static class ShapeHelpers
	{
		private static Dictionary<string, EShapeType> _shapeTypeMap = null;
		public static Dictionary<string, EShapeType> shapeTypeMap
		{
			get
			{
				if (_shapeTypeMap == null)
				{
					_shapeTypeMap = new Dictionary<string, EShapeType>( );
					EShapeType[] types = (EShapeType[])System.Enum.GetValues( typeof( EShapeType ) );
					foreach (EShapeType est in types)
					{
						_shapeTypeMap.Add( est.ToString( ), est );
					}
				}
				return _shapeTypeMap;
			}
		}

		static public bool ExtractShapeTypeFromString(ref string str, ref EShapeType result, bool required)
		{
			bool success = false;

			foreach( KeyValuePair<string, EShapeType> kvp in shapeTypeMap )
			{
				if (str.StartsWith(kvp.Key))
				{
					result = kvp.Value;
					str = str.Replace( kvp.Key, "" );
					success = true;
					break;
				}
			}
			if (!success)
			{
				if (required)
				{
					Debug.LogError( "Can't get shape type from '" + str + "'" );
				}
			}
			return success;
		}
	}
}
