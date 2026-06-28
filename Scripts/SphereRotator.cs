using UnityEngine;

public class SphereRotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f; // drag sensitivity
    [SerializeField] float inertiaDamping = 4f; // higher = stops faster
    [SerializeField] float maxSpeed = 500f; // drgrees per second

    Vector3 lastPointerPos;
    Vector3 angularVelocity;

    bool dragStarted;
    bool isDragging;

    private void Update()
    {
        Vector2 dragDelta = Vector2.zero;

        // --- Mouse Input ---
        if (Input.GetMouseButtonDown(0))
        {
            dragStarted = true;
            lastPointerPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            dragStarted = false;
        }
        else if(dragStarted && Input.GetMouseButton(0))
        {
            Vector3 newPos = Input.mousePosition;
            dragDelta = newPos - lastPointerPos;
            lastPointerPos = newPos;
        }

        isDragging = Mathf.Abs(dragDelta.x) > 5f || Mathf.Abs(dragDelta.y) > 5f; // dragDelta != Vector2.zero

        // --- Touch Input ---
        if(Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                dragStarted = true;
                lastPointerPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved)
            {
                dragDelta = touch.deltaPosition;
            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                dragStarted = false;
            }
        }

        // --- Drag Rotation ---
        if (dragStarted)
        {
            // convert drag to rotation in world-space axes
            float rotX = dragDelta.y * rotationSpeed * Time.deltaTime;
            float rotY = -dragDelta.x * rotationSpeed * Time.deltaTime;

            //Build rotation data from quaternions
            Quaternion deltaRotation = Quaternion.Euler(rotX, rotY, 0f);
            transform.rotation = deltaRotation * transform.rotation;

            // Save angular velocity (deg/sec)
            angularVelocity = new Vector3(rotX / Time.deltaTime, rotY / Time.deltaTime, 0f);
            angularVelocity = Vector3.ClampMagnitude(angularVelocity, maxSpeed);
        }
        else
        {
            // --- Inertia ---
            if(angularVelocity.sqrMagnitude > .01f)
            {
                Quaternion deltaRotation = Quaternion.Euler(angularVelocity * Time.deltaTime);
                transform.rotation = deltaRotation * transform.rotation;

                // smoothly slow down
                angularVelocity = Vector3.Lerp(angularVelocity, Vector3.zero, Time.deltaTime * inertiaDamping);
            }
        }
    }
}
