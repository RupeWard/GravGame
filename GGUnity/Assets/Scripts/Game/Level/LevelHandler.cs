using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

namespace RJWS.GravGame
{
	public class LevelHandler 
	{
		private LevelDefinition _levelDef = null;
		private Transform blockContainer = null;

		public LevelHandler (LevelDefinition ld)
		{
			_levelDef = ld;
			blockContainer = GameObject.Find( "3DWorld" ).transform;
		}

		public void SetUpLevel()
		{
			DeleteAllBlocks( );

			foreach (BlockDefinition bd in _levelDef.initialStaticBlocks)
			{
				Block newBlock = BlockFactory.Instance.CreateStaticBlock( blockContainer, bd );
				if (newBlock == null)
				{
					Debug.LogError( "Failed to make static block from " + bd.DebugDescribe( ) );
				}
				else
				{
					_staticBlocks.Add( newBlock );
					Debug.Log( "Created static block from " + bd.DebugDescribe( ) );
				}
			}
		}

		private List<Block> _staticBlocks = new List<Block>( );
		private List<Block> _freeBlocks = new List<Block>( );
		private Block _selectBlock = null;

		private void DeleteAllBlocks(List<Block> l)
		{
			for (int i = 0; i < l.Count; i++)
			{
				GameObject.Destroy( l[i].gameObject );
			}
			l.Clear( );
		}

		private void DeleteAllBlocks()
		{
			DeleteAllBlocks( _staticBlocks );
			DeleteAllBlocks( _freeBlocks );
			if (_selectBlock)
			{
				GameObject.Destroy( _selectBlock );
				_selectBlock = null;
			}
		}
	}
}
