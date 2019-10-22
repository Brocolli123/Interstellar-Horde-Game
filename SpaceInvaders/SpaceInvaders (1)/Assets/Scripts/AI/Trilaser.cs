using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trilaser : MonoBehaviour
{
    private Rigidbody _rb;
    public int speed = 350;
    public int damage = 1;
    public GameObject explosion;
    public string[] ignore;
    private bool okToFire = false;
    public float angle;     //For which angle 

    public GameObject coin;
    int coinChance;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Assert(explosion != null);
    }

    void Start()
    {
        _rb.velocity = new Vector3(angle/32, speed * Time.deltaTime, 0);
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
        //GameObject fire = (GameObject)Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity);
        //Destroy(gameObject);

        if (other.tag != "Laser")
        {
            GameObject fire = (GameObject)Instantiate(explosion, other.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (other.tag == "Boss")
        {
            stats.ChangeHealth(-5);
            ScoreManager.IncreaseScore();
        }
        if (other.tag == "Player")
        {
            stats.ChangeHealth(-5);
            if (stats.currentHealth <= 0)
            {
                GameManager.PlayerDied();
            }
        }
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);

            ScoreManager.IncreaseScore();
            coinChance = Random.Range(0, 10);    //Change the chance to lower? Is this truly random?
            if (coinChance == 5)
            {
                Instantiate(coin, other.gameObject.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        if (other.tag == "Boss" && stats.currentHealth <= 0)

        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            coinChance = Random.Range(0, 10);    //Change the chance to lower? Is this truly random?
            if (coinChance == 5)
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
