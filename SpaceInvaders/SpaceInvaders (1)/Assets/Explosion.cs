using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour      //Should this be a seperate script?
{
    private float timer = 0f;
    private int growFactor = 10;
    private int damage = 75;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")   //Kill enemy on collision               
        {
            Destroy(other.gameObject);
            ScoreManager.score += 1;
        }
        if (other.tag == "Boss")
        {
            HealthStats hs = other.GetComponent<HealthStats>();
            if (hs.currentHealth > damage)
            {
                hs.ChangeHealth(-75);       //deal damage if they can take it
            }
            else
            {
                Destroy(other.gameObject);  //otherwise destroy     (should destroy itself on healthstats if it has 0 or less health, only killed by lasers atm)
                ScoreManager.score += 10;
            }
            //doesn't kill, only sets health down
        }

    }

    void Update()
    {
        transform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growFactor;

        timer += Time.deltaTime;        //Destroys itself after explosion clip ends     (can use clip.length by getcomponent instead of hard coded number)
        if (timer >= 2.5f)
        {
            Destroy(gameObject);
        }
    }


}
