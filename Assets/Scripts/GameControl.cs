using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameControl : MonoBehaviour
{
    public static GameControl gameControl;
    [SerializeField] Text scoreText,hightScoreText, GOVscoreText;
    [Tooltip("Lives of Player when start game")]
    [SerializeField] int startLives;
    [SerializeField] MainMenu mainMenu;
    [SerializeField] SpaceShip spaceShip;
    [SerializeField] LivesDisplay livesDisplay;
    [SerializeField] EnemyManagement enemyManagement;
    [Tooltip("Ready/Game over display Time before change to another scence.")]
    [SerializeField] float waitTime=3f;
    [SerializeField] GameObject MainMemuCanvas, Gameplay, GameplayCanvas, ReadyText,GameOverText,GameOverCanvas;
    public static int liveCount;
    public int score = 0;
    public int hightScore = 0;
    public enum GameState
    {
        MENU,GAMEPLAY,GAMEOVER
    }
    public GameState gameState;
    /// <summary>
    /// set gamecontro to global value. For every object can access it.
    /// </summary>
    private void Awake()
    {
        gameControl = this;

    }
    void Start()
    {
        gameState = GameState.MENU;
        LoadHightScore();
    }

    // Update is called once per frame
    void Update()
    {
       switch(gameState)
        {
            case GameState.MENU:
                if (Input.GetKeyDown("up"))
                {
                    mainMenu.chageMenuIndex(-1);//select upper menu
                }

                if (Input.GetKeyDown("down"))
                {
                    mainMenu.chageMenuIndex(1);//select lower menu
                }

                if (Input.GetKeyDown("space"))
                {
                    mainMenu.InvokeSelectedEvent();
                }
                break;
            case GameState.GAMEPLAY:
                break;
            case GameState.GAMEOVER:
                if (Input.GetKeyDown("space"))
                {
                    Restart();
                }
                break;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void SaveHightScore(int newscore)
    {
        HighScore hs = new HighScore();
        hs.highScore = newscore;

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/HighScore.save");
       
        bf.Serialize(file, hs);
        file.Close();
        Debug.Log(hs.highScore);

    }
    public void LoadHightScore()
    {
        if (File.Exists(Application.persistentDataPath + "/HighScore.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/HighScore.save", FileMode.Open);
            HighScore hs = (HighScore)bf.Deserialize(file);
            hightScore = hs.highScore;
            file.Close();
            Debug.Log("Loaded");
        }
        else
        {
            hightScore = 0;
            Debug.Log("save not found");
        }
    }

    /// <summary>
    ///Enter GAMEPLAY State and show READY Text for notifying to the player game will start soon.
    /// </summary>
    public void StartGame()
    {
        gameState = GameState.GAMEPLAY;
        MainMemuCanvas.SetActive(false);
        ReadyText.SetActive(true);
        GameplayCanvas.SetActive(true);
        score = 0; 
        scoreText.text = "0";
        hightScoreText.text = hightScore.ToString();
        liveCount = startLives;
        livesDisplay.IncreasLive(liveCount);//display amount of player's lives on screen
        Invoke("StartGamePlay", waitTime);//start game in waitTime secondes 
        
    }


    /// <summary>
    /// start game play after wait 
    /// </summary>
    public void StartGamePlay()
    {
        ReadyText.SetActive(false);
        Gameplay.SetActive(true);
        spaceShip.ReSpawn();
        enemyManagement.StartEnemyManagement();
    }


    /// <summary>
    ///Gameover Text for notifying and wait for waitTime to change Scence
    /// </summary>
    public void GameOver()
    {
        
        GameOverText.SetActive(true);
        enemyManagement.ClearEnemy();
        Gameplay.SetActive(false);
        Invoke("ShowGameOverCanvas", waitTime);// scence will change in waitTime seconds
    }

    /// <summary>
    ///Show gameover scence 
    /// </summary>
    public void ShowGameOverCanvas()
    {
        if(score > hightScore)
        {
            SaveHightScore(score);
            hightScore = score;
        }
        gameState = GameState.GAMEOVER;
        GameplayCanvas.SetActive(false);
        GameOverText.SetActive(false);
        GameOverCanvas.SetActive(true);

        GOVscoreText.text= score.ToString();

    }

    

    public void Restart()
    {
        GameOverCanvas.SetActive(false);
        StartGame();
    }



    /// <summary>
    /// Decrease player's live by 1
    /// </summary>
    public void DecreaseLive()
    {

        if (liveCount == 0)
        {
            GameOver();
        }
        else if (liveCount > 0)
        {
            liveCount--;
            livesDisplay.DecreasLive(1);
        }

    }



    /// <summary>
    /// Increase Score by Paramiter s
    /// </summary>
    /// <param name="s"> 
    /// score increase amount 
    /// </param>
    public void IncreaseScore(int s)
    {
        if (s < 0) return;
        score += s;

        scoreText.text = score.ToString();
    }
}
