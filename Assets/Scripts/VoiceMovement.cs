using System.Collections;
using UnityEngine;

public class VoiceMovement : MonoBehaviour
{
    public delegate void DeathListener(int score);

    public BallSpawnRegion ballSpawnRegion;
    public MoveRight moveRight;

    private MicrophoneData microphoneData;
    public float movementRate;
    public float frequenciesPerBin;
    public float percentLeftScreen;
    public int bins;
    public LayerMask collisionLayer;
    public float stupidLittleOffset;
    public float velocity;
    public float explosionForce;
    public float waitTimeBeforeSpawn;
    public TrailRenderer trailRenderer;

    private bool controllable;
    private int score;
    private Vector3 targetPosition;
    private float binSize;
    private Vector2 boundingBox;
    private RaycastHit2D[] results;
    private Rigidbody2D rigidbody2d;
    private DeathListener deathListener;

    public void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        trailRenderer.sortingOrder = -1;
        controllable = false;
        microphoneData = Singleton<MicrophoneData>.Instance;
        rigidbody2d = GetComponent<Rigidbody2D>();
        float topOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
        float leftOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x;
        float xPosition = -leftOfScreen + leftOfScreen * percentLeftScreen;
        transform.position = new Vector3(xPosition, 0, 0);
        targetPosition = transform.position;
        binSize = (topOfScreen * 2) / bins;
        score = 0;
        StartCoroutine(SpawnAnimation());
    }

    private IEnumerator SpawnAnimation()
    {
        yield return new WaitForSeconds(waitTimeBeforeSpawn);
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("spawn");
    }

    public void FixedUpdate()
    {
        if(!controllable)
            return;
        int expectedSpectrumFrequency = microphoneData.expectedSpectrumFrequency;
        Debug.Log(expectedSpectrumFrequency + " " + microphoneData.maxSpectrumFrequency);

        float frequencyDifference = microphoneData.maxSpectrumFrequency - expectedSpectrumFrequency;
        int newBin = (int)(frequencyDifference / frequenciesPerBin);
        targetPosition = new Vector3(transform.position.x, newBin * binSize, 0);

        Vector3 v3Difference = targetPosition - transform.position;
        Vector2 difference= new Vector2(v3Difference.x, v3Difference.y);
        Vector2 direction;
        if (difference.magnitude <= 1)
            direction = difference;
        else
            direction = difference.normalized;
        rigidbody2d.velocity = direction * velocity;
    }

    public void IncrementScore()
    {
        score++;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() == null)
            return;

        deathListener(score);
        Explodable explodable = GetComponent<Explodable>();
        explodable.explode(explosionForce, transform.position);
        ballSpawnRegion.DestroyActiveBalls(transform.position);
        moveRight.move = false;
        Singleton<SlowmoManager>.Instance.StartSlowmo();
    }

    public void RegisterDeathListener(DeathListener deathListener)
    {
        this.deathListener = deathListener;
    }

    public void AnimationDone()
    {
        StartCoroutine(StartSmallDelay());
    }

    public IEnumerator StartSmallDelay()
    {
        yield return new WaitForSeconds(.7f);
        controllable = true;
        ballSpawnRegion.stopSpawning = false;
        moveRight.move = true;
        trailRenderer.gameObject.SetActive(true);
    }
}