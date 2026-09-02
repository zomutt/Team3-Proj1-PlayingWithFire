using _1A_Scripts.Managers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private KeyColor keyColor;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (SceneManager.GetActiveScene().name == "LevelOne")          // Will create LevelTwo logic on a day that isn't deadline day. It works.
            {
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

            }
            //else if (SceneManager.GetActiveScene().name == "LevelTwo")     // Already handled in KeysLevelTwo.cs, but this is here for future reference if we want to add more levels.
            //{
            //    switch (keyColor)
            //    {
            //        case KeyColor.Red:
            //            UIController.Instance.UpdateKeys("red");
            //            break;
            //        case KeyColor.Green:
            //            UIController.Instance.UpdateKeys("green");
            //            break;
            //        case KeyColor.Blue:
            //            UIController.Instance.UpdateKeys("blue");
            //            break;
            //        case KeyColor.Purple:
            //            UIController.Instance.UpdateKeys("purple");
            //            break;
            //        default:
            //            throw new ArgumentOutOfRangeException();
            //    }
            //}
            gameObject.SetActive(false);
        }
    }
}