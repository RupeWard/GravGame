using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;
using RJWS.Core.Data;

namespace RJWS.GravGame
{
	public class BlockDefinition: AbstractStringExtractable<BlockDefinition>, IDebugDescribable
	{
		static private readonly bool DEBUG_BlockDefinition = true;

		private BlockPositionInfo _blockPositionInfo = new BlockPositionInfo( );
		private Shape.AbstractShapeDefn _shapeDefinition = null;

		public BlockDefinition() : base()
		{

		}

		public BlockDefinition( BlockPositionInfo bpi, Shape.AbstractShapeDefn asd ) : base( )
		{
			_blockPositionInfo.position = bpi.position;
			_shapeDefinition = asd;
		}

		public BlockDefinition( Vector3 v, Shape.AbstractShapeDefn asd ) : base( )
		{
			_blockPositionInfo.SetFromVector3( v );
			_shapeDefinition = asd;
		}

		public BlockPositionInfo position
		{
			get { return _blockPositionInfo; }
		}

		public Shape.AbstractShapeDefn shape
		{
			get { return _shapeDefinition;  }
		}

		#region AbstractStringExtractable

		protected override bool DebugType( )
		{
			return DEBUG_BlockDefinition;
		}

		protected override bool _addToString( BlockDefinition target, System.Text.StringBuilder sb )
		{
			bool success = false;
			if (target == null)
			{
				target = this;
			}
			if (target._shapeDefinition == null)
			{
				Debug.LogWarning( "Null shapeDefn on attempt to addtoString" );
			}
			else
			{
				_blockPositionInfo.Value = target._blockPositionInfo.Value;
				_blockPositionInfo.AddToString( sb );

				if (Shape.AbstractShapeDefn.WriteShapeDefn(sb, target._shapeDefinition ))
				{
					success = true;
				}
			}
			return success;
		}

		protected override bool _extractFromString( ref string str, ref BlockDefinition result )
		{
			bool success = false;
			if (result == null)
			{
				result = this;
			}
			if (_blockPositionInfo.ExtractOptionalFromString(ref str ))
			{
				result._blockPositionInfo = _blockPositionInfo;
				Shape.AbstractShapeDefn asd=null;
				if (Shape.AbstractShapeDefn.ExtractShapeDefn(ref str, ref asd, true ))
				{
					result._shapeDefinition = asd;
					success = true; 
				}
			}
			return success;
		}

		#endregion AbstractStringExtractable

		#region IDebugDescribable

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "[ " ).DebugDescribe( _blockPositionInfo ).Append(" ");
			if (_shapeDefinition != null)
			{
				sb.DebugDescribe( _shapeDefinition );
			}
			else
			{
				sb.Append( " NULL SHAPE" );
			}
			sb.Append( "]" );
		}
		#endregion IDebugDescribable

	}

}
