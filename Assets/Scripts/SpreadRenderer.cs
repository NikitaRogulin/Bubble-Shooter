using System.Collections.Generic;
using UnityEngine;

public class SpreadRenderer : MonoBehaviour
{
    [SerializeField] private List<LineRenderer> lineRenderer = new List<LineRenderer>(3);
    private Vector2 dir;
    private Vector2 pos;
    private float padding;
    private float angle;
    public void SetDirection(Vector2 position, Vector2 direction, float angle)
    {
        pos = position;
        dir = direction;
        this.angle = angle;
        UpdateLine(lineRenderer[1], dir,6);
        if(angle != 0)
        {
            lineRenderer[0].enabled = true;
            lineRenderer[2].enabled = true;
            UpdateLine(lineRenderer[0], Quaternion.Euler(0,0, angle / 2f) * dir,2);
            UpdateLine(lineRenderer[2], Quaternion.Euler(0, 0, -angle / 2f) * dir,2);
        }
        else
        {
            lineRenderer[0].enabled = false;
            lineRenderer[2].enabled = false;
        }
    }
    public void SetPadding(float indent)
    {
        padding = indent;
    }
    private void UpdateLine(LineRenderer lineRenderer, Vector2 direction, int countBounce)
    {
        List<Vector3> positions = new List<Vector3>();

        Vector2 currentPos = pos;
        Vector2 currentDir = direction;
        for (int i = 0; i < countBounce; i++)
        {
            positions.Add(currentPos);
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir);
            if (hit)
            {
                currentDir = Vector2.Reflect(currentDir, hit.normal);
                currentPos = hit.point + hit.normal * 0.01f;
                if (hit.collider.CompareTag("Wall"))
                {
                    currentPos += hit.normal * padding;
                }
                positions.Add(currentPos);
                if (hit.collider.CompareTag("Ball"))
                {
                    break;
                }
            }
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
