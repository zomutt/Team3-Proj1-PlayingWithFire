using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour // place on player 
{
    public TextMeshProUGUI promptDisplay; // put UI text here in inspector field
    private WaterStatue currentStatue;

    private void Start()
    {
        promptDisplay.gameObject.SetActive(false);
    } // so player doesn't see this until later

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentStatue != null)
        {
            currentStatue.Interact();
        } // if player is near statue and presses E, they interact with it
    }

    private void OnTriggerEnter(Collider other)
    {
        WaterStatue statue = other.GetComponent<WaterStatue>();
        if (statue != null)
        {
            currentStatue = statue;
            promptDisplay.gameObject.SetActive(true);
        } // if player enters the trigger hit box, checks to see if its the statue and shows the text
    }

    private void OnTriggerExit(Collider other)
    {
        WaterStatue statue = other.GetComponent<WaterStatue>();
        if (statue == currentStatue)
        {
            currentStatue = null;
            promptDisplay.gameObject.SetActive(false); // disables text box when player exits the current statues hitbox
        }
    }
}