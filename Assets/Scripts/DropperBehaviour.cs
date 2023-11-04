using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class DropperBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector2 bounds;
    [SerializeField] private int maxSpawnTier;
    [SerializeField] private Timer replenishTimer;
    [SerializeField] private Transform dropPoint;
    [SerializeField] private Transform nextBallPoint;
    private bool ballDropped;
    private Transform dropBall;
    private Transform nextBall; 
    [SerializeField] private GameObject ball;
    [SerializeField] private GameAndUIManager gameAndUIManager;

    private float ballSizeOffset;

    private void Start()
    {
        dropBall = SpawnBallFrozen(0, dropPoint.position);
        nextBall = SpawnBallFrozen(Random.Range(0, maxSpawnTier + 1), nextBallPoint.position);
    }

    void Update()
    {
        replenishTimer.Tick(Time.deltaTime);
        if (replenishTimer.Finished() && ballDropped)
            ReplenishBall();
        if (dropBall != null)
            dropBall.position = dropPoint.position;
        Move();

        if (ballDropped) return;
        if (Input.GetButtonDown("Fire1"))
        {
            DropBall();
        }
        else if (Input.GetButtonDown("Fire2")) 
        {
            SwapBalls();
        }
    }

    private void SpawnBall() 
    {
        nextBall = SpawnBallFrozen(Random.Range(0, maxSpawnTier + 1), nextBallPoint.position);
    }

    private void SwapBalls() 
    {
        Transform dummy = nextBall;
        nextBall = dropBall;
        dropBall = dummy;
        Vector3 dummyPosition = nextBall.position;
        nextBall.position = dropBall.position;
        dropBall.position = dummyPosition;
    }

    private void DropBall() 
    {
        GetComponent<AudioSource>().Play();
        dropBall.GetComponent<BallBehaviour>().FreezeBall(false);
        dropBall = null;
        replenishTimer.Reset();
        ballDropped = true;
    }

    private Transform SpawnBallFrozen(int tier, Vector3 position) 
    {
        GameObject spawnedBall = Instantiate(ball, position, Quaternion.identity);
        BallBehaviour _ballBehaviour = spawnedBall.GetComponent<BallBehaviour>();
        _ballBehaviour.SetTier(tier);
        _ballBehaviour.FreezeBall(true);
        return spawnedBall.transform;
    }

    private void Move()
    {
        float offset = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (dropBall != null)
            ballSizeOffset = dropBall.transform.localScale.x / 2;
        float clampedOffset = Mathf.Clamp(transform.position.x + offset, bounds.x + ballSizeOffset, bounds.y - ballSizeOffset);
        transform.position = new Vector3(clampedOffset, transform.position.y, 0);
    }

    private void ReplenishBall() 
    {
        dropBall = nextBall;
        SpawnBall();
        ballDropped = false;
    }
}