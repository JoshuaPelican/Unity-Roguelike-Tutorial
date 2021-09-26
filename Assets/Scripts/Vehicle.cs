using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    [Header("Vehicle Settings")]
    public float moveSpeed = 1;
    public float rotationSpeed = 60;
    [Min(1)]
    public float tireDirectionModifier = 1;

    [Header("Audio Settings")]

    //Private variables
    private Rigidbody2D rig;
    private Vector3 localVelocity = Vector3.zero;

    protected virtual void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rig.velocity);

        ApplyTireDirectionRestrictions();
    }

    private void ApplyTireDirectionRestrictions()
    {
        Vector3 modifiedVelocity = localVelocity;

        modifiedVelocity.x /= 2;

        rig.velocity = transform.TransformDirection(modifiedVelocity);
    }

    protected void MoveInDirection(Vector3 direction)
    {
        Vector2 force = moveSpeed * direction.normalized * 100 * Time.deltaTime;

        rig.AddForce(force, ForceMode2D.Force);
    }

    protected void Rotate(float direction)
    {
        float torque = -direction * Time.deltaTime * rotationSpeed * rig.velocity.sqrMagnitude * Mathf.Sign(localVelocity.y);

        rig.AddTorque(torque, ForceMode2D.Force);
    }

    protected void RotateTowards(Vector3 targetPosition)
    {
        Vector3 desiredDirection = (targetPosition - transform.position).normalized;

        float angleMagnitude = (desiredDirection - transform.up).normalized.magnitude * Time.deltaTime * rotationSpeed;

        rig.AddTorque(angleMagnitude, ForceMode2D.Force);
    }
}
