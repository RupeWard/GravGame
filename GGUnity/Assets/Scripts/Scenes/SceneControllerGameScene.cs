using UnityEngine;
using System.Collections;

using RJWS.Core.DebugDescribable;

public class SceneControllerGameScene : SceneController_Base
{
	public RectTransform canvasRT;

	public UnityEngine.UI.Text levelText;

	private RJWS.GravGame.LevelHandler _levelHandler = null;

	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.GameScene;
	}

	//	static private readonly bool DEBUG_LOCAL = false;


	protected override void PostStart( )
	{
	}

	protected override void PostAwake( )
	{
		levelText.text = RJWS.GravGame.GravGameManager.Instance.currentLevel.levelName;
		_levelHandler = new RJWS.GravGame.LevelHandler( RJWS.GravGame.GravGameManager.Instance.currentLevel );
		_levelHandler.SetUpLevel( );
		_levelHandler.StartGame( );
	}

	void Update()
	{
		if (_levelHandler != null)
		{
			_levelHandler.update(Time.deltaTime );
		}
	}

	public void QuitScene()
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}

	public void HandleQuitButtonPressed()
	{
		QuitScene( );
	}

	public void HandleRestartButtonPressed()
	{
		SceneManager.Instance.ReloadScene( Scene( ) );
	}
}
