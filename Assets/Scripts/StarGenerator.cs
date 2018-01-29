using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    public GameObject starObject;
    public int numberOfStars;

    private float width;
    private float height;

    public void Awake()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        width = worldPoint.x;
        height = worldPoint.y;

        for(int i = 0; i < numberOfStars; i++)
        {
            Instantiate(starObject, GetRandomPosition() ,Quaternion.identity,transform);
        }
    }

    public Vector3 GetRandomPosition()
    {
        float x = Random.Range (-width, width);
        float y = Random.Range (-height, height);

        return new Vector3(x, y, 0);
    }
}