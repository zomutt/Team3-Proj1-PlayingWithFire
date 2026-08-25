using Unity.VisualScripting;
using UnityEngine;

public class FountainController : MonoBehaviour // place on fountain 
{
    public WaterStatue[] statues; // add 4 fields and add each statue to it 
    public GameObject fountainWaterHazard; // hazard for the water fountain

    public void checkStatues()
    {
        foreach (WaterStatue statue in statues)
        {
            if (!statue.IsCorrect())
            {
                return;
            }
        }
        fountainWaterHazard.SetActive(false); // disables the hazard once all 4 statues face the right way
    }
}
