using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;

namespace RJWS.GravGame.Shape
{
	public class PolygonShapeDefn: AbstractShapeDefn
	{
		private static readonly bool DEBUG_PolygonShapeDefn = true;

		/*
        private float _radius = 0f;

		public float radius
		{
			get
			{
				if (!_radiusSet)
				{
					Debug.LogWarning( "Accessing radiusSet before setting it" );
				}
				return _radius;
			}
			set
			{
				_radius = value;
				_radiusSet = true;
			}
		}

		private bool _radiusSet = false;
		private FloatExtractable _radiusExtractor = new FloatExtractable( );

		private const float DEFAULT_RADIUS = 1f;
		*/

		private const string polygonSeps = " POLY{}poly ";

		public PolygonShapeDefn( ): base(EShapeType.Polygon, polygonSeps)
		{
		}

		public PolygonShapeDefn( float f) : base( EShapeType.Polygon, polygonSeps )
		{
//			SetRadius( f );
		}

		#region AbstractStringExtractable

		override protected bool _extractFromString( ref string str, ref AbstractShapeDefn result )
		{
			bool success = false;
			if (result == null)
			{
				result = this;
			}

			PolygonShapeDefn poly = result as PolygonShapeDefn;
			if (poly == null)
			{
				Debug.LogError( "NOT A POLYGON, A " + result.eShapeType );
			}
			else
			{
				Debug.Log( "Extracting polygon from '" + str + "'" );
				/*
				if (_radiusExtractor.ExtractFromString( ref str, true ))
				{
					Debug.Log( "Extracted radius " + _radiusExtractor.Value );

					circle.SetRadius( _radiusExtractor.Value );
					success = true;
				}
				*/
			}
			return success;
		}

		override protected bool _addToString(AbstractShapeDefn target, System.Text.StringBuilder sb )
		{
			bool success = false;
			if (target == null)
			{
				target = this;
			}
			PolygonShapeDefn poly = target as PolygonShapeDefn;
			if (poly== null)
			{
				Debug.LogError( "NOT A POLYGON, A " + target.eShapeType );
			}
			else
			{
				/*
				_radiusExtractor.Value = circle._radius;
				_radiusExtractor.AddToString( sb );
				success = true;
				*/
			}
			return success;
		}

		protected override bool DebugType( )
		{
			return DEBUG_PolygonShapeDefn || DEBUG_SHAPE;
		}

		protected override void DebugDescribeType( System.Text.StringBuilder sb )
		{
//			sb.Append( "r=" ).Append( _radius );
		}

		#endregion AbstractStringExtractable

	}

}
