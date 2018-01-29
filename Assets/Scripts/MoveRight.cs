using UnityEngine;

public class MoveRight: MonoBehaviour
{
    public float velocity;
    public bool move;

    public void FixedUpdate()
    {
        if (!move)
            return;
        transform.position = new Vector3(transform.position.x + velocity * Time.deltaTime, transform.position.y, transform.position.z);
    }
}