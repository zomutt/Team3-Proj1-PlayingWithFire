using UnityEngine;

namespace _1A_Scripts.Managers
{
    // Goes on Yania's TransitionAnimCanvas -- one fresh copy per scene, never DontDestroyOnLoad.
    public class TransitionPanel : MonoBehaviour
    {
        [SerializeField] private float animationLength = 1f; 

        public float AnimationLength => animationLength;

        private void Awake()
        {
            gameObject.SetActive(false); // stays off until the scene has settled, then UIController turns it on
        }

        public void Play()
        {
            gameObject.SetActive(true); // Animator auto-plays once enabled
        }
    }
}
