using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScore : MonoBehaviour
{
	public TextMeshProUGUI BestTime;
	public TextMeshProUGUI LastTime;
	public TextMeshProUGUI NewBestTime;

	public float Best
	{
		get => PlayerPrefs.GetFloat("BestTime", -1);
		set => PlayerPrefs.SetFloat("BestTime", value);
	}

	public void Show(double time)
	{
		var last = (float)time;
		var best = Best;

		NewBestTime.enabled = last < best;

		if (best == -1 || last < best)
			Best = last;

		BestTime.text = best.ToString();
		LastTime.text = last.ToString();

		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}

	public void RestartLevel()
	{
		// ToDo : Proper death screen
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
