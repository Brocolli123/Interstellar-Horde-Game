using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthStats : MonoBehaviour
{
    public Transform healthBar;
    public Slider healthFill;

    public float currentHealth;
    public float maxHealth;
    public float healthBarYoffset = 2;

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);      //allows us to ensure that the health cant go under 0 or above the maxHealth
        healthFill.value = currentHealth / maxHealth;                //allows to get a value between 0 and 1 o turn into % of what the maxHealth is
    }

    private void PositionHealthBar()            //allows to get the healthbar position
    {
        Vector3 currentPos = transform.position;
        healthBar.position = new Vector3(currentPos.x, currentPos.y + healthBarYoffset, currentPos.z);
        healthBar.LookAt(Camera.main.transform);            //makes sure healthbar always faces the camera
    }
    // Update is called once per frame
    void Update()
    {
        PositionHealthBar();
    }
}
