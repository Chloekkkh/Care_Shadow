using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyPath : MonoBehaviour
{
    public List<Transform> wayPoints;
    [SerializeField]
    private bool alwaysDrawPath;
    [SerializeField]
    private bool drawAsLoop;
    [SerializeField]
    private bool drawNumbers;
    public Color debugColour = Color.white;

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (alwaysDrawPath)
        {
            DrawPath();
        }
    }
    public void DrawPath()
    {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;
            labelStyle.normal.textColor = debugColour;
            if (drawNumbers)
                Handles.Label(wayPoints[i].position, i.ToString(), labelStyle);
            //Draw Lines Between Points.
            if (i >= 1)
            {
                Gizmos.color = debugColour;
                Gizmos.DrawLine(wayPoints[i - 1].position, wayPoints[i].position);

                if (drawAsLoop)
                    Gizmos.DrawLine(wayPoints[wayPoints.Count - 1].position, wayPoints[0].position);

            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        if (alwaysDrawPath)
            return;
        else
            DrawPath();
    }
#endif
}
