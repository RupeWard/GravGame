using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;
using RJWS.Core.DebugDescribable;

namespace RJWS.GravGame.Shape
{
	abstract public class AbstractShapeDefn : AbstractStringExtractable<AbstractShapeDefn>, IDebugDescribable
	{
		public static readonly bool DEBUG_SHAPE = true;
		private static readonly TextExtractable s_shapeTypeExtractable = new TextExtractable( );
		 
		public EShapeType eShapeType
		{
			get;
			protected set;
		}

		protected AbstractShapeDefn( EShapeType est ): base(" SHAPE{}shape ")
		{
			eShapeType = est;
		}

		public static bool WriteShapeDefn(System.Text.StringBuilder sb, AbstractShapeDefn asd)
		{
			System.Text.StringBuilder defnSb = new System.Text.StringBuilder( );
			asd.AddToString( defnSb );

			s_shapeTypeExtractable.Value = asd.eShapeType.ToString( );
			s_shapeTypeExtractable.AddToString( sb );

			//			s_shapeTypeExtractable.Value = defnSb.ToString();
			//			s_shapeTypeExtractable.AddToString( sb );

			sb.Append( defnSb );
			return true;
		}

		public static bool ExtractShapeDefn( ref string str, ref AbstractShapeDefn result, bool required )
		{
			Debug.Log( "Trying to extract shape defn from '" + str + "'" );

			bool success = false;

			if (s_shapeTypeExtractable.ExtractFromString(ref str, required))
			{
				string shapetypeStr = s_shapeTypeExtractable.Value;
				Debug.Log( "shapeTypeStr = " + shapetypeStr );

				EShapeType shapeType = EShapeType.NONE;
				if (ShapeHelpers.ExtractShapeTypeFromString(ref shapetypeStr, ref shapeType, required))
				{
					switch (shapeType)
					{
						case EShapeType.Circle:
							{
								CircleShapeDefn circleDefn = new CircleShapeDefn( );
								if (circleDefn.ExtractRequiredFromString(ref str))
								{
									Debug.Log( "Extracted CircleDefn " + circleDefn.DebugDescribe( ) );
									result = circleDefn;
									success = true;
								}
								else
								{
									Debug.LogWarning( "Failed to extract circleDefn from '" + str + "'" );

								}
								break;
							}
						default:
							{
								Debug.LogError( "Unhandled shape type: " + shapeType );
								break;
							}
					}
				}
			}
			else
			{
				Debug.LogWarning( "Failed to get shape type from '" + str + "'" );
			}
			return success;
		}

		#region IDebugDescribable

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "Shape t=" ).Append( eShapeType ).Append(" ");
			DebugDescribeType( sb );
		}

		abstract protected void DebugDescribeType( System.Text.StringBuilder sb );

		#endregion IDebugDescribable

	}
}
