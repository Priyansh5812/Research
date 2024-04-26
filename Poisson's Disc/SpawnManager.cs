using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    public float radius;
    public Vector2 regionSize;
    public List<Vector3> points;
    public int maxItr;
    public int c;

    private void OnValidate()
    {
        points = BlueNoise.GeneratePoints(radius , regionSize.x , regionSize.y , maxItr , this.transform.position);
    }

    private void OnDrawGizmos()
    {

        if (points == null)
        {
            return;
        }
        Gizmos.DrawWireCube(this.transform.position + (new Vector3(regionSize.x , 0f , regionSize.y)/2), new Vector3(regionSize.x, 0, regionSize.y));

        foreach (var i in points)
        {   
            Gizmos.DrawSphere(i, 1);
        }
      
    }
}
