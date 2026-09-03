using _1A_Scripts.Managers;
using System;
using UnityEngine;

namespace _1A_Scripts.Level2Puzzles
{
    public enum KeyColor
    {
        Red, Green, Blue, Purple
    }

    public class KeysLevelTwo : MonoBehaviour
    {
        [SerializeField] private KeyColor keyColor;
        [SerializeField] private AudioClip keyClip;
        [SerializeField] private GameObject poi;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            poi.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            switch (keyColor)
            {
                case KeyColor.Red:
                    LevelTwoPuzzleManager.Instance.CollectKey("red");
                    UIController.Instance.UpdateKeys("red");
                    break;
                case KeyColor.Green:
                    LevelTwoPuzzleManager.Instance.CollectKey("green");
                    UIController.Instance.UpdateKeys("green");
                    break;
                case KeyColor.Blue:
                    LevelTwoPuzzleManager.Instance.CollectKey("blue");
                    UIController.Instance.UpdateKeys("blue");
                    break;
                case KeyColor.Purple:
                    LevelTwoPuzzleManager.Instance.CollectKey("purple");
                    UIController.Instance.UpdateKeys("purple");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            poi.SetActive(false);
            gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(keyClip, transform.position);
        }
    }
}