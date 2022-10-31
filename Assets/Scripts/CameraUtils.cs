using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraUtils
{
    private static Camera cam;
    public static Vector2 CalculateScreenSize()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
        var width = cam.aspect * cam.orthographicSize * 2;
        var height = cam.orthographicSize * 2;
        return new Vector2(width, height);
    }
}
