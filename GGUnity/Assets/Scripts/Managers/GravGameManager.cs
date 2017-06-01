using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.GravGame
{
	public class GravGameManager : RJWS.Core.Singleton.SingletonApplicationLifetimeLazy<GravGameManager>
	{
		protected override void PostAwake( )
		{
			LevelStore.CreateInstance( ); 
		}

		private LevelDefinition _currentLevel = null;
		public LevelDefinition currentLevel
		{
			get
			{
				return _currentLevel;
			}
			set
			{
				_currentLevel = value;
			}
		}

	}
}
