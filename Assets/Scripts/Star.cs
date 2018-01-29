using UnityEngine;

public class Star : MonoBehaviour
{
    /*public MoveRight moveRight;
    public float variance;*/
    public float velocity;

    public void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + velocity * Time.deltaTime, transform.position.y, transform.position.z);
    }
}