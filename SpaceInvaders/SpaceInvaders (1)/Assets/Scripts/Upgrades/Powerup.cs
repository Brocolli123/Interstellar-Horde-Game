using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    //Inefficient to spawn this then spawn the actual pickup
    public GameObject[] puplist;
    int index;
    //List of different powerup scripts here to pick randomly? Or inheriting?
    //Just use this as spawner?
    //Have on GameManager? (How to get spawn location then?)

    void Start()
    {
        index = Random.Range(0, puplist.Length);    //Pick random pickup
        Instantiate(puplist[index]);                //Spawn Pickup
        Destroy(gameObject);                        //Destroy this
    }

}
