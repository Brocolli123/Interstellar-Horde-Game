using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    private float fallspeed = 4f;

    void Update()
    {
        transform.Translate(Vector3.down * fallspeed * Time.deltaTime);         //fall over time
    }

    private void OnTriggerEnter(Collider other)     //Not entering here?    //To make it work can shoot the coin in laser
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<HealthStats>().ChangeHealth(10);
            Destroy(gameObject);
        }
    }

}