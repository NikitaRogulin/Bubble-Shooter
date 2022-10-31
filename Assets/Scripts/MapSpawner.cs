using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private Ball ballPrefab;
    private Vector2 mapSize;
    private GameplayController gameController; 
    void Start()
    {
        gameController = GameplayController.GetInstance();
        TextAsset asset = Resources.Load("test") as TextAsset;
        string[] mapLines = asset.text.Replace("\r", "").Split('\n');

        mapSize = CameraUtils.CalculateScreenSize();

        var ballSize = CalculateBallScale(mapLines[0].Length);
        gameController.BallSize = ballSize;
        //gameController.map = new Ball[mapLines[0].Length,(int)(mapSize.y / ballSize)];
        
        var startPosSpawn = gameController.SearchStartPosition();

        gameController.InstalPosShootingBall();

       
        for (int y = 0; y < mapLines.Length; y++)
        {
            for (int x = 0; x < mapLines[y].Length; x++)
            {
                var pos = new Vector3(ballSize * x + ballSize/2, -ballSize * y - ballSize / 2, 1) + startPosSpawn;
                char current = mapLines[y][x];
                var ball = SpawnBall(pos, colors[int.Parse(current.ToString())]);
                //gameController.map[x, y] = ball;
            }
        }
    }
    public Ball SpawnBall(Vector2 pos, Color color) 
    {
        var newBall = Instantiate(ballPrefab, pos, Quaternion.identity);
        newBall.transform.localScale = new Vector3(gameController.BallSize,gameController.BallSize,1);
        newBall.BallColor = color;
        newBall.GetComponent<SpringJoint2D>().connectedAnchor = pos;
        return newBall;
    }
    private float CalculateBallScale(int rowSize)
    {
        return mapSize.x / rowSize;
    }
    
}
