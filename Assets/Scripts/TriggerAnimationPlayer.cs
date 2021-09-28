using UnityEngine;

public class TriggerAnimationPlayer : MonoBehaviour
{
    public string triggerTag = "Player";

    public string triggerName;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            anim.SetTrigger(triggerName);
        }
    }
}
