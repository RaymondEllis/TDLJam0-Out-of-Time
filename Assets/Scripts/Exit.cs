using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
	private void Update()
	{
		// Exit or stops playing
		if (Input.GetButtonDown("Exit"))
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		else if (Input.GetButtonDown("Restart"))
			// ToDo : Proper death screen
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
