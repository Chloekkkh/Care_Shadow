using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public float rotationSpeed = 500f; // 旋转速度，可以根据需要调整

    void Update()
    {
        // 使飞镖围绕其前进方向（Z轴）旋转
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
