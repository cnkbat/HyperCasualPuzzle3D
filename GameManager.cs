using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [Header("Stack Related")]
    public int maxNumOfCoinPlacements;
    public int coinSize;
    public List<Stack> Stacks;

    public float startingCheckTimer;
    public float currentCheckTimer;
    public float PositionResetDelay;

    [Header("StackColors")]
    public Material platformColor;

    public Material highlightedPlatformColor;

    [Header ("Merge Related")]
    
    public int numOfCoinsToMerge;

    public Vector3 trashVector;

    public float MergeDelay;

    [Header ("Coin Related")]
    public float destroyDelay;
    public float moveAnimationTime;
    
    public float rotationAnimTime;

    public Vector3 rotationVector;

    public GameObject[] allCoinsInScene;

    public float tagDelay;

    [Header("Level System")]
    public bool endlessLevel;

    public bool gameHasEnded;

    public int winCondition;

    public int currentWinCounter;

    [Header("Endless Level")]

    public List<int> allPossibleTargets;

    private int targetIndex; 
    public int targetValue;

    public TMP_Text targetValueText;

    [Header("Levels")]

    public TMP_Text currentLevelText;

    public List<int> levelSystemTargets;

    public TMP_Text levelSystemTargetValuesText;


    [Header("Coin Throwing System")]

    bool firstSpawns;
    public GameObject coinInHand;

    public GameObject nextCoinInHand;

    public List<GameObject> possibleSpawnCoinPrefabs;
    public Transform coinInHandPos;
    public Transform nextCoinHandPos;

    int throwingRandomizer;


    [Header("UI")]

    [SerializeField] GameObject playerHud;

    [SerializeField] GameObject endHud;
    
    [SerializeField] List<GameObject> stars;


    [Header("Scoring")]

    public int moveCounter;

    public GameObject starIcon;

    public int coin;

    public TMP_Text coinText;

    [SerializeField] int scoringStage1;

    [SerializeField] int scoringStage2;

    [Header("Skills")]

    List<GameObject> selectorPrefabs;
    public  bool selectorPrefabsActive;

    public TMP_Text deleteSkillText;
    public TMP_Text selectorSkillText;
    void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }

        currentCheckTimer = startingCheckTimer;

        gameHasEnded = false;   

        firstSpawns = true;

        if(endlessLevel)
        {
            targetValue = allPossibleTargets[0];
            targetValueText.text = targetValue.ToString();
        }

        else
        {
            currentLevelText.text = SceneManager.GetActiveScene().name;

            if(levelSystemTargets.Count == 3)
            {
                targetValueText.text = levelSystemTargets[0].ToString()+ " " + levelSystemTargets[1].ToString() + " " 
                    + levelSystemTargets[2].ToString();
            }
            else if(levelSystemTargets.Count == 5)
            {
                targetValueText.text = levelSystemTargets[0].ToString()+ " " + levelSystemTargets[1].ToString() + " " 
                    + levelSystemTargets[2].ToString() +" " + levelSystemTargets[3].ToString() + " " 
                    + levelSystemTargets[4].ToString();
            }
            else if(levelSystemTargets.Count == 6)
            {
                targetValueText.text = levelSystemTargets[0].ToString()+ " " + levelSystemTargets[1].ToString() + " " 
                    + levelSystemTargets[2].ToString() +" " + levelSystemTargets[3].ToString() + " " 
                    + levelSystemTargets[4].ToString() + " " 
                    + levelSystemTargets[5].ToString();
            }
            // aybüke hedefleri yazınca buradaki sistemi kurucaz
        }
    }
    void Start() 
    {
        
        UpdateGameHudText();
    }
    void Update() 
    {
        if(gameHasEnded)
        {
            LevelEndSequence();

            return;
        }

        if(coinInHand != null && !selectorPrefabsActive)
        {
        //    for (int i = 0; i < selectorPrefabs.Count; i++)
            {
            //    selectorPrefabs[i].SetActive(false);
            }
        }

        allCoinsInScene = GameObject.FindGameObjectsWithTag("Mergeable");

        if(endlessLevel)
        {
            foreach (var coin in allCoinsInScene)
            {
                if(coin.GetComponent<Coin>().value == targetValue)
                {
                    targetIndex++;
                    targetValue = allPossibleTargets[targetIndex];
                    targetValueText.text = targetValue.ToString();
                }    
            }
        }

        else
        {   
            for(int i = 0; i < levelSystemTargets.Count; i++)
            {
                foreach (var coin in allCoinsInScene)
                {
                    if(coin.GetComponent<Coin>().value == levelSystemTargets[i])
                    {
                        currentWinCounter++;

                        if(currentWinCounter == winCondition)
                        {
                            gameHasEnded = true;
                            break;
                        }
                    }
                }

                currentWinCounter = 0;
            }
                
        }




        // COIN SPAWN SYSTEM
        if(nextCoinInHand == null && !selectorPrefabsActive)
        {
            if(firstSpawns)
            {
                nextCoinInHand = Instantiate(possibleSpawnCoinPrefabs[0],trashVector,Quaternion.identity);
                firstSpawns = false;
                return;
            }

            Debug.Log("nextcoininhand null");

            int maxValue = 0;
            foreach (var coin in allCoinsInScene)
            {
                if(maxValue < coin.GetComponent<Coin>().value)
                {
                    maxValue = coin.GetComponent<Coin>().value;
                }
            }

            if(allPossibleTargets.IndexOf(maxValue) >= 0)
            {   
                Debug.Log("index 0 dan büyükl");
                
                if(allPossibleTargets.IndexOf(maxValue) - 2 > 0)
                {
                   throwingRandomizer = Random.Range(0 , allPossibleTargets.IndexOf(maxValue) - 2);
                }
                else
                {
                    throwingRandomizer = 0;
                }

                nextCoinInHand = Instantiate(possibleSpawnCoinPrefabs[0],trashVector,Quaternion.identity);
            }
        }

        if(coinInHand == null && !selectorPrefabsActive)
        {
            Debug.Log("coininhand null");
            coinInHand = nextCoinInHand;

            nextCoinInHand = null;

            coinInHand.transform.DOMove(coinInHandPos.position,moveAnimationTime);
            DOTween.Complete(coinInHand);

            int maxValue = 0;
            foreach (var coin in allCoinsInScene)
            {
                if(maxValue < coin.GetComponent<Coin>().value)
                {
                    maxValue = coin.GetComponent<Coin>().value;
                }
            }

            if(allPossibleTargets.IndexOf(maxValue) >= 0)
            {
                Debug.Log("index 0 dan kücük");
                int throwingRandomizer = Random.Range(0 , allPossibleTargets.IndexOf(maxValue));

                nextCoinInHand = Instantiate(possibleSpawnCoinPrefabs[throwingRandomizer],trashVector,Quaternion.identity);
                
                nextCoinInHand.transform.DOMove(nextCoinHandPos.position,moveAnimationTime);
                DOTween.Complete(nextCoinInHand);
            }
            
        }

    }

    private void LevelEndSequence()
    {
        playerHud.SetActive(false);
        endHud.SetActive(true);

        if(moveCounter > scoringStage2)
        {
            for (int i = 0; i < 1; i++)
            {
                stars[i].GetComponent<Image>().sprite = starIcon.GetComponent<SpriteRenderer>().sprite;
            }
        }
        else if(moveCounter > scoringStage1)
        {
            for (int i = 0; i < 2 ; i++)
            {
                stars[i].GetComponent<Image>().sprite = starIcon.GetComponent<SpriteRenderer>().sprite;
            }
        }
        else
        {
            for (int i = 0; i < 3 ; i++)
            {
                stars[i].GetComponent<Image>().sprite = starIcon.GetComponent<SpriteRenderer>().sprite;
            }
        }
        

    }

    public void ClickedChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ClickedDeleteSkill()
    {
        Debug.Log("delete skill ");
        if(coinInHand == null)
        {
            return;
        }
        Debug.Log("delete skill ");
        int deleteSkillCounter = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.DELETESKILL,10);
        PlayerPrefs.SetInt(Constants.MARKETPURCHASES.DELETESKILL,deleteSkillCounter -1 );
        deleteSkillText.text = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.DELETESKILL,10).ToString();

        coinInHand.transform.position = trashVector;
        coinInHand.gameObject.SetActive(false);
        coinInHand = null;
    }

    public void ClickedChooseCoin()
    {

        if(coinInHand == null) return;

        int chooseCoinSkill = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.CHOOSECOIN,10);
        PlayerPrefs.SetInt( Constants.MARKETPURCHASES.DELETESKILL,chooseCoinSkill -1 );
        selectorSkillText.text = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.CHOOSECOIN,10).ToString();
        
        for (int i = 0; i < selectorPrefabs.Count; i++)
        {   
            selectorPrefabs[i].SetActive(true);

            selectorPrefabsActive = true;

            if(DragSystem.instance.selectedObject!= null)
            {

                DragSystem.instance.selectedObject.transform.position = trashVector;

                DragSystem.instance.selectedObject.gameObject.SetActive(false);
                
                DragSystem.instance.selectedObject = null;

            }

        }

        coinInHand.transform.position = trashVector;

        coinInHand.gameObject.SetActive(false);

    }

    public void UpdateGameHudText()
    {
        coinText.text = PlayerPrefs.GetInt(Constants.DATA.COIN,0).ToString();

        deleteSkillText.text = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.DELETESKILL,0).ToString();

        selectorSkillText.text = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.CHOOSECOIN,0).ToString();

    }
}
