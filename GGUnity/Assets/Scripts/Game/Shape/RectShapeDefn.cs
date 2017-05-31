using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;

namespace RJWS.GravGame.Shape
{
	public class RectShapeDefn: AbstractShapeDefn
	{
		private static readonly bool DEBUG_RectShapeDefn = true;

        private Vector2 _dims = Vector2.zero;

		public Vector2 dims
		{
			get
			{
				if (!_dimsSet)
				{
					Debug.LogWarning( "Accessing dims.Set before setting it" );
				}
				return _dims;
			}
			set
			{
				_dims= value;
				_dimsSet = true;
			}
		}

		private bool _dimsSet = false;

//		private FloatExtractable _radiusExtractor = new FloatExtractable( );

//		private const float DEFAULT_RADIUS = 1f;

		private const string rectSeps = " RECT{}rect ";
        public RectShapeDefn(): base(EShapeType.Rectangle, rectSeps)
		{
		}

		public RectShapeDefn( Vector2 d) : base( EShapeType.Rectangle, rectSeps)
		{
			dims = d;
		}


		#region AbstractStringExtractable

		override protected bool _extractFromString( ref string str, ref AbstractShapeDefn result )
		{
			bool success = false;
			if (result == null)
			{
				result = this;
			}
			RectShapeDefn rect = result as RectShapeDefn;
			if (rect == null)
			{
				Debug.LogError( "NOT A RECT, A " + result.eShapeType );
			}
			else
			{
				Debug.Log( "Extracting rect from '" + str + "'" );

				Vector2 v = Vector2.zero;
				if (DataHelpers.extractOptionalVector2(ref str, ref v ))
				{
					Debug.Log( "Extracted dims: " + v );
					dims = v;
					rect.dims = v;
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
			RectShapeDefn rect = target as RectShapeDefn;
			if (rect == null)
			{
				Debug.LogError( "NOT A RECT, A " + target.eShapeType );
			}
			else
			{
				sb.Append( rect.dims );
				success = true;
			}
			return success;
		}

		protected override bool DebugType( )
		{
			return DEBUG_RectShapeDefn || DEBUG_SHAPE;
		}

		#endregion AbstractStringExtractable

		#region DebugDescribable

		protected override void DebugDescribeType( System.Text.StringBuilder sb )
		{
			sb.Append( "dims=" ).Append( dims );
		}

		#endregion DebugDescribable

	}

}
