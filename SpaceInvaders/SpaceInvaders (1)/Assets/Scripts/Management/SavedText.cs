using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SavedText : MonoBehaviour
{
    public Text displayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayText()
    {
        displayText.text = "GAME SAVED!";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
