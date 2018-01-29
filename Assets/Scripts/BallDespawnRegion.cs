using UnityEngine;

public class BallDespawnRegion : MonoBehaviour
{
    public BallSpawnRegion ballSpawnRegion;
    public float extraXPosition;
    public VoiceMovement voiceMovement;

    public void Awake()
    {
        float leftOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x + extraXPosition;
        transform.position = new Vector3(-leftOfScreen, 0, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        if(ball == null)
            return;

        if(voiceMovement != null)
            voiceMovement.IncrementScore();
        ballSpawnRegion.AddBallToQueue(ball);
    }
}