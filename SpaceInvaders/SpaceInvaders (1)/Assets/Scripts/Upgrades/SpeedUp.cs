using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float fallspeed = 4f;
    public int speedUpMult = 5;

    void Update()
    {
        transform.Translate(Vector3.down * fallspeed * Time.deltaTime);
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            ShipControl sc = other.gameObject.GetComponent<ShipControl>();       //Get Player Ship Script
            sc.ChangeMoveSpeed();        //Call Speedup with the speedUp multiplier on this
            Destroy(gameObject);                //Destroy this
        }
    }

}