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

		private const string circleSeps = " CIRC{}circ ";
        public CircleShapeDefn(): base(EShapeType.Circle, circleSeps)
		{
		}

		public CircleShapeDefn( float f) : base( EShapeType.Circle, circleSeps )
		{
			SetRadius( f );
		}

		public void SetRadius(float f)
		{
			radius = f;
		}

		override public List<Vector2> GetEdgePoints( Vector2 centre, float resolution )
		{
			List<Vector2> result = new List<Vector2>( );

			float circumf = 2f * Mathf.PI * _radius;
			int num = Mathf.CeilToInt( circumf / resolution );

			float angleStep = 2f * Mathf.PI / num;

			for (int i = 0; i < num; i++)
			{
				float angle = angleStep * i;
				result.Add( 
					new Vector2( 
						centre.x + _radius * Mathf.Cos(angle ),
						centre.y + _radius * Mathf.Sin( angle  )
                        ));
			}
			return result;
		}


		#region AbstractStringExtractable

		override protected bool _extractFromString( ref string str, ref AbstractShapeDefn result )
		{
			bool success = false;
			if (result == null)
			{
				result = this;
			}
			CircleShapeDefn circle = result as CircleShapeDefn;
			if (circle == null)
			{
				Debug.LogError( "NOT A CIRCLE, A " + result.eShapeType );
			}
			else
			{
				Debug.Log( "Extracting circle from '" + str + "'" );
				if (_radiusExtractor.ExtractFromString( ref str, true ))
				{
					Debug.Log( "Extracted radius " + _radiusExtractor.Value );

					circle.SetRadius( _radiusExtractor.Value );
					success = true;
				}
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
			CircleShapeDefn circle = target as CircleShapeDefn;
			if (circle == null)
			{
				Debug.LogError( "NOT A CIRCLE, A " + target.eShapeType );
			}
			else
			{
				_radiusExtractor.Value = circle._radius;
				_radiusExtractor.AddToString( sb );
				success = true;
			}
			return success;
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
