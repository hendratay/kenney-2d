using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void StartGame()
	{
		SceneManager.LoadScene("Town");
	}

	public void StartMultiplayer()
	{
		SceneManager.LoadScene("BattleGround");
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}

}
