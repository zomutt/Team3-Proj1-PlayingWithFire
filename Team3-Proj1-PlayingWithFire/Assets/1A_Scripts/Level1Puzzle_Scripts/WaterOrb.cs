using UnityEngine;

public class WaterOrb : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Level3") || collision.gameObject.CompareTag("Player")) // attach the level 3 tag to anything you want the orb to be destroyed
                                                                                                    // once it collides with it. 
        {
            Destroy(gameObject); 
        }
    }
}
