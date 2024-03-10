using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class InteractiveShadows : MonoBehaviour
{
    [SerializeField] private Transform shadowTransform;//object shadow
    [SerializeField] private Transform lightTransform;//light position
    private LightType lightType;//light type
    private Transform targetTransform;
    [SerializeField] private LayerMask targetLayerMask;//shadow cast on the layer
    [SerializeField] private Vector3 extrusionDirection = Vector3.zero;//shadow extrusion direction
    //[SerializeField]private float shadowTransformoffset = 1f;//shadow offset


    private Vector3[] objectVerices;//object vertices
    private Vector3[] shaderVertices;//object vertices

    private Mesh shadowColliderMesh;//  shadow collider mesh
    private MeshCollider shadowCollider;//shadow collider

    //public GameObject shadowGameObject;


    //light range
    [SerializeField] private bool isInLight = true;
    [Range(0, 20)]
    public float lightRange = 13f;
    private RaycastHit hit;

    public CheckInLight checkInLight;


    void Start()
    {

    }

    private void Awake()
    {
        checkInLight  = GetComponent<CheckInLight>();
        shadowTransform = transform.Find("Shadow").transform;
        lightTransform = GameObject.FindGameObjectWithTag("Light").transform;
        InitializeShadowCollider();
        lightType = lightTransform.GetComponent<Light>().type;
        objectVerices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();//object mesh verticesDistinct()去重并转化成数组
        shadowColliderMesh = new Mesh();

    }

    private void Update()
    {

        //shadowTransform.position = transform.position - transform.forward * shadowTransformoffset;
        shadowTransform.position = transform.position;
        
    }

    private void FixedUpdate()
    {
        IsInLight();
        if(TransformHasChanged())
        {

            shadowColliderMesh.vertices = GetShadowColliderMeshVertices();
            shadowCollider.sharedMesh = shadowColliderMesh;//make collider mesh
        }
        
    }


    //初始化阴影collider
    private void InitializeShadowCollider()
    {
        GameObject shadowGameObject = shadowTransform.gameObject;
        //shadowGameObject.hideFlags = HideFlags.HideInHierarchy;
        shadowCollider = shadowGameObject.AddComponent<MeshCollider>();
        shadowCollider.convex = true;
        shadowCollider.isTrigger = true;
    }

    //compute the vertices of the shadow collider mesh
    private Vector3[] GetShadowColliderMeshVertices()
    {
        Vector3[] vertices = new Vector3[2 * objectVerices.Length];
        Vector3 raycastDirection = lightTransform.forward;
        
        Color[] colors = new Color[vertices.Length];

        if(isInLight)
        {
            for (int i = 0; i < objectVerices.Length; i++)
            {
                Vector3 vertice = transform.TransformPoint(objectVerices[i]);//object vertices
                if (lightType != LightType.Directional)
                {
                    raycastDirection = vertice - lightTransform.position;//
                }
                
                vertices[i] = ComputeIntersectionPoint(vertice, raycastDirection);//compute the intersection point between the ray and the ground
                vertices[i + objectVerices.Length] = ComputeExtrusionPoint(vertice, vertices[i]);//compute the extrusion point of the shadow
            }
        }
        else
        {
             for (int i = 0; i < objectVerices.Length; i++)
            {
                Vector3 vertice = transform.TransformPoint(objectVerices[i])*10f;//object vertices
                if (lightType != LightType.Directional)
                {
                    raycastDirection = vertice - lightTransform.position;
                }
                vertices[i] = ComputeIntersectionPoint(vertice, raycastDirection);//compute the intersection point between the ray and the ground
                vertices[i + objectVerices.Length] = ComputeExtrusionPoint(vertice, vertices[i]);//compute the extrusion point of the shadow
            }
        }
        

        return vertices;
    }




    //compute the intersection point between the ray and the ground
    private Vector3 ComputeIntersectionPoint(Vector3 fromPosition, Vector3 direction)
    {
        RaycastHit hit;
        if(Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, targetLayerMask))
        {
            return hit.point - transform.position;
        }
        return fromPosition + direction * 100 - transform.position;
    }

    //compute the extrusion point of the shadow
    private Vector3 ComputeExtrusionPoint(Vector3 objectVertexPosition, Vector3 shadowPointPosition)
    {
        if (extrusionDirection.sqrMagnitude == 0)
        {
            return objectVertexPosition - transform.position;
        }
        return shadowPointPosition + extrusionDirection;
    }

    //check if the transform has changed
    private bool TransformHasChanged()
    {
        return transform.hasChanged;
    }

    private void IsInLight()
    {
       
       if (CheckInLight.instance.IsInLight(transform))
        {
        //            Debug.Log("Enemy is in the light,should add collider");
            isInLight = true;
        }
        else
        {
      //      Debug.Log("Enemy is not in the light,should delete collider");
            isInLight = false;
        }
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     //Gizmos.DrawLine(lightTransform.position, hit.point);
    //     Gizmos.DrawWireSphere(hit.point, lightRange);
    // }
}
