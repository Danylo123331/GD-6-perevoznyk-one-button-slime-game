using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{

    public GameObject[] obstacles;
    public GameObject mainMenuUI;
    public GameObject gameplayUI;
    public GameObject pauseButton;
    public GameObject scoreText;
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;
    public GameObject player;
    public GameObject statisticsUI;
    public GameObject settingsUI;

    public GameObject gameOverScore;
    public GameObject gameOverBestScore;

    public GameObject lastScoreText;
    public GameObject bestScoreText;
    public GameObject gamesPlayedText;
    public GameObject colorChangeText;

    public GameObject soundOff;
    public GameObject musicOff;

    private AudioSource buttonClick;

    void Awake()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 300;
        if (PlayerPrefs.GetInt("restartTheGame") == 1)
        {
            PlayerPrefs.SetInt("restartTheGame", 0);
            GameStart();
        }
    }

    void Start()
    {
        GameObject btnClickObj = GameObject.Find("buttonClickSound");
        if (btnClickObj != null) buttonClick = btnClickObj.GetComponent<AudioSource>();

        GameObject expObj = GameObject.Find("explosionSound");
        GameObject sucObj = GameObject.Find("successSound");
        GameObject musObj = GameObject.Find("music");

        if (PlayerPrefs.GetInt("soundOff") == 1)
        {
            if (expObj != null && expObj.GetComponent<AudioSource>() != null) expObj.GetComponent<AudioSource>().mute = true;
            if (sucObj != null && sucObj.GetComponent<AudioSource>() != null) sucObj.GetComponent<AudioSource>().mute = true;
            if (btnClickObj != null && btnClickObj.GetComponent<AudioSource>() != null) btnClickObj.GetComponent<AudioSource>().mute = true;
            if (soundOff != null) soundOff.SetActive(true);
        }
        else
        {
            if (expObj != null && expObj.GetComponent<AudioSource>() != null) expObj.GetComponent<AudioSource>().mute = false;
            if (sucObj != null && sucObj.GetComponent<AudioSource>() != null) sucObj.GetComponent<AudioSource>().mute = false;
            if (btnClickObj != null && btnClickObj.GetComponent<AudioSource>() != null) btnClickObj.GetComponent<AudioSource>().mute = false;
            if (soundOff != null) soundOff.SetActive(false);
        }

        if (PlayerPrefs.GetInt("musicOff") == 1)
        {
            if (musObj != null && musObj.GetComponent<AudioSource>() != null) musObj.GetComponent<AudioSource>().mute = true;
            if (musicOff != null) musicOff.SetActive(true);
        }
        else
        {
            if (musObj != null && musObj.GetComponent<AudioSource>() != null) musObj.GetComponent<AudioSource>().mute = false;
            if (musicOff != null) musicOff.SetActive(false);
        }
    }

    public void GameStart()
    {
        GameObject musObj = GameObject.Find("music");
        if (musObj != null && musObj.GetComponent<AudioSource>() != null) musObj.GetComponent<AudioSource>().pitch = 1;

        PlayerPrefs.SetInt("gamesPlayed", PlayerPrefs.GetInt("gamesPlayed") + 1);
        PlayerPrefs.SetInt("lastScore", 0);
        PlayerLogic.collision = 0;

        if (obstacles != null)
        {
            foreach (GameObject obstacle in obstacles)
            {
                if (obstacle != null && obstacle.GetComponent<Obstacle>() != null)
                {
                    obstacle.GetComponent<Obstacle>().enabled = true;
                }
            }
        }

        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(true);

        if (player != null && player.GetComponent<PlayerLogic>() != null)
        {
            player.GetComponent<PlayerLogic>().enabled = true;
        }

        if (buttonClick == null)
        {
            GameObject btnClickObj = GameObject.Find("buttonClickSound");
            if (btnClickObj != null) buttonClick = btnClickObj.GetComponent<AudioSource>();
        }
        if (buttonClick != null) buttonClick.Play();
    }

    public void GameContinue()
    {
        Time.timeScale = 1;
        if (buttonClick != null) buttonClick.Play();
        PlayerLogic.collision++;

        GameObject exp = GameObject.Find("explosion");
        if (exp != null) Destroy(exp);

        GameObject cam = GameObject.Find("Main Camera");
        if (cam != null) Destroy(cam);

        if (gameOverMenuUI != null) gameOverMenuUI.SetActive(false);
        if (gameplayUI != null) gameplayUI.SetActive(true);

        GameObject playerObj = Instantiate(Resources.Load("player") as GameObject) as GameObject;
        if (playerObj != null)
        {
            playerObj.transform.localPosition = new Vector3(0, 0.51f, (PlayerPrefs.GetInt("lastScore") + PlayerLogic.collision - 1) * 100 + 80);
            playerObj.name = "player";
        }

        if (pauseButton != null) pauseButton.SetActive(true);
        if (scoreText != null) scoreText.SetActive(true);
    }

    public void OpenStatisticsMenu()
    {
        if (lastScoreText != null && lastScoreText.GetComponent<Text>() != null) lastScoreText.GetComponent<Text>().text = "LAST SCORE: " + PlayerPrefs.GetInt("lastScore", 0);
        if (bestScoreText != null && bestScoreText.GetComponent<Text>() != null) bestScoreText.GetComponent<Text>().text = "BEST SCORE: " + PlayerPrefs.GetInt("bestScore", 0);
        if (gamesPlayedText != null && gamesPlayedText.GetComponent<Text>() != null) gamesPlayedText.GetComponent<Text>().text = "GAMES PLAYED: " + PlayerPrefs.GetInt("gamesPlayed", 0);
        if (colorChangeText != null && colorChangeText.GetComponent<Text>() != null) colorChangeText.GetComponent<Text>().text = "SHAPES FITTED: " + PlayerPrefs.GetInt("shapeFitted", 0);

        if (statisticsUI != null) statisticsUI.SetActive(true);
        if (buttonClick != null) buttonClick.Play();
    }

    public void CloseStatisticsMenu()
    {
        if (statisticsUI != null) statisticsUI.SetActive(false);
        if (buttonClick != null) buttonClick.Play();
    }

    public void TurnOnOffSound()
    {
        GameObject expObj = GameObject.Find("explosionSound");
        GameObject sucObj = GameObject.Find("successSound");
        GameObject btnClickObj = GameObject.Find("buttonClickSound");

        if (PlayerPrefs.GetInt("soundOff") == 1)
        {
            PlayerPrefs.SetInt("soundOff", 0);
            if (expObj != null && expObj.GetComponent<AudioSource>() != null) expObj.GetComponent<AudioSource>().mute = false;
            if (sucObj != null && sucObj.GetComponent<AudioSource>() != null) sucObj.GetComponent<AudioSource>().mute = false;
            if (btnClickObj != null && btnClickObj.GetComponent<AudioSource>() != null) btnClickObj.GetComponent<AudioSource>().mute = false;
            if (soundOff != null) soundOff.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("soundOff", 1);
            if (expObj != null && expObj.GetComponent<AudioSource>() != null) expObj.GetComponent<AudioSource>().mute = true;
            if (sucObj != null && sucObj.GetComponent<AudioSource>() != null) sucObj.GetComponent<AudioSource>().mute = true;
            if (btnClickObj != null && btnClickObj.GetComponent<AudioSource>() != null) btnClickObj.GetComponent<AudioSource>().mute = true;
            if (soundOff != null) soundOff.SetActive(true);
        }
        if (buttonClick != null) buttonClick.Play();
    }

    public void TurnOnOffMusic()
    {
        GameObject musObj = GameObject.Find("music");

        if (PlayerPrefs.GetInt("musicOff") == 1)
        {
            PlayerPrefs.SetInt("musicOff", 0);
            if (musObj != null && musObj.GetComponent<AudioSource>() != null) musObj.GetComponent<AudioSource>().mute = false;
            if (musicOff != null) musicOff.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("musicOff", 1);
            if (musObj != null && musObj.GetComponent<AudioSource>() != null) musObj.GetComponent<AudioSource>().mute = true;
            if (musicOff != null) musicOff.SetActive(true);
        }
        if (buttonClick != null) buttonClick.Play();
    }

    public void OpenSettingsMenu()
    {
        if (settingsUI != null) settingsUI.SetActive(true);

        GameObject sliderObj = GameObject.Find("sensitivitySlider");
        if (sliderObj != null && sliderObj.GetComponent<Slider>() != null)
        {
            sliderObj.GetComponent<Slider>().value = PlayerPrefs.GetFloat("sensitivity", 7);
        }

        GameObject expObj = GameObject.Find("explosionSound");
        if (expObj != null && expObj.GetComponent<AudioSource>() != null && expObj.GetComponent<AudioSource>().mute == true)
        {
            if (soundOff != null) soundOff.SetActive(true);
        }

        GameObject musObj = GameObject.Find("music");
        if (musObj != null && musObj.GetComponent<AudioSource>() != null && musObj.GetComponent<AudioSource>().mute == true)
        {
            if (musicOff != null) musicOff.SetActive(true);
        }

        if (buttonClick != null) buttonClick.Play();
    }

    public void ChangeSensitivity()
    {
        GameObject sliderObj = GameObject.Find("sensitivitySlider");
        if (sliderObj != null && sliderObj.GetComponent<Slider>() != null)
        {
            PlayerPrefs.SetFloat("sensitivity", sliderObj.GetComponent<Slider>().value);
        }
    }

    public void CloseSettingsMenu()
    {
        if (settingsUI != null) settingsUI.SetActive(false);
        if (buttonClick != null) buttonClick.Play();
    }

    public void PauseMenu()
    {
        GameObject playerObj = GameObject.Find("player");
        if (playerObj != null && playerObj.GetComponent<BoxCollider>() != null)
        {
            playerObj.GetComponent<BoxCollider>().enabled = false;
        }
        Time.timeScale = 0;
        if (pauseButton != null) pauseButton.SetActive(false);
        if (scoreText != null) scoreText.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (buttonClick != null) buttonClick.Play();
    }

    public void GameOver()
    {
        if (pauseButton != null) pauseButton.SetActive(false);
        if (scoreText != null) scoreText.SetActive(false);
        Invoke("GameOverMenu", 1.5f);
    }

    public void GameOverMenu()
    {
        if (pauseButton != null) pauseButton.SetActive(false);
        if (scoreText != null) scoreText.SetActive(false);
        if (gameOverMenuUI != null) gameOverMenuUI.SetActive(true);

        if (gameOverScore != null && gameOverScore.GetComponent<Text>() != null)
        {
            gameOverScore.GetComponent<Text>().text = "SCORE: " + PlayerPrefs.GetInt("lastScore");
        }
        if (gameOverBestScore != null && gameOverBestScore.GetComponent<Text>() != null)
        {
            gameOverBestScore.GetComponent<Text>().text = "BEST SCORE: " + PlayerPrefs.GetInt("bestScore");
        }
    }

    public void Resume()
    {
        GameObject playerObj = GameObject.Find("player");
        if (playerObj != null && playerObj.GetComponent<BoxCollider>() != null)
        {
            playerObj.GetComponent<BoxCollider>().enabled = true;
        }
        Time.timeScale = 1;
        if (pauseButton != null) pauseButton.SetActive(true);
        if (scoreText != null) scoreText.SetActive(true);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (buttonClick != null) buttonClick.Play();
    }

    public void ExitToMainMenu()
    {
        if (buttonClick != null) buttonClick.Play();
        GameObject musObj = GameObject.Find("music");
        if (musObj != null && musObj.GetComponent<AudioSource>() != null) musObj.GetComponent<AudioSource>().pitch = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RestartGame()
    {
        PlayerPrefs.SetInt("restartTheGame", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitTheGame()
    {
        if (buttonClick != null) buttonClick.Play();
        Application.Quit();
    }
}