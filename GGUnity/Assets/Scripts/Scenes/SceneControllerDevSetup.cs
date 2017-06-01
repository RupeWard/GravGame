using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

using RJWS.GravGame;

public class SceneControllerDevSetup: SceneController_Base 
{
	#region inspector hooks

	public UnityEngine.UI.Text versionText;
	public UnityEngine.UI.Text levelText;

	private RJWS.GravGame.LevelDefinition _currentLevel = null;

	private void SetCurrentLevel(RJWS.GravGame.LevelDefinition ld)
	{
		_currentLevel = ld;
		if (_currentLevel == null)
		{
			levelText.text = "No Current Level";
		}
		else
		{
			levelText.text = "Level: " + _currentLevel.DebugDescribe( );
		}
	}

	#endregion inspector hooks

	#region event handlers

	public void HandlePlayButtonPressed( )
	{
		if (_currentLevel == null)
		{
			Debug.LogError( "Can't play, no level" );
		}
		else
		{
			GravGameManager.Instance.currentLevel = _currentLevel;
			SceneManager.Instance.SwitchScene( SceneManager.EScene.GameScene );
		}
	}

	public void HandleQuitButtonPressed( )
	{
		Application.Quit();
	}

	public void HandleNextLevelButton()
	{
		SetCurrentLevel( LevelStore.Instance.GetNext( _currentLevel ));
	}

	#endregion event handlers

	#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.DevSetup;
	}

	override protected void PostStart()
	{
		versionText.text = RJWS.Core.Version.Version.versionNumber.DebugDescribe( );
		SetCurrentLevel( null );
	}

	override protected void PostAwake()
	{
	}

#endregion SceneController_Base

}
