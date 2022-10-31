
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public partial class ShootingBall : Ball
{
    private const float MOVE_SPEED = 10f;
    private const float ANGLE_SPREAD = 25;

    [SerializeField] private SpreadRenderer spreadRenderer;
    [SerializeField] private List<Color> colors;

    private bool isActive = false;
    private float force;
    private Vector3 startPos;
    private Vector3 moveDirection;
    private int countCollision = 0;
    private SpringJoint2D springJoint;
    private Rigidbody2D rb;

    private float h;
    private float w;

    public event Action<Ball> OnWeakShoot;
    public event Action<Ball> OnStrongShoot;

    private void OnEnable()
    {

        springJoint = GetComponent<SpringJoint2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        InstColorBall();
    }
    private void Update()
    {
        if (!isActive)
        {
            Pull();
        }
        Move();
    }
    private void Pull()
    {
        if (Input.touchCount > 0)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Collider2D collider = Physics2D.OverlapPoint(mousePos);

                if (collider && collider.gameObject == gameObject)
                {
                    startPos = gameObject.transform.position;
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved && startPos != Vector3.zero)
            {
                var offset = mousePos - startPos;
                offset.z = 0;

                if (offset.magnitude >= 1)
                {
                    offset = offset.normalized;

                }
                force = offset.magnitude;
                gameObject.transform.position = startPos + offset;

                if (force >= 1)
                {
                    spreadRenderer.SetDirection(startPos, -offset, ANGLE_SPREAD);
                }
                else
                {
                    spreadRenderer.SetDirection(startPos, -offset, 0);
                }
                spreadRenderer.SetPadding(transform.localScale.x / 2);
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended && startPos != Vector3.zero)
            {
                gameObject.transform.position = startPos;

                var offset = mousePos - startPos;
                offset.z = 0;

                if (offset.magnitude > 1)
                {
                    offset = offset.normalized;
                }

                isActive = true;

                if (force >= 1)
                {
                    float randomAngle = Random.Range(-ANGLE_SPREAD / 2, ANGLE_SPREAD / 2);
                    offset = Quaternion.Euler(0, 0, -randomAngle / 2f) * offset;
                }
                moveDirection = -offset;
            }
        }
    }
    private void Move()
    {
        transform.Translate(moveDirection * force * MOVE_SPEED * Time.deltaTime);
        var ballSize = transform.localScale.x / 2;

        if (transform.position.x - ballSize < -w / 2 || transform.position.x + ballSize > w / 2)
        {
            moveDirection.x *= -1;
        }
        if (transform.position.y + ballSize > h / 2)
        {
            moveDirection.y *= -1;
        }
    }
    public void SetBorders(float width, float height)
    {
        h = height;
        w = width;
    }
    public void InstColorBall()
    {
        BallColor = colors[UnityEngine.Random.Range(0, colors.Count - 1)];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out Ball target))
        {
            if (force >= 1)
            {
                OnStrongShoot?.Invoke(target);
            }
            else
            {
                OnWeakShoot?.Invoke(target);
            }
        }
    }
}
