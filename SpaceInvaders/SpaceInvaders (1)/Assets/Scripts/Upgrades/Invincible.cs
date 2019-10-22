using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincible : MonoBehaviour
{
    public float fallspeed = 4f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * fallspeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            ShipControl sc = other.gameObject.GetComponent<ShipControl>();       //Get Player Ship Script
            GameManager.isInvincible = true; //set back false after?
            Destroy(gameObject);                //Destroy this
        }
    }

}
