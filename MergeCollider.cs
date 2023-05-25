using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MergeCollider : MonoBehaviour
{
    public List<GameObject> currentCollisions;
    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Mergeable"))
        {
            currentCollisions.Add(other.gameObject);
            currentCollisions = currentCollisions.Distinct().ToList();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.CompareTag("Mergeable"))
        {
            currentCollisions.Remove(other.gameObject);
            currentCollisions = currentCollisions.Distinct().ToList();
        }
    }
}
