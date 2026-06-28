using UnityEngine;

public class ItemCollisionHandler : MonoBehaviour
{
    [field: SerializeField] public float collisionRadius { get; private set; } = 2.5f;
    [field: SerializeField] public float spreadMultiplier { get; private set; } = .5f;
    [SerializeField] float bounceStrength = 0.8f;
    [SerializeField] float damping = 0.5f;       // How much velocity to keep after bounce
    [SerializeField] float smoothPush = 5f;      // How strongly to push back inside

    void FixedUpdate()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            Vector3 offset = rb.position - transform.position;
            float dist = offset.magnitude;

            if (dist > collisionRadius)
            {
                Vector3 normal = offset.normalized;

                // Push the object softly toward the inside
                Vector3 targetPos = transform.position + normal * collisionRadius * 0.98f;
                rb.position = Vector3.Lerp(rb.position, targetPos, Time.fixedDeltaTime * smoothPush);

                // Reflect velocity gently and apply damping
                Vector3 reflectedVel = Vector3.Reflect(rb.linearVelocity, normal) * bounceStrength;
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, reflectedVel, Time.fixedDeltaTime * 8f);
                rb.linearVelocity *= (1f - damping * Time.fixedDeltaTime);
            }
        }
    }
}
