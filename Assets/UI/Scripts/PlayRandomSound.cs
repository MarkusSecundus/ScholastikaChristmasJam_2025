using System.Collections;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip randomClip;

    void Start()
    {
        StartCoroutine(PlaySoundAtRandomIntervals());
    }

    IEnumerator PlaySoundAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(120f, 360f);

            yield return new WaitForSeconds(waitTime);

            if (randomClip != null)
            {
                audioSource.PlayOneShot(randomClip);
            }
        }
    }
}