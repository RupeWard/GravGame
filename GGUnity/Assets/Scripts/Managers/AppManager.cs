using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS
{
	public class AppManager : RJWS.Core.Singleton.SingletonApplicationLifetimeLazy< AppManager >
	{
		public const float MIN_CLICKABLE_mm = 7f;

		public float minClickablePixels
		{
			get;
			private set;
		}

		private Dictionary<string, int> _layers
			= new Dictionary<string, int>( )
			{
				{ "Background", 8 },
				{ "Block", 9 },
				{ "Effects", 10 }
			};
		protected override void PostAwake( )
		{
			float minInches = MIN_CLICKABLE_mm / 25.4f;
			minClickablePixels = minInches * Screen.dpi;
		}

		public int GetLayerIndex(string layerName)
		{
			int result;
			if (_layers.TryGetValue(layerName, out result))
			{
			}
			else
			{
				Debug.LogError( "No layer called " + layerName );
				result = 0;
			}
			return result;
		}
	}

}
