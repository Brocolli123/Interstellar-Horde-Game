using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class ShipControl : MonoBehaviour 
{
    private Rigidbody _rb;
    public float moveSpeed = 150.0f;
    public float defaultSpeed = 0f;
    public GameObject laser;
    public GameObject triLaser;
    public AudioSource audioSource;
    public AudioClip fire1;
    public AudioClip fire2;
    public AudioClip coin;
    public AudioClip death;
    public TextMeshProUGUI weaponTxt;

    public Transform chargeBar;
    public Slider chargeFill;

    public bool canFire;       //For time between shots     (public for use in other scripts to stop player firing)
    private float timer = 0f;
    private int fireMode = 1;               //which weapon is being used
    private float timeBetweenShots = 0.5f;  //how often the current weapon can be fired
    private float[] weaponCooldowns = new float[3]{0.5f,1.5f,1.5f}; //array of different cooldowns for each weapon
    private char lastMoved;
    
    bool isSpedUp;      //for speed up     
    float speedTimer = 0f;

    bool firingBurst = false;
    float burstTimer = 0f;
    float timeBetweenBursts = 0.1f;
    int burstsFired = 0;
    public float damage = 5;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        weaponTxt.text = "Weapon Type: Single Shot";
    }

    void Start ()
    {
        audioSource.volume = 0.8f;
        defaultSpeed = moveSpeed;       //sets the ships default speed to the movespeed at start
	}

	void Update () 
    {   //Horizontal and Vertical Movement
        if (!canFire)
            chargeFill.value = timer / timeBetweenShots;
        else
            chargeFill.value = 1f;
        _rb.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime, 0);

        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Vector3.Angle(transform.forward, Vector3.forward) <= 25.0)
            {
                transform.Rotate(0, -Input.GetAxis("Horizontal")*3, 0);
            }

            if (Input.GetAxis("Horizontal") > 0)
                lastMoved = 'R';
            else
                lastMoved = 'L';

            if (Vector3.Angle(transform.forward, Vector3.forward) > 25.0)
            {
                if (lastMoved=='R')
                    transform.localRotation = Quaternion.Euler(0, -25, 0);
                else
                    transform.localRotation = Quaternion.Euler(0, 25, 0);
            }
        }
        else
        {
            if (Vector3.Angle(transform.forward, Vector3.forward) != 0)
            {
                if (lastMoved=='R')
                    transform.Rotate(0, 3, 0);
                else
                    transform.Rotate(0, -3, 0);

                if (Vector3.Angle(transform.forward,Vector3.forward)<3)
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (Input.GetButtonDown("Fire1") && canFire && Time.timeScale!=0)       //fire laser
        {
            if (fireMode == 1)
            {
                audioSource.PlayOneShot(fire1, 0.4F);
                var instance = Instantiate(laser, transform.position, Quaternion.identity);
            }
            else if (fireMode == 2)
            {
                audioSource.PlayOneShot(fire2, 0.35F);
                var triInstance = Instantiate(triLaser, transform.position, Quaternion.identity);
            }
            if (fireMode == 3 && !firingBurst)
            {
                firingBurst = true;
                burstsFired = 0;
            }
            canFire = false;
        }

        if (Input.GetButtonDown("Firemode 1") && fireMode != 1)     //switch to weapon 1 (standard fire)
        {
            fireMode = 1;
            timeBetweenShots = weaponCooldowns[fireMode-1];
            canFire = false;
            weaponTxt.text = "Weapon Type: Single Shot";
        }
        if (Input.GetButtonDown("Firemode 2") && fireMode!=2)       //switch to weapon 2 (tri laser)
        {
            fireMode = 2;
            timeBetweenShots = weaponCooldowns[fireMode - 1];
            canFire = false;
            weaponTxt.text = "Weapon Type: Tri-Shot";
        }
        if (Input.GetButtonDown("Firemode 3") && fireMode != 3)     //switch to weapon 3 (burst shot)
        {
            fireMode = 3;                                       //change the variable which decides which shot to fire when the player clicks
            timeBetweenShots = weaponCooldowns[fireMode - 1];   //set the shot cooldown to the appropriate length (weapons have varying cooldowns)
            canFire = false;                                    //reset cooldown (so player cannot switch between weapons and spam attacks)
            weaponTxt.text = "Weapon Type: Burst Fire";         //update UI text showing which weapon is equipped
        }

        if (!canFire)
        {
            timer += Time.deltaTime;    //so player can't spam fire
            if (timer >= timeBetweenShots)
            {
                canFire = true;
                timer = 0f;
            }
        }

        if (firingBurst)
        {
            burstTimer += Time.deltaTime;
            if (burstTimer >= timeBetweenBursts)
            {
                burstsFired++;
                burstTimer = 0f;

                audioSource.PlayOneShot(fire1, 0.35F);
                var instance = Instantiate(laser, transform.position, Quaternion.identity);
            }
            if (burstsFired >= 3)
            {
                firingBurst = false;
            }
        }

        if (isSpedUp)     //Use same timer for every pickup?                 CHECK if it works with a second pickup      
        { 
            speedTimer += Time.deltaTime;
            if (speedTimer >= 10f)
            {
                moveSpeed = defaultSpeed;   //resets movespeed   
                speedTimer = 0f;            //reset timer
                isSpedUp = false;           //exit loops
            }
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6f, 6f), Mathf.Clamp(transform.position.y, -4f, 4f), transform.position.z); //sets boundaries for player
    }

    public void ChangeMoveSpeed()
    {
        moveSpeed *= 2;     //Speedup player        have the increase stored as public?
        isSpedUp = true;      //to do timer in update
    }

    public void IncreaseFireRate()
    {
        weaponCooldowns[0] = (weaponCooldowns[0] * 0.5f);
        weaponCooldowns[1] = (weaponCooldowns[1] * 0.5f);
        weaponCooldowns[2] = (weaponCooldowns[2] * 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss") {
            GameManager.PlayerDead(true);       //Kills player if they collide with boss
        }
        if (other.tag == "Enemy")       //Take damage if collide with regular enemy
        {
            GetComponent<HealthStats>().ChangeHealth(-5);  //Hard coded number here
        }
        if (other.tag == "Coin")
        {
            audioSource.PlayOneShot(coin, 1F);
        }
    }

}
