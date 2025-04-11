using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    public GameObject victoryScreen;
    public GameObject gameOverScreen;
    public int AvailableLives = 3;

    public int lives { get; set; }

    public bool IsGameStarted { get; set; }

    private void Start()
    {
        this.lives = this.AvailableLives;
        Screen.SetResolution(540, 960, false);
        Ball.OnBallDeath += OnBallDeath;
        Brick.OnBrickDestruction += OnBrickDestruction;
    }

    private void OnBrickDestruction(Brick obj)
    {
        if (BricksManager.Instance.RemainingBricks.Count <= 0)
        {
            BallsManager.Instance.ResetBalls();
            GameManager.Instance.IsGameStarted = false;
            BricksManager.Instance.LoadNextLevel();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBallDeath(Ball obj)
    {
        if (BallsManager.Instance.Balls.Count <= 0)
        {
            this.lives--;

            if (this.lives < 1)
            {
                gameOverScreen.SetActive(true);
            }
            else
            {

                BallsManager.Instance.ResetBalls();
                IsGameStarted = false;

                BricksManager.Instance.LoadLevel(BricksManager.Instance.CurrentLevel);
            }
        }
    }
    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
    private void OnDisable()
    {
       Ball.OnBallDeath -= OnBallDeath;
    }
}
