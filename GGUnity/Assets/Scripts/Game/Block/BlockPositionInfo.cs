using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;
using RJWS.Core.DebugDescribable;

namespace RJWS.GravGame
{
	public class BlockPositionInfo : AbstractStringExtractable< Vector3 >, IDebugDescribable
	{
		private static readonly bool DEBUG_BlockPositionInfo = true;

		private Vector3 _info = Vector3.zero;

		public Vector2 position
		{
			get { return new Vector2( _info.x, _info.y ); }
			set
			{
				_info.x = value.x;
				_info.y = value.y;
			}
		}

		public float zRotation
		{
			get { return _info.z;  }
			set
			{
				_info.z = value;
			}
		}

		private const string blockSep = " BP{}bp ";
		public BlockPositionInfo(): base(blockSep)
		{

		}

		public BlockPositionInfo(Vector2 v, float r): base(blockSep)
		{
			position = v;
			zRotation = r;
		}

		public BlockPositionInfo( Vector3 v) : base( blockSep )
		{
			SetFromVector3( v );
		}

		public BlockPositionInfo( Transform t ) : base( blockSep )
		{
			SetFromTransform( t );
		}


		public void SetFromVector3(Vector3 v)
		{
			position = new Vector2(v.x, v.y);
			zRotation = v.z;
		}

		public void SetFromTransform(Transform t)
		{
			position = new Vector2( t.localPosition.x, t.localPosition.y );
			zRotation = t.rotation.eulerAngles.z;
		}


		#region AbstractStringExtractable

		protected override bool DebugType( )
		{
			return DEBUG_BlockPositionInfo;
		}

		protected override bool _addToString( Vector3 target, System.Text.StringBuilder sb )
		{
			sb.Append( target.ToString( ) );
			return true;
		}

		protected override bool _extractFromString( ref string str, ref Vector3 result )
		{
			bool success = false;
			if (DataHelpers.extractOptionalVector3(ref str, ref result))
			{
				// Value = result; // FIXME??
				success = true;
			}
			else
			{
				Debug.LogWarning( "Failed to extract Vector3 for BlockPosition from '" + str +"'");
			}
			return success;
		}

		#endregion AbstractStringExtractable

		#region IDebugDescribable

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "[POS=" ).Append( position ).Append( ", zR=" ).Append( zRotation ).Append( "]" );
		}
		#endregion IDebugDescribable

	}

}
