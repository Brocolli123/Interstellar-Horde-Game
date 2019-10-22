using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionRing : MonoBehaviour
{
    public float fallspeed = 4f;
    public GameObject explosion;

    void Update()
    {
        transform.Translate(Vector3.down * fallspeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            Instantiate(explosion, transform.position, Quaternion.identity);        //create the explosion
            Destroy(gameObject);                //Destroy this
        }
    }

}
