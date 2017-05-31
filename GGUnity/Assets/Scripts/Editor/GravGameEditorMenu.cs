using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using RJWS.Core.Data;

namespace RJWS.GravGame
{
	public class GravGameEditorMenu
	{
		[MenuItem( "GravGame/DB/Delete CoreData", false, 1000 )]
		public static void MenuItem_DeleteCoreData( )
		{
			SqliteUtils.DeleteDB( "CoreData" );
		}
	}
}
