using UnityEngine;

namespace Unity_UI_Samples.Scripts
{
	public class ApplicationManager : MonoBehaviour {
	

		public void Quit () 
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}
}
