using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldTarget : MonoBehaviour
{
    public Transform trans;
    public int lastGridIndex;

    private void Awake()
    {
        trans = transform;
    }

}
