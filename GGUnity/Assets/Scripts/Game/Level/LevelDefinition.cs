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
			private set;
		}

		public string levelName
		{
			get;
			private set;
		}

		public LevelDefinition() : base ()
		{
			levelId = s_numLevelsCreated;
			s_numLevelsCreated++;

			levelName = "_Level_" + levelId;
		}

		public LevelDefinition(int id, string n) : base( )
		{
			levelId = id;
			levelName = n;
		}

		#region AbstractStringExtractable

		override protected bool _extractFromString( ref string str)
		{
			// TODO Implement
			return true;
		}

		override protected bool _addToString( System.Text.StringBuilder sb )
		{
			// TODO Implement
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
			sb.Append( "Level ").Append(levelId).Append(" ("+ levelName + ")" );
		}

		#endregion IDebugDescribable

	}

}
