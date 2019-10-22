using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement : MonoBehaviour
{
    private Rigidbody _rb;
    float movement = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
        transform.position = new Vector3(transform.position.x + movement*Time.deltaTime, transform.position.y, transform.position.z);   

        if (transform.position.x < -4.7)
        {
            for (int i = 0; i < 5; i++)
            {
                transform.position = new Vector3(-4.7f, transform.position.y - 0.35f, transform.position.z);
            }
            movement *= -1;
        }

        if (transform.position.x > 4.7)
        {
            for (int i = 0; i < 5; i++)
            {
                transform.position = new Vector3(4.7f, transform.position.y - 0.35f, transform.position.z);
            }
            movement *= -1;
        }
    }
}
