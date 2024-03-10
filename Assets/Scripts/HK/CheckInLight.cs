using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInLight : MonoBehaviour
{
    public static CheckInLight instance;
    private Transform lightTransform;//光源位置
    private Transform midLightTransform;//塔光位置位置

    [Range(0, 20)]
    public float lightRange = 13f;
    [Range(0, 10)]
    public float midLightRange = 5f;
    [SerializeField] private LayerMask targetLayerMask;//影子投射到的layer
   //[SerializeField] private bool isInLight = true;
    private RaycastHit towerLightHit;
    private RaycastHit midLightHit;
    private void Awake()
    {
        instance = this;
        lightTransform = GameObject.FindGameObjectWithTag("Light").transform;
        midLightTransform = GameObject.FindGameObjectWithTag("MidLight").transform;
        targetLayerMask = LayerMask.GetMask("Ground");
    }
    public bool IsInLight(Transform transform)
    {
       Physics.Raycast(lightTransform.position, lightTransform.forward, out towerLightHit, 100, targetLayerMask);
       Physics.Raycast(midLightTransform.position, midLightTransform.forward, out midLightHit, 100, targetLayerMask);
       if (Vector3.Distance(transform.position, towerLightHit.point) < lightRange || Vector3.Distance(transform.position, midLightHit.point) < midLightRange)
        {
        //Debug.Log("Enemy is in the light,should add collider");
//            Debug.Log("in light");
            return true;
        }
        else
        {
        // Debug.Log("Enemy is not in the light,should delete collider");
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(lightTransform.position, hit.point);
        Gizmos.DrawWireSphere(towerLightHit.point, lightRange);
        Gizmos.DrawWireSphere(midLightHit.point, midLightRange);
    }
}
