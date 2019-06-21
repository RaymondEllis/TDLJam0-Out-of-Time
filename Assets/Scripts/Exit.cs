using UnityEngine;

public class Exit : MonoBehaviour
{
	private void Update()
	{
		// Escape exits or stops playing
		if (Input.GetKeyDown(KeyCode.Escape))
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}
}
