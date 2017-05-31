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

		public Shape.AbstractShapeDefn tmpShapeDefn = null;

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

		override protected bool _extractFromString( ref string str, ref LevelDefinition result)
		{
			bool success = false;

			result = this;
			Shape.AbstractShapeDefn asdIn = null;
			if (Shape.AbstractShapeDefn.ExtractShapeDefn(ref str, ref asdIn, true))
			{
				result.tmpShapeDefn = asdIn;
				success = true;
			}
			else
			{
				Debug.LogWarning( "Failed to extract shape defn from '" + str + "'" );
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
