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
		private IBlockSequencer _blockSequencer = null;

		public LevelHandler (LevelDefinition ld)
		{
			_levelDef = ld;
			blockContainer = GameObject.Find( "3DWorld" ).transform;
		}

		public void StartGame()
		{
			_blockSequencer = _levelDef.GetBlockSequencer( );
		}

		private Block _currentSelectedBlock = null;

		private Transform spawnPoint;

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
			if (spawnPoint == null)
			{
				spawnPoint = new GameObject( "SpawnPoint" ).transform;
				spawnPoint.SetParent( blockContainer, false );
			}
			// TODO SET UP SPAWN POINT REL TO VIEW
			spawnPoint.localPosition = new Vector3( 0f, 4f, 0f );
		}

		public void update(float deltaTime)
		{
			if (_currentSelectedBlock == null)
			{
				if (_blockSequencer == null)
				{

				}
				else if (_blockSequencer.IsFinished())
				{
					// end game
				}
				else
				{
					Shape.AbstractShapeDefn shapeDefn = _blockSequencer.GetNextShapeDefn( );
					BlockDefinition blockDefn = new BlockDefinition( new Vector3( spawnPoint.localPosition.x, spawnPoint.localPosition.y, 0f ), shapeDefn);
					_currentSelectedBlock = BlockFactory.Instance.CreateSelectedBlock(blockContainer, blockDefn) ;
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
