using System.Runtime.Serialization.Formatters.Binary;   //for binary formatting
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    static private GameManager instance = null;

    static string path = "./saveGame.gd";      

    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject retryScreen;
    public GameObject shopScreen;
    private GameObject player;
    private WaveManager waveManager;
    public GameObject m_FadeImage;
    public GameObject waveDisplay;

    public TextMeshProUGUI livesCount;
    static bool isPlayerDead;
    static bool playerWon;

    public AudioSource audioSource;
    public AudioClip backgroundMusic;

    public TextMeshProUGUI bombText;        //serializefield private?
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI invincibleText;
    public TextMeshProUGUI winScoreText;
    public TextMeshProUGUI winHighScoreText;
    public TextMeshProUGUI winTimeText;
    public TextMeshProUGUI shopCoins;

    public static bool isInvincible;        //Power up Variables
    public static int[] powerUpList = new int[3]{ 1, 2, 3 };       //Pos 0 = Bomb, Pos 1 = Invincible, Pos 2 = SpeedUp 
    private int speedCost = 1;
    private int explodeCost = 2;
    private int invincibleCost = 5;
    private int healthCost = 5;
    private int lifeCost = 10;
    private int damageCost = 5;
    private int fireRateCost = 5;
    private int healthUpgradeCost = 5;
    public GameObject growExplosion;
    private ShipControl sc;
    [SerializeField] private int speedUpMult = 2;
    private float playerHealth;
    public static bool canPowerup = true;

    public Text displayText;

    public TextMeshProUGUI coinCount;      //manage coins in scoremanager?
    static int coingot = 0;

    public int playerLives = 1;
    float timer = 0f;

    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    void Start()
    {
        audioSource.loop = true;
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        playerWon = false;
        isPlayerDead = false;
        coinCount.text = "";
        Time.timeScale = 1;
        player = GameObject.FindWithTag("Player");
        waveManager = GetComponent<WaveManager>();
        sc = player.GetComponent<ShipControl>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        coinCount.text = "Coins: " + coingot;   
        bombText.text = powerUpList[0].ToString();
        invincibleText.text = powerUpList[1].ToString();
        speedText.text = powerUpList[2].ToString();

        if (shopScreen.activeSelf)
            shopCoins.text = "Coins: " + coingot;


        if (Input.GetKeyDown("q") && powerUpList[0] > 0 && canPowerup)      //Explosion       Have an else to make a beep sound if the user doesn't have enough powerups to use (nested if instead of OR), audiosource if one is used
        {
            Instantiate(growExplosion, player.transform.position, player.transform.rotation);       //Make the object BOI
            powerUpList[0]--;
        }
        if (Input.GetKeyDown("e") && powerUpList[1] > 0 && !isInvincible && canPowerup)        //Invincible
        {
            isInvincible = true;
            powerUpList[1]--;
            playerHealth = player.GetComponent<HealthStats>().currentHealth;
        }
        if (Input.GetKeyDown("r") && powerUpList[2] > 0 && canPowerup)        //SpeedUp
        {
            powerUpList[2]--;
            sc.ChangeMoveSpeed();    //Multiplies speed for a few seconds               TIME WORKS BUT FREEZES PLAYER
        }

        if (isInvincible)
        {
            player.GetComponent<HealthStats>().currentHealth = playerHealth;        //keeps player health the same as before invincibility
        }

        if (playerWon == true)
        {
           winScreen.SetActive(true);
           winScoreText.text = "Score: " + ScoreManager.GetScore();
           winHighScoreText.text = "High score: " + ScoreManager.GetHighScore();
           int minutes = ((int)Time.timeSinceLevelLoad) / 60;
           int seconds = ((int)Time.timeSinceLevelLoad) % 60;
            winTimeText.text = "Time taken: " + minutes+":"+seconds;
           Cursor.visible = true;
           Cursor.lockState = CursorLockMode.None;
        }

        if (isPlayerDead == true)
        {
            if (playerLives > 0)                //if player has more lives, show the retry screen
            {
                Time.timeScale = 0;
                retryScreen.SetActive(true);
                livesCount.text = "REMAINING LIVES: " + playerLives;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
                loseScreen.SetActive(true);     //otherwise, show the game over screen
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
        }


        if (isInvincible == true)   //If invincible, set a timer before turning it off
        {
            timer += Time.deltaTime;
            if (timer >= 10f)
            {
                isInvincible = false;
                timer = 0f;
            }
        }
    }

    public void BuySpeedUp()
    {
        //store cost as variable? Pass as parameter?
        if (coingot >= speedCost)       //change to variable
        {
            coingot -= speedCost;
            powerUpList[2] += 1;
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public void BuyExplosion()
    {
        if (coingot >= explodeCost)
        {
            coingot -= explodeCost;
            powerUpList[0] += 1;

        } else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public void BuyInvincible()
    {
        if (coingot >= invincibleCost)
        {
            coingot -= invincibleCost;
            powerUpList[1] += 1;
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public void BuyLife()
    {
        if (coingot >= lifeCost)
        {
            coingot -= lifeCost;
            playerLives++;
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }        
    }

    public void BuyHealth()
    {
        if (coingot >= healthCost)
        {
            coingot -= healthCost;
            playerLives++;
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public void BuyFireRate()
    {
        if (coingot >= fireRateCost)
        {
            coingot -= fireRateCost;
            sc.IncreaseFireRate();
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public void BuyDamage()
    {
        if (coingot >= damageCost)
        {
            coingot -= damageCost;
            sc.damage = (sc.damage * 1.5f);
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public void BuyHealthUpgrade()
    {
        if (coingot >= healthUpgradeCost)
        {
            coingot -= healthUpgradeCost;
            player.GetComponent<HealthStats>().maxHealth = (player.GetComponent<HealthStats>().maxHealth*1.5f);
            player.GetComponent<HealthStats>().currentHealth = (player.GetComponent<HealthStats>().currentHealth*1.5f);
        }
        else
        {
            Debug.Log("Can't afford this");     //Maybe have pop up window here
        }
    }

    public static void PlayerDied()     //check invincible and turn off after timer
    {
        if (isInvincible == false)      //change later based on damage not isPlayerDead
        {
            isPlayerDead = true;
            SaveData.current.highScore = Mathf.Max(SaveData.current.highScore, ScoreManager.score);
            Save(); //saves score

            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Debug.Log("invincible");
        }
    }

    public static void PlayerWon()
    {
        playerWon = true;
        SaveData.current.highScore = Mathf.Max(SaveData.current.highScore, ScoreManager.score); 
        Save(); //saves score
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public static void PlayerDead(bool value)     //set player dead
    {
        isPlayerDead = value;
    }

    public void RestartGame()           
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game"); 
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        isPlayerDead = false;
        SaveData.current.highScore = Mathf.Max(SaveData.current.highScore, ScoreManager.score); //sets highscore to current or players score (highest between them)     (change second to scoremanager.score when it is changed over)
        Save(); //saves score
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Retry()
    {
        playerLives--;
        Time.timeScale = 1;
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        retryScreen.SetActive(false);
        isPlayerDead = false;
        player.GetComponent<HealthStats>().currentHealth = player.GetComponent<HealthStats>().maxHealth;
        waveManager.RetryWave();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void IncreaseCoin()       //Change to take score as a parameter
    {
        coingot += 1;
    }

    public static void Save()
    {
        SaveData.current.highScore = Mathf.Max(SaveData.current.highScore, ScoreManager.score);
        BinaryFormatter bf = new BinaryFormatter(); //formats text to binary
        FileStream file = File.Create(path);    //makes new file at path
        bf.Serialize(file, SaveData.current);   //serialises file with savedata
        file.Close();   //closes and creates file
    }

    public static void Load()
    {
        if (File.Exists(path))      //if the file exists at path
        {
            BinaryFormatter bf = new BinaryFormatter(); //formats to binary
            FileStream file = File.Open(path, FileMode.Open);   //opens file

            SaveData.current = (SaveData)bf.Deserialize(file);  //reads the binary file, casts savedata as a binaryformatter
            file.Close();   //finishes reading file
        } else
        {
            SaveData.current = new SaveData();  //creates a file if none exists
        }
    }

    public void DisplaySavedText()
    {
        displayText.text = "GAME SAVED!";
    }
}
