using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.GravGame
{
	public class BlockFactory : RJWS.Core.Singleton.SingletonSceneLifetime<BlockFactory>
	{
		private int _numBlocks = 0;

		public Material staticBlockMat;
		public Material freeBlockMat;
		public Material selectBlockMat;

		public GameObject blockPrefab;

		protected override void PostAwake( )
		{
		}

		public Block CreateStaticBlock(Transform container, BlockDefinition bd)
		{
			GameObject go = GameObject.Instantiate( blockPrefab ) as GameObject;
			go.name = "Block_" + _numBlocks.ToString( "0000" );
			Block block = go.GetComponent<Block>( );
			block.cachedTransform.SetParent( container );
            block.cachedTransform.localPosition = new Vector3( bd.position.position.x, bd.position.position.y, 0f );
			block.cachedTransform.rotation = Quaternion.Euler( new Vector3( 0f, 0f, bd.position.zRotation ) );
			block.cachedTransform.localScale = Vector3.one;
			block.Init( bd );
			block.MakeStatic( staticBlockMat);
			_numBlocks++;
			return block;
		}
	}

}
