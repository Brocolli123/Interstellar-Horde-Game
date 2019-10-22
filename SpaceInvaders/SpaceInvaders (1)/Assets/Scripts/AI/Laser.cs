using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laser : MonoBehaviour
{
    private Rigidbody _rb;
    public int speed = 350;
    public int damage = 5;
    public GameObject explosion;
    public string[] ignore;
    private bool okToFire = false;

    public GameObject coin;
    public GameObject healthDrop;
    int coinChance;

    public AudioSource audioSource;
    public AudioClip enemyExplode;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Assert(explosion != null);
    }

    void Start()
    {
        _rb.velocity = new Vector3(0, speed * Time.deltaTime,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in ignore)
        {
            if (other.tag == tag)
            {
                return;
            }
        }
        HealthStats stats = other.gameObject.GetComponent<HealthStats>();
        GameObject fire = (GameObject)Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity);
        Destroy(fire, 2f);
        Destroy(gameObject);

        if (other.tag == "Enemy Laser")
        {
            Destroy(other.gameObject);
        }
        if (other.tag == "Boss")
        {
            if (stats.currentHealth <= damage)
            {
                Destroy(other.gameObject);
                ScoreManager.IncreaseScore();   //change to take in a number
                ScoreManager.IncreaseScore();
                ScoreManager.IncreaseScore();
            }
            stats.ChangeHealth(-damage);
        }
        if (other.tag == "Player")
        {
            stats.ChangeHealth(-5);
            audioSource.PlayOneShot(enemyExplode, 1F);
            if (stats.currentHealth <= 0)
            {
                GameManager.PlayerDied();
            }
        }
        if(other.tag == "Enemy")
        {
            audioSource.PlayOneShot(enemyExplode, 1F);
            Destroy(other.gameObject);

            ScoreManager.IncreaseScore();
            coinChance = Random.Range(0, 10);    //Change the chance to lower? Is this truly random?
            if (coinChance > 7)
            {
                Instantiate(coin, other.gameObject.transform.position, Quaternion.identity);
            }
            if (coinChance == 2)
            {
                Instantiate(healthDrop, other.gameObject.transform.position, Quaternion.identity);
            }   
            Destroy(gameObject);
        }
        if (other.tag == "Boss" && stats.currentHealth <= 0)
            
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            coinChance = Random.Range(0, 10);    //Change the chance to lower? Is this truly random?
            if (coinChance > 1)
            {
                Instantiate(coin, other.gameObject.transform.position, Quaternion.identity);
            }
        }
        
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
