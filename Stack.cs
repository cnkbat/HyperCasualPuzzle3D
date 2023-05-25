using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

public class Stack : MonoBehaviour
{
    [Header("Stack Holders")]
    public List<Transform> groundCoinPlacements;
    public List<Transform> firstFloorCoinPlacements;

    public Transform stackEntrance;

    [SerializeField] Transform groundFirstCoinPlacement;
    
    [SerializeField] Transform secondFloorFirstCoinPlacement;

    [Header("Stack State")]
    public bool isStackLocked;

    
    [Header("Coins inside stack")]
    public List<GameObject> coinsInGroundFloor;
    public List<GameObject> coinsInFirstFloor;
    public List<GameObject> coinsInSecondFloor;

    //bool for merge
    bool sidebysideFirstMatch;
    bool sidebysideSecondMatch;
    bool OverTheTopFirstMatch;
    bool OverTheTopSecondMatch;

    void Start()
    {
        groundCoinPlacements.Add(groundFirstCoinPlacement);
        

        //0.kat
        for (int i = 0; i < GameManager.instance.maxNumOfCoinPlacements-1 ; i++)
        {
            Transform nextCoinPlacement = Instantiate(groundFirstCoinPlacement,transform.position,Quaternion.identity);
            nextCoinPlacement.position = new Vector3 (groundCoinPlacements[i].position.x , groundCoinPlacements[i].position.y, 
            groundCoinPlacements[i].position.z + GameManager.instance.coinSize);
            groundCoinPlacements.Add(nextCoinPlacement);
        }
    
    }
    void Update() 
    {
        // ground 
        coinsInGroundFloor = gameObject.transform.Find("firstGroundCoinPlacement").GetComponentInChildren<StackFloor>().coinsInsideFloor;
        coinsInGroundFloor = coinsInGroundFloor.Distinct().ToList();
        coinsInGroundFloor.Sort(SortByPosition);

        // first
        coinsInFirstFloor = gameObject.transform.Find("firstFirstFloorCoinPlacement").GetComponentInChildren<StackFloor>().coinsInsideFloor;
        coinsInFirstFloor = coinsInFirstFloor.Distinct().ToList();
        coinsInFirstFloor.Sort(SortByPosition);

        //second
        coinsInSecondFloor = gameObject.transform.Find("firstSecondFloorCoinPlacement").GetComponentInChildren<StackFloor>().coinsInsideFloor;
        coinsInSecondFloor = coinsInSecondFloor.Distinct().ToList();
        coinsInSecondFloor.Sort(SortByPosition);
        GameManager.instance.currentCheckTimer -= Time.deltaTime;

    }
    void FixedUpdate()
    {
        
        if(GameManager.instance.currentCheckTimer <= 0)
        {

            CheckForUpStacking();
        }

        OverTheTopMerge();

        SideBySideMerge();
        
        BacktoBackMerge();

        LockStack();

    }

    void CheckForUpStacking()
    {
        //GROUND

        for (int i = 0; i < coinsInGroundFloor.Count -1 ; i++)
        {    
            
            Coin theCoinBefore = coinsInGroundFloor[i].GetComponent<Coin>();
            if(!coinsInGroundFloor[i+1]) break;
            if(theCoinBefore.collidingCoins.Count > 0)
            {
                DOTween.Kill(coinsInGroundFloor[i+1]);
                if(theCoinBefore.collidingCoins[theCoinBefore.collidingCoins.Count -1].GetComponent<Coin>().value 
                == coinsInGroundFloor[i+1].GetComponent<Coin>().value )
                {
                    if(theCoinBefore.collidingCoins.Count == 1)
                    {
                        coinsInGroundFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(coinsInGroundFloor[i]);
                    }

                    if(theCoinBefore.collidingCoins.Count == 2)
                    {
                        coinsInGroundFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(theCoinBefore.collidingCoins[0].gameObject);
                    }

                    if(theCoinBefore.collidingCoins.Count == 3)
                    {
                        coinsInGroundFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(theCoinBefore.collidingCoins[1].gameObject);
                    }

                    StartCoroutine(PositionReset());
                }
            }
            else
            {
                if(!coinsInGroundFloor[i+1]) break;
                if(theCoinBefore.GetComponent<Coin>().value == coinsInGroundFloor[i+1].GetComponent<Coin>().value )
                {
                    DOTween.Kill(coinsInGroundFloor[i+1]);
                    
                    coinsInGroundFloor[i+1].GetComponent<Coin>().RotateAndRetag(theCoinBefore.gameObject);

                    StartCoroutine(PositionReset());
                }
            }
        }

       // FIRST FLOOR
        for (int i = 0; i < coinsInFirstFloor.Count -1 ; i++)
        {    
            Coin theCoinBefore = coinsInFirstFloor[i].GetComponent<Coin>();
            if(!coinsInFirstFloor[i+1]) break;
            if(theCoinBefore.collidingCoins.Count > 0)
            {
                DOTween.Kill(coinsInFirstFloor[i+1]);
                if(theCoinBefore.collidingCoins[theCoinBefore.collidingCoins.Count -1].GetComponent<Coin>().value 
                == coinsInFirstFloor[i+1].GetComponent<Coin>().value )
                {
                    if(theCoinBefore.collidingCoins.Count == 1)
                    {
                        coinsInFirstFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(coinsInFirstFloor[i]);
                    }

                    if(theCoinBefore.collidingCoins.Count == 2)
                    {
                        coinsInFirstFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(theCoinBefore.collidingCoins[0].gameObject);
                    }

                    if(theCoinBefore.collidingCoins.Count == 3)
                    {
                        coinsInFirstFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(theCoinBefore.collidingCoins[1].gameObject);
                    }

                    StartCoroutine(PositionReset());
                }
            }
            else
            {
                if(!coinsInFirstFloor[i+1]) break;
                if(theCoinBefore.GetComponent<Coin>().value == coinsInFirstFloor[i+1].GetComponent<Coin>().value )
                {
                    DOTween.Kill(coinsInFirstFloor[i+1]);
                    
                    coinsInFirstFloor[i+1].GetComponent<Coin>().RotateAndRetag(theCoinBefore.gameObject);

                    StartCoroutine(PositionReset());
                }
            }
        }

        //SECOND FLOOR
        for (int i = 0; i < coinsInSecondFloor.Count -1 ; i++)
        {    
            
            Coin theCoinBefore = coinsInSecondFloor[i].GetComponent<Coin>();
            if(theCoinBefore.collidingCoins.Count > 0)
            {
                if(!coinsInSecondFloor[i+1]) break;
                DOTween.Kill(coinsInSecondFloor[i+1]);
                if(theCoinBefore.collidingCoins[theCoinBefore.collidingCoins.Count -1].GetComponent<Coin>().value 
                == coinsInSecondFloor[i+1].GetComponent<Coin>().value )
                {
                    if(theCoinBefore.collidingCoins.Count == 1)
                    {
                        coinsInSecondFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(coinsInSecondFloor[i]);
                    }

                    if(theCoinBefore.collidingCoins.Count == 2)
                    {
                        coinsInSecondFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(theCoinBefore.collidingCoins[0].gameObject);
                    }

                    if(theCoinBefore.collidingCoins.Count == 3)
                    {
                        coinsInSecondFloor[i+1].GetComponent<Coin>().
                        RotateAndRetag(theCoinBefore.collidingCoins[1].gameObject);
                    }

                    StartCoroutine(PositionReset());
                }
            }
            else
            {
                if(!coinsInSecondFloor[i+1]) break;
                if(theCoinBefore.GetComponent<Coin>().value == coinsInSecondFloor[i+1].GetComponent<Coin>().value )
                {
                    DOTween.Kill(coinsInSecondFloor[i+1]);
                    
                    coinsInSecondFloor[i+1].GetComponent<Coin>().RotateAndRetag(theCoinBefore.gameObject);

                    StartCoroutine(PositionReset());
                }
            }
            GameManager.instance.currentCheckTimer = GameManager.instance.startingCheckTimer;
        }
    }

    void OverTheTopMerge()
    {
        for (int i = 0; i < coinsInGroundFloor.Count; i++)
        {
            for(int a = 0 ; a < coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Count; a++)
            {
                if(0 == coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Count 
                || 1 == coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Count) break;

                if(a+1 > 1) break ;
                if(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1] == null ) break;
                if(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0] == null ) break;
                if(!coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Contains(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1])) break;
                if(!coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Contains(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0])) break;  

                if(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Contains(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0]) && 
                    coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0].GetComponent<Coin>().value == 
                        coinsInGroundFloor[i].GetComponent<Coin>().value)
                {
                    OverTheTopFirstMatch = true;
                }

                if(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins.Contains(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1]) && 
                    coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1].GetComponent<Coin>().value == 
                        coinsInGroundFloor[i].GetComponent<Coin>().value)
                {
                    OverTheTopSecondMatch = true;
                }

                if(OverTheTopFirstMatch && OverTheTopSecondMatch)
                {
                    if(!coinsInGroundFloor[i].GetComponent<Coin>().canMerge) return;
                    if(!coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0].GetComponent<Coin>().canMerge) return;
                    if(!coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1].GetComponent<Coin>().canMerge) return;

                    StartCoroutine(MergeCoins(coinsInGroundFloor[i], coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0],
                    coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1]));

                    OverTheTopFirstMatch = false;
                    OverTheTopSecondMatch = false;

                    if(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0] && coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1])
                    {
                        Debug.Log("last if before break");
                        coinsInFirstFloor.Remove(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[0]);
                        coinsInSecondFloor.Remove(coinsInGroundFloor[i].GetComponent<Coin>().collidingCoins[1]);
                        coinsInGroundFloor.Remove(coinsInGroundFloor[i]);
                        StartCoroutine(PositionReset());
                    } 
                    break;
                }

                
                OverTheTopFirstMatch = false;
                OverTheTopSecondMatch = false;
                

            } 
        }
    }

    void BacktoBackMerge()
    {
        //ground
        for (int i = 0; i < coinsInGroundFloor.Count-3; i++)
        {

            if(coinsInGroundFloor.Contains(coinsInGroundFloor[i+1]) && coinsInGroundFloor.Contains(coinsInGroundFloor[i+2]))
            {
                if(coinsInGroundFloor[i].GetComponent<Coin>().value == coinsInGroundFloor[i+1].GetComponent<Coin>().value
                    && coinsInGroundFloor[i].GetComponent<Coin>().value == coinsInGroundFloor[i+2].GetComponent<Coin>().value)
                    {
                        if(!coinsInGroundFloor[i].GetComponent<Coin>().canMerge) return;
                        if(!coinsInGroundFloor[i+1].GetComponent<Coin>().canMerge) return;
                        if(!coinsInGroundFloor[i+2].GetComponent<Coin>().canMerge) return;

                        coinsInGroundFloor.Remove(coinsInGroundFloor[i]);
                        coinsInGroundFloor.Remove(coinsInGroundFloor[i+1]);
                        coinsInGroundFloor.Remove(coinsInGroundFloor[i+2]);

                        StartCoroutine(MergeCoins(coinsInGroundFloor[i],coinsInGroundFloor[i+1],coinsInGroundFloor[i+2]));
                        StartCoroutine(PositionReset());

                    }
            }
            else break;
        }
        
        // first floor
        for (int i = 0; i < coinsInFirstFloor.Count-2; i++)
        {

            if(coinsInFirstFloor[i+1] && coinsInFirstFloor[i+2])
            {
                if(coinsInFirstFloor[i].GetComponent<Coin>().value == coinsInFirstFloor[i+1].GetComponent<Coin>().value
                    && coinsInFirstFloor[i].GetComponent<Coin>().value == coinsInFirstFloor[i+2].GetComponent<Coin>().value)
                    {
                        if(!coinsInFirstFloor[i].GetComponent<Coin>().canMerge) return;
                        if(!coinsInFirstFloor[i+1].GetComponent<Coin>().canMerge) return;
                        if(!coinsInFirstFloor[i+2].GetComponent<Coin>().canMerge) return;

                        coinsInFirstFloor.Remove(coinsInFirstFloor[i]);
                        coinsInFirstFloor.Remove(coinsInFirstFloor[i+1]);
                        coinsInFirstFloor.Remove(coinsInFirstFloor[i+2]);
                        
                        StartCoroutine(MergeCoins(coinsInFirstFloor[i],coinsInFirstFloor[i+1],coinsInFirstFloor[i+2]));
                        StartCoroutine(PositionReset());
                        
                    }
            }
        }
        
        // second floor
        for (int i = 0; i < coinsInSecondFloor.Count-2; i++)
        {

            if(coinsInSecondFloor[i+1] && coinsInSecondFloor[i+2])
            {
                if(coinsInSecondFloor[i].GetComponent<Coin>().value == coinsInSecondFloor[i+1].GetComponent<Coin>().value
                    && coinsInSecondFloor[i].GetComponent<Coin>().value == coinsInSecondFloor[i+2].GetComponent<Coin>().value)
                    {
                        if(!coinsInSecondFloor[i].GetComponent<Coin>().canMerge) return;
                        if(!coinsInSecondFloor[i+1].GetComponent<Coin>().canMerge) return;
                        if(!coinsInSecondFloor[i+2].GetComponent<Coin>().canMerge) return;

                        coinsInSecondFloor.Remove(coinsInSecondFloor[i]);
                        coinsInSecondFloor.Remove(coinsInSecondFloor[i+1]);
                        coinsInSecondFloor.Remove(coinsInSecondFloor[i+2]);

                        StartCoroutine(MergeCoins(coinsInSecondFloor[i],coinsInSecondFloor[i+1],coinsInSecondFloor[i+2]));
                        StartCoroutine(PositionReset());
                    }
            }
        }
    }

    void SideBySideMerge()
    {
        // ground floor 
        for (int i = 0; i < coinsInGroundFloor.Count; i++)
        {   

            // all condinitons
            if((0 > GameManager.instance.Stacks.IndexOf(this) - 1)) break;
            if(GameManager.instance.Stacks.Count - 1 < GameManager.instance.Stacks.IndexOf(this) +1 ) break;

            if(!GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1]) break;
            if(!GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) +1]) break;
 
            if(!GameManager.instance.Stacks.Contains(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1])) break;
            if(!GameManager.instance.Stacks.Contains(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1])) break;

            if(!(i < GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor.Count)) break;
            if(!(i < GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor.Count)) break;

            if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor.Contains
                    (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor[i]) &&
                    GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor.Contains
                    (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor[i]))
            {
                if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor[i] &&
                    GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor[i])
                {
                    if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor[i].GetComponent<Coin>().value ==
                        coinsInGroundFloor[i].GetComponent<Coin>().value)
                    {
                        sidebysideFirstMatch = true;
                    }
                    if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor[i].GetComponent<Coin>().value ==
                        coinsInGroundFloor[i].GetComponent<Coin>().value)
                    {
                        sidebysideSecondMatch = true;
                    }

                    if (sidebysideFirstMatch && sidebysideSecondMatch)
                    {
                        
                        SideBySideMergeFinisher(coinsInGroundFloor[i], GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor[i],
                            GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor[i], "firstGroundCoinPlacement");

                        
                        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor.Remove(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInGroundFloor[i]);
                        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor.Remove(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInGroundFloor[i]);

                        coinsInGroundFloor.Remove(coinsInGroundFloor[i]);

                        sidebysideFirstMatch = false;
                        sidebysideSecondMatch = false;
                        break;
                    }
                    else
                    {
                        sidebysideFirstMatch = false;
                        sidebysideSecondMatch = false;
                        break;
                    }
                }
                else
                {
                    sidebysideFirstMatch = false;
                    sidebysideSecondMatch = false;
                    break;
                }
            }

            else
            {
                sidebysideFirstMatch = false;
                sidebysideSecondMatch = false;
            }
        }

        // first floor
        for (int i = 0; i < coinsInFirstFloor.Count; i++)
        {
            if((0 > GameManager.instance.Stacks.IndexOf(this) - 1)) break;
            if(GameManager.instance.Stacks.Count - 1 < GameManager.instance.Stacks.IndexOf(this) +1 ) break;

            if(!GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1]) break;
            if(!GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) +1]) break;
 
            if(!GameManager.instance.Stacks.Contains(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1])) break;
            if(!GameManager.instance.Stacks.Contains(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1])) break;
            if(!(i < GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor.Count)) break;
            if(!(i < GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor.Count)) break;

            if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1] &&
                GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1])
            {
                if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor.Contains
                    (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor[i]) &&
                    GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor.Contains
                    (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor[i]))
                {
                    if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor[i].GetComponent<Coin>().value ==
                        coinsInFirstFloor[i].GetComponent<Coin>().value)
                    {
                        sidebysideFirstMatch = true;
                    }
                    if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor[i].GetComponent<Coin>().value ==
                        coinsInFirstFloor[i].GetComponent<Coin>().value)
                    {
                        sidebysideSecondMatch = true;
                    }

                    if (sidebysideFirstMatch && sidebysideSecondMatch)
                    {
                        SideBySideMergeFinisher(coinsInFirstFloor[i], GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor[i],
                            GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor[i], "firstFirstFloorCoinPlacement");
                        coinsInFirstFloor.Remove(coinsInFirstFloor[i]);
                        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor.Remove(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInFirstFloor[i]);
                        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor.Remove(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInFirstFloor[i]);
                    }

                    else
                    {
                        sidebysideFirstMatch = false;
                        sidebysideSecondMatch = false;
                        break;
                    }
                }
                else
                {
                    sidebysideFirstMatch = false;
                    sidebysideSecondMatch = false;
                    break;
                }
            }
            else
            {
                sidebysideFirstMatch = false;
                sidebysideSecondMatch = false;
                break;
            }
        }

        //second floor
        for (int i = 0; i < coinsInSecondFloor.Count; i++)
        {
            if((0 > GameManager.instance.Stacks.IndexOf(this) - 1)) break;
            if(GameManager.instance.Stacks.Count - 1 < GameManager.instance.Stacks.IndexOf(this) +1 ) break;

            if(!GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1]) break;
            if(!GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) +1]) break;
 
            if(!GameManager.instance.Stacks.Contains(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1])) break;
            if(!GameManager.instance.Stacks.Contains(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1])) break;
            if(!(i < GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor.Count)) break;
            if(!(i < GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor.Count)) break;

            if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1] &&
                GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1])
            {
                if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor.Contains
                    (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor[i]) &&
                    GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor.Contains
                    (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor[i]))
                {
                    if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor[i].GetComponent<Coin>().value ==
                        coinsInSecondFloor[i].GetComponent<Coin>().value)
                    {
                        sidebysideFirstMatch = true;
                    }
                    if (GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor[i].GetComponent<Coin>().value ==
                        coinsInSecondFloor[i].GetComponent<Coin>().value)
                    {
                        Debug.Log("second");
                        sidebysideSecondMatch = true;
                    }

                    if (sidebysideFirstMatch && sidebysideSecondMatch)
                    {
                        SideBySideMergeFinisher(coinsInSecondFloor[i], GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor[i],
                            GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor[i], "firstSecondFloorCoinPlacement");

                        
                        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor.Remove(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].coinsInSecondFloor[i]);
                        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor.Remove(GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].coinsInSecondFloor[i]);
                        coinsInSecondFloor.Remove(coinsInSecondFloor[i]);
                    }

                    else
                    {
                        sidebysideFirstMatch = false;
                        sidebysideSecondMatch = false;
                        break;
                    }
                }
                else
                {
                    sidebysideFirstMatch = false;
                    sidebysideSecondMatch = false;
                    break;
                }
            }

            else
            {
                sidebysideFirstMatch = false;
                sidebysideSecondMatch = false;
                break;
            }
        }
    }

    void SideBySideMergeFinisher(GameObject baseCoin, GameObject leftCoin,GameObject rightCoin, string firstCoinPlacement)
    {
        if(!baseCoin.GetComponent<Coin>().canMerge) return;
        if(!leftCoin.GetComponent<Coin>().canMerge) return;
        if(!rightCoin.GetComponent<Coin>().canMerge) return;

        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) - 1].
            transform.Find(firstCoinPlacement).GetComponentInChildren<StackFloor>().coinsInsideFloor.Remove(leftCoin);
        GameManager.instance.Stacks[GameManager.instance.Stacks.IndexOf(this) + 1].
            transform.Find(firstCoinPlacement).GetComponentInChildren<StackFloor>().coinsInsideFloor.Remove(rightCoin);
        
        StartCoroutine(MergeCoins(baseCoin, leftCoin, rightCoin));
        StartCoroutine(PositionReset());
        
    }

    IEnumerator MergeCoins(GameObject baseCoin, GameObject leftCoin, GameObject rightCoin)
    {
        baseCoin.GetComponent<Coin>().canMerge = false;
        leftCoin.GetComponent<Coin>().canMerge = false;
        rightCoin.GetComponent<Coin>().canMerge = false;

        yield return new WaitForSeconds(GameManager.instance.MergeDelay);

        DOTween.Clear();

        

        GameObject mergedCoin = Instantiate(baseCoin.GetComponent<Coin>().mergedCoinPrefab, baseCoin.transform.position, Quaternion.identity);

        rightCoin.transform.position = GameManager.instance.trashVector;
        leftCoin.transform.position = GameManager.instance.trashVector;
        baseCoin.transform.position = GameManager.instance.trashVector;

        rightCoin.SetActive(false);
        leftCoin.SetActive(false);
        baseCoin.SetActive(false);

        Debug.Log("MERGED");
        
        mergedCoin.tag = "Mergeable";

        int currentCoins = PlayerPrefs.GetInt(Constants.DATA.COIN,0);

        PlayerPrefs.SetInt(Constants.DATA.COIN,currentCoins + 20);

        GameManager.instance.UpdateGameHudText();

    }

    IEnumerator PositionReset()
    {
        yield return new WaitForSeconds(GameManager.instance.PositionResetDelay);

        for (int i = 0; i < coinsInGroundFloor.Count; i++)
        {
            coinsInGroundFloor[i].transform.DOMove(groundCoinPlacements[i].position,GameManager.instance.moveAnimationTime);
        }

        for (int i = 0; i < coinsInFirstFloor.Count; i++)
        {
            
            if(coinsInFirstFloor[i] && i > coinsInGroundFloor.Count)
            {
                coinsInFirstFloor[i].transform.DOMove(groundCoinPlacements[i].position,GameManager.instance.moveAnimationTime);
            }
        }

        for (int i = 0; i < coinsInSecondFloor.Count; i++)
        {
            if(coinsInSecondFloor[i] && i > coinsInFirstFloor.Count)
            {
                coinsInSecondFloor[i].transform.DOMove(firstFloorCoinPlacements[i].position,GameManager.instance.moveAnimationTime);
            }
        }
    }

    static int SortByPosition(GameObject coin1, GameObject coin2)
    {
        return coin1.transform.position.z.CompareTo(coin2.transform.position.z);
    }

    void LockStack()
    {
        if(coinsInGroundFloor.Count == GameManager.instance.maxNumOfCoinPlacements)
        {
            isStackLocked = true;
        }

        else
        {
            isStackLocked = false;
        }
    }


}
