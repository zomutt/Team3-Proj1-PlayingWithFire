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
        [SerializeField] private AudioClip audioClip;
        private AudioSource audioSource;

        [SerializeField] private GameObject POIRing;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            POIRing.SetActive(true);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            POIRing.SetActive(false);
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
                if (audioClip)
                    audioSource.PlayOneShot(audioClip);
            }
            gameObject.SetActive(false);
        }
    }
}