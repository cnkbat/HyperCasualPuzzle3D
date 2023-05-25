using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DragSystem : MonoBehaviour
{
    public static DragSystem instance;

    public GameObject selectedObject;

    [Header("Drag")]
    [SerializeField] float positiveLimitValue;
    [SerializeField] float negativeLimitValue;
    [SerializeField] float dragSpeed;
    float horizontalPos;


    GameObject[] stacks;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }
    private void Start() 
    {
        stacks = GameObject.FindGameObjectsWithTag("Stack");    
    }

    void Update()
    {
        RaycastHit hit = CastRay();

        if(hit.collider == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if(hit.collider == null)
            {
                return;
            }

            if(hit.collider != null && selectedObject == null )
            {   
                if(hit.collider.CompareTag("Coin"))
                {
                    selectedObject = hit.collider.gameObject.transform.parent.gameObject;

                    GameManager.instance.selectorPrefabsActive = false;

                }
            }

            if(hit.collider.CompareTag("Stack") && selectedObject != null)
            {
                Debug.Log("stack detected");
                if(!hit.collider.gameObject.GetComponent<Stack>().isStackLocked)
                {
                    Debug.Log("stacklocked");
                    selectedObject.transform.DOMove(hit.collider.gameObject.GetComponent<Stack>().stackEntrance.position,
                        GameManager.instance.moveAnimationTime / 3).OnComplete(()=>
                        selectedObject.transform.DOMove
                        (hit.collider.gameObject.GetComponent<Stack>().groundCoinPlacements
                            [hit.collider.gameObject.transform.Find("firstGroundCoinPlacement")
                                .GetComponentInChildren<StackFloor>()
                                    .coinsInsideFloor.Count].position,
                                        GameManager.instance.moveAnimationTime).OnComplete(OnMoveCompleteCallBack));

                    GameManager.instance.moveCounter++;
                }
            }
        }

        foreach (var stack in stacks)
        {
            stack.gameObject.transform.Find("Platform").GetComponent<MeshRenderer>().material = GameManager.instance.platformColor;
        }
        
        if(selectedObject != null)
        {   
            if(selectedObject.CompareTag("Coin"))
            {
                // seçildiğini belli etmek için bişeyler yapcaz
            }

            if(hit.collider == null)
            {
                return;
            }

            else if(hit.collider.gameObject.CompareTag("Stack"))
            {
                hit.collider.gameObject.transform.Find("Platform").GetComponent<MeshRenderer>().material = GameManager.instance.highlightedPlatformColor;
            }
        }
    }

    private void OnMoveCompleteCallBack()
    {
        selectedObject.tag = "Mergeable";
        selectedObject = null;
        GameManager.instance.coinInHand = null;
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    void MoveCoin()
    {

        float halfScreen = Screen.width /2;
        
        float finalXPos = (Input.mousePosition.x) / halfScreen;
    
        horizontalPos = Mathf.Clamp(finalXPos *  dragSpeed , negativeLimitValue, positiveLimitValue);

        selectedObject.gameObject.transform.position = new Vector3(-horizontalPos , selectedObject.transform.position.y,selectedObject.transform.position.z );

    }

}
