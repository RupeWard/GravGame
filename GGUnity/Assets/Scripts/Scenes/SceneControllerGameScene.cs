using UnityEngine;
using System.Collections;

using RJWS.Core.DebugDescribable;

public class SceneControllerGameScene : SceneController_Base
{
	public RectTransform canvasRT;

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
	}

	public void QuitScene()
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}

}
