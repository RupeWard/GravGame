using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;

using RJWS.Core.Singleton;
using RJWS.Core.Data;
using RJWS.Core.DebugDescribable;

namespace RJWS.GravGame
{
	public class LevelStore : SingletonApplicationLifetimeLazy< LevelStore >, IDebugDescribable
	{
		public static readonly bool DEBUG_LevelStore = true;
		public static readonly bool DEBUG_LevelIO = true;

		private Dictionary<int, LevelDefinition> _levels = new Dictionary<int, LevelDefinition>( );

		private SqliteUtils.IntegerColumn IdCol = new SqliteUtils.IntegerColumn( "LevelId" );
		private SqliteUtils.TextColumn nameCol = new SqliteUtils.TextColumn( "LevelName" );
		private SqliteUtils.TextColumn defnCol = new SqliteUtils.TextColumn( "Defn" );

		private SqliteUtils.Table _table = null;

		protected override void PostAwake( )
		{
			// create table object 
			_table
				= new SqliteUtils.Table( "LevelDefn",
					new List<SqliteUtils.Column>
					{
						IdCol,
						nameCol,
						defnCol
					}
				);
			if (_table.CreateIfNecessary( "CoreData"))
			{
				if (DEBUG_LevelStore)
				{
					Debug.Log( GetType( ) + " Created "+_table.DebugDescribe() );
				}
			}

			// load table
			LoadAllDefns( true );


		}

		public void LoadAllDefns( bool bClearFirst = true )
		{
			if (bClearFirst)
			{
				_levels.Clear( );
			}

			SqliteConnection connection = SqliteUtils.Instance.getConnection( "CoreData" );

			SqliteCommand selectCommand = connection.CreateCommand( );
			selectCommand.CommandText = _table.GetSelectCommand( );

			SqliteDataReader reader = selectCommand.ExecuteReader( );
			while (reader.Read( ))
			{
				int levelId = IdCol.Read( reader, 0 );
				string levelName = nameCol.Read( reader, 1 );
				string defnData = defnCol.Read( reader, 2 );

				LevelDefinition defn = null;
				if (defn.ExtractOptionalFromString( ref defnData))
				{
					if (_levels.ContainsKey(levelId))
					{
						if (DEBUG_LevelIO)
						{
							Debug.Log( GetType( ) + " replacing id=" + levelId + "\n" + _levels[levelId].DebugDescribe( ) + "\n...with...\n" + defn.DebugDescribe( ) );
						}
						_levels[levelId]= defn;
					}
					else
					{
						if (DEBUG_LevelIO)
						{
							Debug.Log( GetType( ) + " loading id=" + levelId +  "\n...as...\n" + defn.DebugDescribe( ) );
						}
						_levels.Add( levelId, defn );
					}
				}
				else
				{
					Debug.LogError( "Couldn't extract LevelDefinition data from '" + defnData+"'" );
				}
			}
		}

		/*
		public LevelDefinition GetDefn( int id )
		{
			LevelDefinition result = null;

			SqliteConnection connection = SqliteUtils.Instance.getConnection( "CoreData" );

			SqliteCommand selectCommand = connection.CreateCommand( );
			selectCommand.CommandText = _table.GetSelectCommand( );
			selectCommand.CommandText = selectCommand.CommandText + " WHERE " + IdCol.name + " = '" + id + "'";
			SqliteDataReader reader = selectCommand.ExecuteReader( );
			while (reader.Read( ))
			{
				int levelId = IdCol.Read( reader, 0 );
				string levelName = nameCol.Read( reader, 1 );
				string defnData = defnCol.Read( reader, 2 );

				result = new LevelDefinition( levelId, levelName );
				if (result.ExtractOptionalFromString(ref defnData))
				{

				}
				else
				{
					result = null;
				}                 
			}
			selectCommand.Dispose( );
			return result;
		}
		*/

		#region IDebugDescribable

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "LevelStore has " + _levels.Count + " entries" );
			foreach (KeyValuePair<int, LevelDefinition> entry in _levels)
			{
				sb.Append( "\n ").Append(entry.Key).Append(" [ ");
				sb.DebugDescribe( entry.Value );
				sb.Append( " ]" );
			}
		}

		#endregion IDebugDescribable
	}

}
