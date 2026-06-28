using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float forceStrength;

    private void FixedUpdate()
    {
        rb.AddForce(Random.insideUnitSphere * forceStrength);
    }
}
