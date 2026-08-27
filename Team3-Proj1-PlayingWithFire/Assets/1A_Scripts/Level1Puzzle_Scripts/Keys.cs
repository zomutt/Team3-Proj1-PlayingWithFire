using System;
using UnityEngine;

namespace _1A_Scripts.Level1Puzzle_Scripts
{
    public enum KeyColor
    {
        Red,
        Green,
        Blue,
        Purple
    }
    public class Keys : MonoBehaviour
    {
        private enum KeyColor { Red, Green, Blue, Purple }
        [SerializeField] private KeyColor keyColor;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            switch (keyColor)
            {
                case KeyColor.Red:
                    LevelOnePuzzleManager.Instance.CollectKeys("red");
                    break;
                case KeyColor.Green:
                    LevelOnePuzzleManager.Instance.CollectKeys("green");
                    break;
                case KeyColor.Blue:
                    LevelOnePuzzleManager.Instance.CollectKeys("blue");
                    break;
                case KeyColor.Purple:
                    LevelOnePuzzleManager.Instance.CollectKeys("purple");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            gameObject.SetActive(false);
        }
    }
}