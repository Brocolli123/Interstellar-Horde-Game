using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float fallspeed = 4f;
    //change gamemanager increase score to take in a string of the score
    //public int score

    // Start is called before the first frame update
    void Start()
    {
        fallspeed = Random.Range(2f,5f);
}

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * fallspeed * Time.deltaTime);
        transform.Rotate(0, -2, 0);
    }

    private void OnTriggerEnter(Collider other)     //Not entering here?    //To make it work can shoot the coin in laser
    {   
        if(other.tag == "Player")
        {
            GameManager.IncreaseCoin();
            Destroy(gameObject);
        }
    }
}
