using UnityEngine;

/// <summary>
/// This is a master script for all of level one that controls puzzle behavior. Puzzles may still have their own supporting scripts, but this is a 
/// centralized way to reduce clutter in our main scripts so that they can be reused throughout the game.
/// </summary>
public class LevelOnePuzzleManager : MonoBehaviour
{
    private bool hasRedKey;
    public bool HasRedKey => hasRedKey;
    private bool hasGreenKey;
    public bool HasGreenKey => hasGreenKey;

    private bool hasBlueKey;
    public bool HasBlueKey => hasBlueKey;

    private bool hasPurpleKey;
    public bool HasPurpleKey => hasPurpleKey;

    [SerializeField] private GameObject[] keysArray;   // For bulk processes
    [SerializeField] private GameObject redKey;    // For individual behaviour
    [SerializeField] private GameObject greenKey;
    [SerializeField] private GameObject blueKey;
    [SerializeField] private GameObject purpleKey;
    private void Start()
    {

    }

    private void ResetGame()
    {
        foreach (var key in keysArray)
        {
            key.SetActive(false);
        }

        hasRedKey = false;
        hasGreenKey = false;
        hasBlueKey = false;
        hasPurpleKey = false;
    }
}
