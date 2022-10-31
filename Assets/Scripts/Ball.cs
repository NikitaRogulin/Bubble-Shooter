using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Ball : MonoBehaviour
{
    private GameplayController gameplayController;
    private static List<Ball> balls;
    [SerializeField] private SpringJoint2D spring;
    [SerializeField] private Collider2D collider2;

    [SerializeField]
    public Color BallColor
    {
        get => gameObject.GetComponent<SpriteRenderer>().color;
        set => gameObject.GetComponent<SpriteRenderer>().color = value;
    }

    public void Awake()
    {
        gameplayController = GameplayController.GetInstance();
    }
    public Vector2Int GetIndexPos()
    {
        var BallX = transform.position.x / gameplayController.BallSize;
        var BallY = transform.position.y / gameplayController.BallSize;
        return new Vector2Int((int)BallX, (int)BallY);
    }
    public void Fall()
    {
        collider2.enabled = false;
        spring.enabled = false;
        Destroy(gameObject, 4f);
    }
    public List<Ball> SearchSimilarBalls()
    {
        balls = new List<Ball>();
        balls.Add(this);
        ShootRays();
        return balls;
    }

    public LayerMask ignore;
    private void ShootRays()
    {

        var hits = Physics2D.OverlapCircleAll(transform.position, gameplayController.BallSize * 0.7f);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Ball>(out Ball ball))
            {
                if (ball.BallColor == BallColor && !balls.Contains(ball))
                {
                    balls.Add(ball);
                    ball.ShootRays();
                }
            }
        }

        /*
          var hits = Physics2D.OverlapCircleAll(transform.position, gameplayController.BallSize * 0.7f);
        

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Ball>(out Ball ball))
            {
                if (ball.BallColor == BallColor && !balls.Contains(ball))
                {
                    balls.Add(ball);
                    ball.ShootRays();
                }
            }
        }
        */
    }
}
