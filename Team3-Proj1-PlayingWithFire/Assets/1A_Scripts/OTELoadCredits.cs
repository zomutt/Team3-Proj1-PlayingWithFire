using UnityEngine;
using UnityEngine.SceneManagement;

public class OTELoadCredits : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Credits");
        }
    }
}