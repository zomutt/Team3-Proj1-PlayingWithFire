using UnityEngine;


/// <summary>
/// Small script that is attached to the water wall. Used to control it's disappearance.
/// This is called from the appropriate key script when the player picks up the key that opens the water wall.
/// </summary>
public class WaterWall : MonoBehaviour
{
    public void LowerWall()
    {
        // Lower the water wall by moving it downwards
        transform.position += Vector3.down * 5f; // Adjust the value as needed for how much you want to lower it
    }
}
