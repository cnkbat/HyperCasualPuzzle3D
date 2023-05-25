using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class Coin : MonoBehaviour
{
    public int value;

    [Header("Merge Related")]
    public GameObject mergedCoinPrefab;

    public List<GameObject> collidingCoins;
    public GameObject coinModelPart;
    public bool canMerge;
    private void Start() 
    {
        canMerge = true;    
    }
    void FixedUpdate() 
    {
        if(tag== "Mergeable")
        {   
            collidingCoins = GetComponentInChildren<MergeCollider>().currentCollisions;  
            collidingCoins = collidingCoins.Distinct().ToList();  
        }
    }

    public void RotateAndRetag(GameObject theCoinBefore)
    {
        
        StartCoroutine(Retag(theCoinBefore));

    }

    IEnumerator Retag(GameObject theCoinBefore)
    {
        if(transform.parent != null) 
            transform.parent = null;

        transform.DOMove(new Vector3(theCoinBefore.transform.position.x, theCoinBefore.transform.position.y + GameManager.instance.coinSize/ 4,
                    theCoinBefore.transform.position.z),
                        GameManager.instance.moveAnimationTime);

        
        coinModelPart.transform.DORotate(GameManager.instance.rotationVector,
                            GameManager.instance.rotationAnimTime,
                                RotateMode.FastBeyond360);
        
        
        yield return new  WaitForSeconds(GameManager.instance.tagDelay);
        gameObject.tag = "Mergeable";
    }
}
