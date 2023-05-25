using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StackFloor : MonoBehaviour
{
    public int floorValue;

    public List<GameObject> coinsInsideFloor;

    BoxCollider BoxCollider;

    private void Awake() 
    {
        BoxCollider = GetComponent<BoxCollider>();
    }

    private void Update() 
    {
        for (int i = 0; i < coinsInsideFloor.Count; i++)
        {
            if(!IsInBounds(coinsInsideFloor[i].gameObject.transform.position, BoxCollider.bounds))
            {
                coinsInsideFloor.Remove(coinsInsideFloor[i].gameObject);
                coinsInsideFloor = coinsInsideFloor.Distinct().ToList();
                coinsInsideFloor.Sort(SortByPosition);
            }
        }    
    }
    
    void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.CompareTag("Mergeable"))
        {
            if(IsInBounds(other.gameObject.transform.position, BoxCollider.bounds))
            {
                coinsInsideFloor.Add(other.gameObject);
                coinsInsideFloor = coinsInsideFloor.Distinct().ToList();
                coinsInsideFloor.Sort(SortByPosition);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.CompareTag("Mergeable") || other.gameObject.CompareTag("Coin"))    
        {
            coinsInsideFloor.Remove(other.gameObject);
            coinsInsideFloor = coinsInsideFloor.Distinct().ToList();
        }   
    }
    private bool IsInBounds(Vector3 position, Bounds bounds)
    {
        return bounds.Contains(position);
    }

    static int SortByPosition(GameObject coin1, GameObject coin2)
    {
        return coin1.transform.position.z.CompareTo(coin2.transform.position.z);
    }
}
