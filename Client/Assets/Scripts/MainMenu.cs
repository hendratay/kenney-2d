using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartGame()
	{
		SceneManager.LoadScene("Town");
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}

}
