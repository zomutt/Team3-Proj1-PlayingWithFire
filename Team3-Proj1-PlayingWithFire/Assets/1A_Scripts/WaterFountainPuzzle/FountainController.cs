using UnityEngine;

public class FountainController : MonoBehaviour // place on fountain 
{
    public WaterStatue[] statues; // add 4 fields and add each statue to it 
    public ParticleSystem fountainVFX; // for one particle system

    public void checkStatues()
    {
        foreach (WaterStatue statue in statues)
        {
            if (!statue.IsCorrect())
            {
                return;
            }
        }
        fountainVFX.Stop(); // disables the particle once all 4 statues face the right way
    }
}
