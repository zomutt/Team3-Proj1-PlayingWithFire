using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    [SerializeField] private Lever lever1; 
    [SerializeField] private Lever lever2; 
    [SerializeField] private Lever lever3; 
    [SerializeField] private Lever lever4; 

    [SerializeField] private GameObject objectToActivate;

    public void CheckLevers()
    {
        if (!lever1.IsUp() && lever2.IsUp() && lever3.IsUp() && !lever4.IsUp())
        {
            objectToActivate.SetActive(true);
        }
    }
}