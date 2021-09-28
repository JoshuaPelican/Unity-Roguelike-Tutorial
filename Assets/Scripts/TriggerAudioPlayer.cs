using UnityEngine;

public class TriggerAudioPlayer : MonoBehaviour
{
    public string triggerTag = "Player";

    public float lowPitch = 0.9f;
    public float highPitch = 1.1f;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            source.pitch = Random.Range(lowPitch, highPitch);

            source.Play();
        }
    }
}
