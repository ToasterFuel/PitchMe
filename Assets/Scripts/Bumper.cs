using System.Collections;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public enum PositionEnum
    {
        TOP,
        BOTTOM
    }

    public float waitTime;
    public PositionEnum position;

    public void Awake()
    {
        int multiplier = 1;
        if(position == PositionEnum.BOTTOM)
            multiplier = -1;

        float topOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y;
        transform.position = new Vector3(0, multiplier * topOfScreen, 0);
        StartCoroutine(SpawnBumper());
    }

    private IEnumerator SpawnBumper()
    {
        yield return new WaitForSeconds(waitTime);

        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("spawn");
    }
}