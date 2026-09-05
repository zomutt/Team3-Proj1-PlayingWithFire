using UnityEngine;

namespace _1A_Scripts
{
    // Tracks its own movement, so drop it on the player or an enemy, doesn't matter.
    [RequireComponent(typeof(AudioSource))]
    public class FootstepSounds : MonoBehaviour
    {
        // 0-4 = jump sounds, 5-24 = footsteps. Don't rearrange the array or I *will* cry.
        [SerializeField] private AudioClip[] footstepClips;
        private const int JumpClipCount = 5;

        [SerializeField] private float stepInterval = 0.45f;
        [SerializeField] private float referenceSpeed = 6f;    // above this and the steps come quicker
        [SerializeField] private float minSpeedToStep = 0.1f;  // Go slow, go fast, who cares? You runnin', babe. Till you're not. :)

        private const float SpeedSampleWindow = 0.1f; // averaged over this long so a Rigidbody's FixedUpdate ticks don't spike a single frame's reading

        private AudioSource audioSource;
        private Vector3 sampleStartPosition;
        private float sampleTimer;
        private float speed;
        private float stepTimer;
        private int lastFootstepIndex = -1;
        private int lastJumpIndex = -1;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            sampleStartPosition = transform.position;
        }

        private void Update()
        {
            sampleTimer += Time.deltaTime;
            if (sampleTimer >= SpeedSampleWindow)
            {
                speed = Vector3.Distance(transform.position, sampleStartPosition) / sampleTimer;
                sampleStartPosition = transform.position;
                sampleTimer = 0f;
            }

            if (speed < minSpeedToStep)
            {
                stepTimer = 0f;
                return;
            }

            stepTimer += Time.deltaTime;

            float interval = stepInterval * (referenceSpeed / speed);

            if (stepTimer >= interval)
            {
                stepTimer = 0f;
                PlayRandomFootstep();
            }
        }

        private void PlayRandomFootstep()
        {
            int footstepCount = footstepClips.Length - JumpClipCount;
            if (footstepClips == null || footstepCount <= 0) return;

            int index;
            do     // Pick a sound
            {
                index = JumpClipCount + Random.Range(0, footstepCount);
            }
            while (footstepCount > 1 && index == lastFootstepIndex);   // Reroll, no playing the same one back to back.

            lastFootstepIndex = index;   
            audioSource.PlayOneShot(footstepClips[index]);
        }

        public void PlayJumpSound()     // The wheeeee gotta have a thud
        {
            if (footstepClips == null || footstepClips.Length < JumpClipCount) return;

            int index;
            do
            {
                index = Random.Range(0, JumpClipCount);
            }
            while (JumpClipCount > 1 && index == lastJumpIndex);

            lastJumpIndex = index;
            audioSource.PlayOneShot(footstepClips[index]);
        }
    }
}
