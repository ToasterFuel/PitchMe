using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnRegion : MonoBehaviour
{
    public GameObject ballPrefab;
    public float extraXPosition;
    public float yShrinkRange;
    public float maxVelocity;
    public float minVelocity;
    public float minWaitSpawnTime;
    public float waitSpawnTime;
    public int ballCount;
    public int dodgesBeforeDecreaseLatency;
    public float decreaseSpawnTimeRate;
    public float increaseVelocityRate;
    public float maxMaxVelocity;
    public float maxMinVelocity;
    public float minRotateVelocity;
    public float maxRotateVelocity;
    public float explosionForce;
    public int spawnAtBottomCount;
    public bool stopSpawning;

    private int bottomCounter;
    private int ballsAdded;
    private float ySpawnRange;
    private float timer;
    private List<Ball> pendingBalls;
    private HashSet<Ball> activeBalls;

    public void Awake()
    {
        bottomCounter = 0;
        ballsAdded = 0;
        pendingBalls = new List<Ball>(ballCount);
        activeBalls = new HashSet<Ball>();
        for(int i = 0; i < ballCount; i++)
        {
            Ball ball = Instantiate(ballPrefab, transform.parent).GetComponent<Ball>();
            //Yes I'm lazy, intead of deactivating the ball I'm just moving it stupidly far away from the camera.
            ball.transform.position = new Vector3(-49999, 0, 0);
            pendingBalls.Add(ball);
        }
        ySpawnRange = (Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y * 2 - yShrinkRange)/2;
        float rightOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x + extraXPosition;
        transform.position = new Vector3(rightOfScreen, 0, 0);
        timer = 0;
    }

    public void FixedUpdate()
    {
        if(stopSpawning)
            return;
        timer += Time.deltaTime;

        if(timer > waitSpawnTime && pendingBalls.Count != 0)
        {
            Ball ball = pendingBalls[0];
            pendingBalls.Remove(ball);
            if(++bottomCounter >= spawnAtBottomCount)
            {
                ball.transform.position = GetBottomSpawnPosition();
                bottomCounter = 0;
            }
            else
            {
                ball.transform.position = GetRandomPosition();
            }
            ball.velocity = Random.Range(minVelocity, maxVelocity);
            ball.rotationVelocity = Random.Range(minRotateVelocity, maxRotateVelocity);
            timer = 0;
            activeBalls.Add(ball);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float y = Random.Range(transform.position.y - ySpawnRange, transform.position.y + ySpawnRange);
        return new Vector3(transform.position.x, y, 0);
    }

    private Vector3 GetBottomSpawnPosition()
    {
        float y = transform.position.y - ySpawnRange;
        return new Vector3(transform.position.x, y, 0);
    }

    public void AddBallToQueue(Ball ball)
    {
        activeBalls.Remove(ball);
        pendingBalls.Add(ball);
        if(++ballsAdded > dodgesBeforeDecreaseLatency)
        {
            maxVelocity += increaseVelocityRate;
            if(maxVelocity > maxMaxVelocity)
                maxVelocity = maxMaxVelocity;
            minVelocity += increaseVelocityRate;
            if (minVelocity > maxMinVelocity)
                minVelocity = maxMinVelocity;

            waitSpawnTime -= decreaseSpawnTimeRate;
            ballsAdded = 0;
            if(waitSpawnTime < minWaitSpawnTime)
                waitSpawnTime = minWaitSpawnTime;
        }
    }

    public void DestroyActiveBalls(Vector3 explosionPosition)
    {
        stopSpawning = true;
        foreach(Ball ball in activeBalls)
        {
            StartCoroutine(DelayedExplosion(ball, explosionPosition));
        }
    }

    private IEnumerator DelayedExplosion(Ball ball, Vector3 explosionPosition)
    {
        float waitTime = Random.Range(.1f, .25f);
        yield return new WaitForSeconds(waitTime);
        ball.GetComponent<Explodable>().explode(explosionForce, explosionPosition);
    }
}