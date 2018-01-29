using UnityEngine;

public class Vector2Utils
{
    public static Vector2 GetSmallVariation(Vector2 direction, float jitter)
    {
        float degree = Vector2.Angle(Vector2.up, direction);
        float radian = (degree + 90) * Mathf.Deg2Rad;
        radian += Random.Range(-jitter, jitter);
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}