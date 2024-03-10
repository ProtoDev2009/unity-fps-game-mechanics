using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPositon;

    void Update()
    {
        transform.position = cameraPositon.position;
    }
}
