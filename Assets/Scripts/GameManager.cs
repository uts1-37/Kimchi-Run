using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Intro,
    Playing,
    Dead
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 
    public GameState State = GameState.Intro;

    public float PlayStartTime;

    public int Lives = 3;

    [Header("References")]
    public GameObject IntroUI;

    public GameObject DeadUI;
    public GameObject EnemySpawner;
    public GameObject FoodSpawner;
    public GameObject GoldenSpawner;
    
    public TMP_Text ScoreText;

    public Player Playerscript; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
    }
    void Start()
    {
        IntroUI.SetActive(true);
    }
    float CalculateScore()
    {
        return Time.time - PlayStartTime;
    }

    void SaveHighScore()
    {
        int score = Mathf.FloorToInt(CalculateScore());
        int CurrentHighScore = PlayerPrefs.GetInt("highScore");

        if (score > CurrentHighScore)
        {
            PlayerPrefs.SetInt("highScore", score);
            PlayerPrefs.Save();
        }
    }
    int GetHighScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }
    public float CalculateGameSpeed()
    {
        if (State != GameState.Playing)
        {
            return 3f;
        }
        float speed = 3f + (0.5f * Mathf.Floor(CalculateScore() / 10f));
        return Mathf.Min(speed , 20f);
    }
    // Update is called once per frame
    void Update()
    {
        if (State == GameState.Playing)
        {
            ScoreText.text = "Score: " + Mathf.FloorToInt(CalculateScore());
        }else if (State == GameState.Dead)
        {
            ScoreText.text = "High Score: " + GetHighScore();
        }
        if (State == GameState.Intro && Input.GetKeyDown(KeyCode.Space))
        {
            State = GameState.Playing;
            IntroUI.SetActive(false);
            EnemySpawner.SetActive(true);
            FoodSpawner.SetActive(true);
            GoldenSpawner.SetActive(true);
            PlayStartTime = Time.time;
        }
        if (State == GameState.Playing && Lives == 0)
        {
            Playerscript.KillPlayer();
            SaveHighScore(); 
            IntroUI.SetActive(false);
            EnemySpawner.SetActive(false);
            FoodSpawner.SetActive(false);
            GoldenSpawner.SetActive(false);


            State = GameState.Dead;
            DeadUI.SetActive(true);
        }
        if (State == GameState.Dead && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("main");
        }
    }
}
