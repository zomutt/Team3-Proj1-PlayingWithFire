using UnityEngine;

public class WaterStatue : MonoBehaviour // places on water statues
{
    public int facingStep;
    public int correctFacingStep; // ints for the statue puzzles
    public FountainController fountainController; // for the fountain

    void Start()
    {
        ApplyRotation();
    }

    public void Interact()
    {
        facingStep = (facingStep + 1) % 4; // so it wraps around 3 
        ApplyRotation();
        fountainController.checkStatues(); // checks to see if all statues are facing the right way everytime the player rotates 
    }

    void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0f, facingStep * 90f, 0f); // makes it so statues actually rotate 90 degrees
    }

    public bool IsCorrect()
    {
        return facingStep == correctFacingStep; // to see if its actually facing the right way
    }

}