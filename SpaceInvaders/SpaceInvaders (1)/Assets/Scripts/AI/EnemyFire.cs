using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public GameObject shotPrefab;
    public Vector2 timeBetweenShots = new Vector2(0.5f, 1.0f);
    public float speed = 1;
    public AudioSource audioSource;
    public AudioClip fire3;

    private float nextShot = -1;
    private bool okToFire = false;

    void Awake()
    {
        Debug.Assert(shotPrefab.GetComponent<Laser>() != null);
        Random.seed = (int)Time.realtimeSinceStartup;
       okToFire = true;
    }

    void Start()
    {
        audioSource.volume = 0.3f;
        nextShot = Time.time + Random.Range(timeBetweenShots.x, timeBetweenShots.y);
    }

    void Update()
    {
        if (okToFire && nextShot < Time.time)
        {
            audioSource.PlayOneShot(fire3, 0.7F);
            nextShot = Time.time + Random.Range(timeBetweenShots.x, timeBetweenShots.y);
            Instantiate(shotPrefab, transform.position, Quaternion.identity);
        }
    }

    
    void OnBecameInvisible()
    {
        okToFire = false;
    }
}