using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    [Header("Vehicle Settings")]
    public float moveSpeed = 1;
    public float rotationSpeed = 60;
    [Min(1)]
    public float tireDirectionModifier = 1;

    [Header("Skidmark Settings")]
    public float skidVelocityThreshold = 0.25f;
    public float skidTorqueThreshold = 0.12f;
    public TrailRenderer[] skids;

    [Header("Audio Settings")]
    public float lowCrashPitch = 0.9f;
    public float highCrashPitch = 1.1f;
    public float minorCrashThreshold = 0.5f;
    public float mediumCrashThreshold = 1.5f;
    public AudioClip minorCrash;
    public AudioClip mediumCrash;
    public AudioClip majorCrash;
    public AudioSource crashSource;

    public AudioSource engineSource;


    //Private variables
    private Rigidbody2D rig;
    private Vector3 localVelocity = Vector3.zero;
    private Vector3 currentDirection;
    private float torque;

    protected virtual void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rig.velocity);

        float enginePower = Mathf.Clamp(Mathf.Sqrt(rig.velocity.magnitude * currentDirection.magnitude) / 2, 0.3f, 1.1f);

        engineSource.volume = enginePower / 4;
        engineSource.pitch = enginePower * 2;

        currentDirection *= 0.995f;
        torque /= 2;

        ApplyTireDirectionRestrictions();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float hitMagnitude = collision.relativeVelocity.magnitude;
        crashSource.pitch = Random.Range(0.9f, 1.1f);

        if(hitMagnitude <= minorCrashThreshold)
        {
            crashSource.clip = minorCrash;
        }
        else if(hitMagnitude > minorCrashThreshold && hitMagnitude <= mediumCrashThreshold)
        {
            crashSource.clip = mediumCrash;
        }
        else if(hitMagnitude > mediumCrashThreshold)
        {
            crashSource.clip = majorCrash;
        }

        crashSource.Play();
    }

    private void ApplyTireDirectionRestrictions()
    {
        Vector3 modifiedVelocity = localVelocity;

        modifiedVelocity.x /= 2;

        rig.velocity = transform.TransformDirection(modifiedVelocity);
    }

    protected void MoveInDirection(Vector3 direction)
    {
        currentDirection = direction;
        Vector2 force = moveSpeed * currentDirection.normalized * 100 * Time.deltaTime;

        foreach (TrailRenderer trail in skids)
        {
            trail.emitting = (rig.velocity.magnitude * force.magnitude) < skidVelocityThreshold || Mathf.Abs(torque) > skidTorqueThreshold;
        }

        rig.AddForce(force, ForceMode2D.Force);
    }

    protected void Rotate(float direction)
    {
        torque = -direction * Time.deltaTime * rotationSpeed * rig.velocity.sqrMagnitude * Mathf.Sign(localVelocity.y);

        foreach (TrailRenderer trail in skids)
        {
            trail.emitting = Mathf.Abs(torque) > skidTorqueThreshold;
        }

        rig.AddTorque(torque, ForceMode2D.Force);
    }

    protected void RotateTowards(Vector3 targetPosition)
    {
        Vector3 desiredDirection = (targetPosition - transform.position).normalized;

        float angleMagnitude = (desiredDirection - transform.up).normalized.magnitude * Time.deltaTime * rotationSpeed;

        rig.AddTorque(angleMagnitude, ForceMode2D.Force);
    }
}
