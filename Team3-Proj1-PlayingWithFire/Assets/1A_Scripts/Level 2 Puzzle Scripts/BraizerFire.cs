using UnityEngine;
using _1A_Scripts.Level2Puzzles;

public class BrazierFire : FireReceiver
{
    [Header("Brazier Variants")]
    [SerializeField] private GameObject defaultBrazier;
    [SerializeField] private GameObject[] colorBraziers; // the order the flame cycles through is green, purple, pink, blue

    [Header("Puzzle")]
    [SerializeField] private int correctColorIndex; // which flame it needs to be on

    [Header("Timing")]
    [SerializeField] private float holdTimeToChange = 1f;
    [SerializeField] private float breakThreshold = 0.15f;

    private int colorIndex = -1;
    private float hitTimer;
    private float lastHitTime;
    private GameObject currentActiveBrazier;

    private void Start()
    {
        currentActiveBrazier = defaultBrazier;
    }

    private void Update()
    {
        if (hitTimer > 0f && Time.time - lastHitTime > breakThreshold)
        {
            hitTimer = 0f;
        }
    } // resets the timer if fire contact doesnt go past 1 second 

    public override void ReceiveFire()
    {
        hitTimer += Time.deltaTime;
        lastHitTime = Time.time;

        if (hitTimer >= holdTimeToChange)
        {
            AdvanceColor();
            hitTimer = 0f;
        }
    } // changes color if it fire contact is past 1 second

    private void AdvanceColor()
    {
        colorIndex = (colorIndex + 1) % colorBraziers.Length;
        Debug.Log($"[BrazierFire] {name} switching to colorIndex={colorIndex} ({colorBraziers[colorIndex].name}), needs={correctColorIndex}, correct={IsCorrect()}");
        SwitchTo(colorBraziers[colorIndex]);
        LevelTwoPuzzleManager.Instance.CheckBraziers();
    }

    private void SwitchTo(GameObject next)
    {
        currentActiveBrazier.SetActive(false);
        next.SetActive(true);
        currentActiveBrazier = next;
    }

    // changes color (prefab to be more accurate) and sends it to the puzzle manager 

    public bool IsCorrect()
    {
        return colorIndex == correctColorIndex;
    }
}