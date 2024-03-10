using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerController : MonoBehaviour
{
    public static TowerController instance;
    // tower rotate
    public Vector3 rotateAxis = new Vector3(0, 1, 0);
    public float rotateSpeed = 10f;

    // tower light range
    // public Transform lightTransform;
    // public LayerMask groundLayerMask;
    // [Range(0, 20)]
    // public float lightRange = 13f;
    // private RaycastHit hit;

    private void Start()
    {
        instance = this;
    }

    void Update()
    {
        TowerRotate();
        //ComputeLightRange();
    }
    
    private void TowerRotate()
    {
        transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
    }
}
