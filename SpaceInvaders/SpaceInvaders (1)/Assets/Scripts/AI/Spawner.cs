using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private ParticleSystem spawnParticles;  //Use same for every enemy for now
    private ShipControl player;
    private bool allSpawned;

    IEnumerator Loop()
    {
        foreach (Transform child in transform)      //Set them to active one by one (have wait inbetween? spawn particle effect?) (Stop enemies auto firing?)
        {
                child.gameObject.SetActive(true);
                Instantiate(spawnParticles, child.gameObject.transform.position, child.gameObject.transform.rotation);  //Creates spawn particles
                yield return new WaitForSeconds(0.1f);      //have number for this delay
        }
        yield return null;
    }

    void Start()                // (it's not setting them all to active)
    {

        foreach (Transform child in transform)  //Starts with the enemies inactive      (or just set inactive in prefab?)
        {
            child.gameObject.SetActive(false);
        }
        //StartCoroutine(Loop());

        foreach (Transform child in transform)      //Set them to active one by one (have wait inbetween? spawn particle effect?) (Stop enemies auto firing?)
        {
            child.gameObject.SetActive(true);
            Instantiate(spawnParticles, child.gameObject.transform.position, child.gameObject.transform.rotation);  //Creates spawn particles
        }
    }
    
}