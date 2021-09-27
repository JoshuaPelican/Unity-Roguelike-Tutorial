using UnityEngine;

public class ImpactAudioPlayer : MonoBehaviour
{
    public float impactAudioThreshold = 0.5f;

    public float lowPitch = 0.9f;
    public float highPitch = 1.1f;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude > impactAudioThreshold)
        {
            source.pitch = Random.Range(lowPitch, highPitch);

            source.Play();
        }
    }
}
