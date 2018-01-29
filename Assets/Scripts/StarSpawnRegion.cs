using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawnRegion : MonoBehaviour
{
    public int numberOfStars;

    private List<Star> pendingStars;

    public void Awake()
    {
    }
}