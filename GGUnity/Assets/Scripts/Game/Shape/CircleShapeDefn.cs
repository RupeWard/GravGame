using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;

namespace RJWS.GravGame.Shape
{
	public class CircleShapeDefn: AbstractShapeDefn
	{
		private static readonly bool DEBUG_CircleShapeDefn = true;

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

		public CircleShapeDefn(): base(EShapeType.Circle)
		{
		}

		public CircleShapeDefn( float f) : base( EShapeType.Circle )
		{
			SetRadius( f );
		}

		public void SetRadius(float f)
		{
			radius = f;
		}

		#region AbstractStringExtractable

		override protected bool _extractFromString( ref string str )
		{
			Debug.Log( "Extracting circle from '" + str + "'" );
			bool success = false;
			if (_radiusExtractor.ExtractFromString(ref str, true ))
			{
				Debug.Log( "Extracted radius "+_radiusExtractor.Value );
				SetRadius( _radiusExtractor.Value );
				success = true;
			}
			return success;
		}

		override protected bool _addToString( System.Text.StringBuilder sb )
		{
			_radiusExtractor.Value = _radius;
			_radiusExtractor.AddToString( sb );
			return true;
		}

		protected override bool DebugType( )
		{
			return DEBUG_CircleShapeDefn || DEBUG_SHAPE;
		}

		protected override void DebugDescribeType( System.Text.StringBuilder sb )
		{
			sb.Append( "r=" ).Append( _radius );
		}

		#endregion AbstractStringExtractable

	}

}
