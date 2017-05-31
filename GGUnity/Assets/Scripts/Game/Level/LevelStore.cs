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

		private SqliteUtils.IntegerColumn _idCol = new SqliteUtils.IntegerColumn( "LevelId" );
		private SqliteUtils.TextColumn _nameCol = new SqliteUtils.TextColumn( "LevelName" );
		private SqliteUtils.TextColumn _defnCol = new SqliteUtils.TextColumn( "Defn" );

		private SqliteUtils.Table _table = null;

		protected override void PostAwake( )
		{
			// create table object 
			_table
				= new SqliteUtils.Table( "LevelDefn",
					new List<SqliteUtils.Column>
					{
						_idCol,
						_nameCol,
						_defnCol
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
			AddTestDefns( );
		}

		private void AddTestDefns()
		{
			if (_levels.Count < 3 )
			{
				if (GetLevelFromName("L0") != null)
				{
					Debug.LogWarning( "Not overrwiting L0" );
				}
				else
				{
					LevelDefinition level0 = new LevelDefinition( 0, "L0" );
					level0.tmpShapeDefn = new Shape.CircleShapeDefn( 5f );

					AddDefnWithNextHighId( level0 );
				}

				if (GetLevelFromName( "L1" ) != null)
				{
					Debug.LogWarning( "Not overrwiting L1" );
				}
				else
				{
					LevelDefinition level1 = new LevelDefinition( 1, "L1" );
					level1.tmpShapeDefn = new Shape.CircleShapeDefn( 3f );
					level1.AddInitialStaticBlock( new BlockDefinition( Vector3.zero, new Shape.CircleShapeDefn( 1f ) ) );
					level1.AddInitialStaticBlock( new BlockDefinition( new Vector3( 0.1f, 0.1f, 0f ), new Shape.CircleShapeDefn( 0.5f ) ) );

					AddDefnWithNextHighId( level1 );
				}

				if (GetLevelFromName( "L2" ) != null)
				{
					Debug.LogWarning( "Not overrwiting L2" );
				}
				else
				{
					LevelDefinition level2 = new LevelDefinition( 2, "L2" );
					level2.tmpShapeDefn = new Shape.CircleShapeDefn( 2f );
					level2.AddInitialStaticBlock( new BlockDefinition( Vector3.zero, new Shape.RectShapeDefn( new Vector2( 3f, 0.5f ) ) ) );
					level2.AddInitialStaticBlock( new BlockDefinition( new Vector3( 1f, 2f, 0f ), new Shape.RectShapeDefn( new Vector2( 2f, 0.5f ) ) ) );

					AddDefnWithNextHighId( level2 );
				}

				if (DEBUG_LevelStore)
				{
					Debug.LogWarning( "Added Default Defns\n" + this.DebugDescribe( ) );
				}
			}
		}

		private int GetNextFreeId()
		{
			int result = -1;
			
			for (int i = 0; ; i++)
			{ 
				if (! _levels.ContainsKey( i ))
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private int GetHighestId( )
		{
			int result = -1;

			foreach (int i in _levels.Keys)
			{
				if (i > result)
				{
					result = i;
				}
			}
			return result;
		}

		private LevelDefinition GetLevelFromId(int id)
		{
			if (_levels.ContainsKey( id ))
			{
				return _levels[id];
			}
			return null;
		}

		private LevelDefinition GetLevelFromName( string n )
		{
			foreach (LevelDefinition ld in _levels.Values)
			{
				if (ld.levelName == n)
				{
					return ld;
				}
			}
			return null;
		}

		public bool AddDefnWithNextFreeId( LevelDefinition ld )
		{
			ld.levelId = GetNextFreeId( );
			return AddDefn( ld, false );
		}

		public bool AddDefnWithNextHighId( LevelDefinition ld )
		{
			ld.levelId = GetHighestId()+1;
			return AddDefn( ld, false );
		}

		public bool AddDefn(LevelDefinition ld, bool replace)
		{
			if (_levels.ContainsKey(ld.levelId))
			{
				if (!replace)
				{
					Debug.LogWarning( "Refusing to overwrite " + _levels[ld.levelId].DebugDescribe( ) + " with " + ld.DebugDescribe( ) );
					return false;
				}
				_levels[ld.levelId] = ld;
			}
			else
			{
				_levels.Add( ld.levelId, ld );
			}
			SaveDefn( ld, true );

			if (DEBUG_LevelStore)
			{
				Debug.LogWarning( "Added Defn\n" + ld.DebugDescribe( )+"\n"+this.DebugDescribe() );
			}

			return true;
		}

		public void ReloadAllDefns( )
		{
			LoadAllDefns( true );
		}

		public void SaveAll()
		{
			SaveDefns( _levels.Values, true );
		}

		public void SaveDefn(LevelDefinition ld, bool force)
		{
			SqliteConnection connection = SqliteUtils.Instance.getConnection( "CoreData" );

			SqliteCommand insertCommand = connection.CreateCommand( );
			insertCommand.CommandText = _table.GetInsertCommand( force );
			_table.AddParamsToCommand( insertCommand );

			_idCol.SetParamValue( ld.levelId );
			_nameCol.SetParamValue( ld.levelName );

			System.Text.StringBuilder sb = new System.Text.StringBuilder( );
			ld.AddToString( sb );

//			ld.AddToString( sb );
			_defnCol.SetParamValue( sb.ToString( ) );

			insertCommand.ExecuteNonQuery( );
			insertCommand.Dispose( );

			if (DEBUG_LevelStore)
			{
				Debug.LogWarning( "Saved Defn "+ ld.DebugDescribe( ) );
			}

		}

		public void SaveDefns( IEnumerable<LevelDefinition> lds, bool force )
		{
			{
				SqliteConnection connection = SqliteUtils.Instance.getConnection( "CoreData" );

				SqliteCommand insertCommand = connection.CreateCommand( );
				insertCommand.CommandText = _table.GetInsertCommand( force );
				_table.AddParamsToCommand( insertCommand );

				foreach (LevelDefinition ld in lds)
				{
					_idCol.SetParamValue( ld.levelId );
					_nameCol.SetParamValue( ld.levelName );

					System.Text.StringBuilder sb = new System.Text.StringBuilder( );
					ld.AddToString( sb );
					_defnCol.SetParamValue( sb.ToString( ) );

					insertCommand.ExecuteNonQuery( );
				}
				insertCommand.Dispose( );
			}

			if (DEBUG_LevelStore)
			{
				Debug.LogWarning( "Saved All Defns\n" + this.DebugDescribe( ) );
			}

		}

		public void LoadAllDefns( bool bClearFirst = true )
		{
			Debug.Log( "LoadAllDefns" );
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
				int levelId = _idCol.Read( reader, 0 );
				string levelName = _nameCol.Read( reader, 1 );
				string defnData = _defnCol.Read( reader, 2 );


				LevelDefinition defn = new LevelDefinition(levelId, levelName);
				Debug.Log( "- Loading " + levelId + " " + levelName + " '" + defnData + "'" );
				if (defn.ExtractRequiredFromString( ref defnData))
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

			if (DEBUG_LevelStore)
			{
				Debug.LogWarning( "Loaded all Defns\n" + this.DebugDescribe( ) );
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
