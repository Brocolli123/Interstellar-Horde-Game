using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    static private WaveManager instance = null; 

    [SerializeField] private List<GameObject> enemyWaves;           //static if need to be accessed by any other script, public for use on buttons
    [SerializeField] private GameObject midWaveScreen;
    public int waveCounter = 0;
    private EnemyMovement[] enemies;
    private int waveScore = 0;          //player score at the start of the current wave, for use in retrying
    private GameObject _instance;
    public TextMeshProUGUI waveDisplay;
    public TextMeshProUGUI waveOver;
    public GameObject waveDisplay_;
    private GameObject player;
    private Vector3 originalTrans;

    private void Awake()
    {
        Debug.Assert(instance == null);
        instance = this;
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        enemies = enemyWaves[waveCounter].GetComponentsInChildren<EnemyMovement>();     //Gets all enemies in first wave
        SpawnWave();
        waveDisplay_.SetActive(true);
        waveDisplay.text = "WAVE "+waveCounter+"/9";
        originalTrans = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }   

    void Update()
    {
        if (_instance.transform.childCount == 0)      
        {
            Destroy(_instance.gameObject);      //Get rid of current instance when no longer in use
            Debug.Log("No Enemies Left");
            waveDisplay_.SetActive(false);
            if (waveCounter >= enemyWaves.Count)
                GameManager.PlayerWon();
            else
            {
                midWaveScreen.SetActive(true);
                waveOver.text = "WAVE "+waveCounter+" COMPLETE";
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            Time.timeScale = 0;
        }
    }

    public void SpawnWave()         //Called by button on midWaveScreen  
    {
        player.transform.position = originalTrans;
        Debug.Log("Spawning wave");
        waveScore = ScoreManager.score;
        Time.timeScale = 1;         //???
        midWaveScreen.SetActive(false);     //Deactivate MidWave Screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _instance = Instantiate(enemyWaves[waveCounter], transform.position, transform.rotation);   //Spawns the current wave
        ++waveCounter;  //Increment Wave Counter
        waveDisplay_.SetActive(true);
        waveDisplay.text = "WAVE "+waveCounter+"/9";
        Debug.Log("Wave spawned");
    }

    public void RetryWave()
    {
        GameObject.Destroy(_instance);                                              //Destroy all currently spawned enemies (these are stored in _instance when spawned)
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");           //Destroy all player lasers
        foreach (GameObject laser in lasers)
            GameObject.Destroy(laser);
        GameObject[] eLasers = GameObject.FindGameObjectsWithTag("Enemy Laser");    //Destroy all enemy lasers
        foreach (GameObject laser in eLasers)
            GameObject.Destroy(laser);
        GameObject[] bLasers = GameObject.FindGameObjectsWithTag("Boss Laser");     //Destroy all boss lasers
        foreach (GameObject laser in bLasers)
            GameObject.Destroy(laser);

        ScoreManager.score = waveScore;     //Reset score to what it was at the start of this wave (so player cannot restart wave and keep their score)
        waveCounter--;                      //SpawnWave() increments waveCounter so it needs to be decremented before retrying
        SpawnWave();
    }
}
