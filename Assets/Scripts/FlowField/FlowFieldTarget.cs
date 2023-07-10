using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldTarget : MonoBehaviour
{
    public Transform trans;
    public int lastGridIndex = -1;
    public int lastSectorIndex = -1;

    private void Awake()
    {
        trans = transform;
    }

}
