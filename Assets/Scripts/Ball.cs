using UnityEngine;

public class Ball : MonoBehaviour
{
    public float velocity;
    public float rotationVelocity;

    private Rigidbody2D rigidbody2d;

    public void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        rigidbody2d.velocity = Vector3.left * velocity;
        rigidbody2d.angularVelocity = rotationVelocity;
    }
}