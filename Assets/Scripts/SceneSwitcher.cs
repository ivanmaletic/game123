using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	[SerializeField] GameObject _pauseMenuUI;
	[SerializeField] GameObject _gameOverMenuUI;

	public bool gameIsPaused;
	public void PlayGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	}
	public void Instructions()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
	}
	public void MainMenu()
	{
		SceneManager.LoadScene("mainMenu");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void Pause()
	{
		_pauseMenuUI.SetActive(true);
		GameObject.Find("Coin1").GetComponent<GameplayScript>().enabled = false;
		GameObject.Find("Coin1").GetComponent<GameplayScript>().EndLine(GameObject.Find("Coin1").GetComponent<GameplayScript>().lr);
		gameIsPaused = true;
	}
	public void Resume()
	{
		_pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameObject.Find("Coin1").GetComponent<GameplayScript>().enabled = true;
		gameIsPaused = false;
	}
	public void Restart()
	{
		_pauseMenuUI.SetActive(false);
		_gameOverMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameObject.Find("Coin1").GetComponent<GameplayScript>().enabled = true;
		gameIsPaused = false;
		SceneManager.LoadScene("gameplayScene");
	}
}
