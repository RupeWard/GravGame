using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Data;
using RJWS.Core.DebugDescribable;

namespace RJWS.GravGame
{
	public class LevelDefinition : AbstractStringExtractable< LevelDefinition >, IDebugDescribable
	{
		public static readonly bool DEBUG_LevelDefinition = true;

		private static int s_numLevelsCreated = 0;

		public int levelId
		{
			get;
			set;
		}

		public string levelName
		{
			get;
			set;
		}

		private List<BlockDefinition> _initialStaticBlocks = new List<BlockDefinition>( );
		public void AddInitialStaticBlock(BlockDefinition bd)
		{
			_initialStaticBlocks.Add( bd );
		}

		private ListExtractable<BlockDefinition, BlockDefinition> _staticBlocksExtractor = new ListExtractable<BlockDefinition, BlockDefinition>( );
		 
		private const string LEVEL_SEPS = " LEVEL{}level ";
        public Shape.AbstractShapeDefn tmpShapeDefn = null;

		public LevelDefinition() : base (LEVEL_SEPS)
		{
			levelId = s_numLevelsCreated;
			s_numLevelsCreated++;

			levelName = "_Level_" + levelId;
		}

		public LevelDefinition(int id, string n) : base(LEVEL_SEPS )
		{
			levelId = id;
			levelName = n;
		}

		#region AbstractStringExtractable

		override protected bool _extractFromString( ref string str, ref LevelDefinition result)
		{
			bool success = false;

			if (result == null)
			{
				result = this;// FIXME
			}

			if (_staticBlocksExtractor.ExtractRequiredFromString(ref str ))
			{
				_staticBlocksExtractor.ConsumeList(_initialStaticBlocks );

				Shape.AbstractShapeDefn asdIn = null;
				if (Shape.AbstractShapeDefn.ExtractShapeDefn( ref str, ref asdIn, true ))
				{
					tmpShapeDefn = asdIn;
					result.tmpShapeDefn = tmpShapeDefn;
					result._initialStaticBlocks = _initialStaticBlocks;
					success = true;
				}
				else
				{
					Debug.LogWarning( "Failed to extract shape defn from '" + str + "'" );
				}

			}
			else
			{
				Debug.LogWarning( "Failed to extract initial static blocks list from '" + str + "'" );
			}

			return success;
		}

		override protected bool _addToString( LevelDefinition target, System.Text.StringBuilder sb )
		{
			if (target == null)
			{
				// FIXME or write empty string
				target = this;
			}
			_staticBlocksExtractor = new ListExtractable<BlockDefinition, BlockDefinition>( _initialStaticBlocks );
			_staticBlocksExtractor.AddToString( sb );

			Shape.AbstractShapeDefn.WriteShapeDefn( sb, target.tmpShapeDefn );
			return true;
		}

		protected override bool DebugType( )
		{
			return DEBUG_LevelDefinition;
		}

		#endregion AbstractStringExtractable

		#region IDebugDescribable

		public void DebugDescribe( System.Text.StringBuilder sb )
		{
			sb.Append( "[ Level ").Append(levelId).Append(" ("+ levelName + ") " );
			if (_initialStaticBlocks.Count == 0)
			{
				sb.Append( "NO ISBs " );
			}
			else
			{
				sb.Append( _initialStaticBlocks.Count ).Append( " ISBs" );
				if (_initialStaticBlocks.Count > 0)
				{
					sb.Append( ":" );
					foreach (BlockDefinition bd in _initialStaticBlocks)
					{
						sb.DebugDescribe( bd );
					}
				}
				sb.Append( " " );
			}
			if (tmpShapeDefn != null)
			{
				tmpShapeDefn.DebugDescribe( sb );
			}
			else
			{
				sb.Append( "NULL TSD" );
			}
			sb.Append( "]" );
		}

		#endregion IDebugDescribable

	}

}
