using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    private void Update()
    {
        Rotate();
    }
    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime),Space.World);
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }
}
