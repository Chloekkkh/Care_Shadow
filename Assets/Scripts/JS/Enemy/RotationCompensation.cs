using UnityEngine;

public class RotationCompensation : MonoBehaviour
{
    void Update()
    {
        
        if (transform.parent != null) 
        {

            /*用这个方法不行，不行的地方在于 在AR项目中player没有父物体，所以直接用Quternion.Euler（new Vector3D（））改变世界坐标不会有bug，但是这个项目shadow绑定了副物体，所以每次副物体移动都会有问题。
            // gain parent.rotation。y
            float parentYRotation = transform.parent.eulerAngles.y;
            Debug.Log(parentYRotation);
            // apply a -Y
            transform.rotation = Quaternion.Euler(0,-parentYRotation,0);
            */

            // gain parent.rotation。y
            float parentYRotation = transform.parent.eulerAngles.y;
          //  Debug.Log(parentYRotation);
            // apply a -Y

            //transform.eulerAngles = new Vector3(0, -parentYRotation, 0);

            //以后想改一定要注意local和global
            transform.localEulerAngles = new Vector3(0, -parentYRotation, 0);
        }
    }
}
