using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private ShootingBall shootingBallPrefab;
    [SerializeField] private Camera cam;
    [SerializeField] private MapSpawner mapSpawner;
    private static GameplayController instance;
    private Vector2 mapSize;
    private ShootingBall currentBall;
    public float BallSize { get; set; }

    public event Action<Ball> OnFall;

    public static GameplayController GetInstance()
    {
        return instance;
    }
    public void InstalPosShootingBall()
    {
        var startPos = new Vector3(0, -3, 1);
        currentBall = Instantiate(shootingBallPrefab, startPos, Quaternion.identity);
        currentBall.OnWeakShoot += WeakForceCollision;
        currentBall.OnStrongShoot += StrongForceCollision;
        currentBall.transform.localScale = new Vector3(BallSize, BallSize, 1);
        currentBall.InstColorBall();

        var borders = CameraUtils.CalculateScreenSize();
        currentBall.SetBorders(borders.x, borders.y);
    }

    void Start()
    {
        mapSize = CameraUtils.CalculateScreenSize();

        if (instance != null)
            Destroy(this);
        instance = this;
    }

    void Update()
    {

    }
    public Vector3 SearchStartPosition()
    {
        var startPosX = cam.transform.position.x - mapSize.x / 2;
        var startPosY = cam.transform.position.y + mapSize.y / 2;
        return new Vector3(startPosX, startPosY, transform.position.z);
    }
    public void WeakForceCollision(Ball mapBall)
    {
        var color = currentBall.BallColor;
        var pos = currentBall.transform.position;
        Destroy(currentBall.gameObject);
        var ball = mapSpawner.SpawnBall(new Vector2(pos.x, pos.y + BallSize / 2), color);
        DeleteSimilarBalls(ball);
    }
    public void StrongForceCollision(Ball mapBall)
    {
        var color = currentBall.BallColor;
        var pos = mapBall.transform.position;
        Destroy(currentBall.gameObject);
        OnFall?.Invoke(mapBall);
        mapBall.Fall();
        var ball = mapSpawner.SpawnBall(new Vector2(pos.x, pos.y + BallSize / 2), color);
        DeleteSimilarBalls(ball);
    }
    private void DeleteSimilarBalls(Ball ball)
    {
        List<Ball> similarBalls;
        similarBalls = ball.SearchSimilarBalls();
        if(similarBalls.Count >= 3)
        {
            for (int i = 0; i < similarBalls.Count; i++)
            {
                OnFall?.Invoke(similarBalls[i]);
                similarBalls[i].Fall();
            }
        }
    }
}
