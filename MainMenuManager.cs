using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] TMP_Text coinText;

    [Header ("Button Movement")]

    [SerializeField] RectTransform[] leftRightButtons;

    [SerializeField] float leftRightMovement;

    [SerializeField] float animTime;

    [Header("Skill Market")]

    [SerializeField] TMP_Text deleteSkill;

    [SerializeField] TMP_Text chooseCoin;

    private void Start() 
    {
       // UpdateMarketMenu();

        // diğer oyundaki değerlere bakıp kopyala yapıştır
        foreach (var button in leftRightButtons)
        {
            button.DOAnchorPos(new Vector3(button.anchoredPosition.x + leftRightMovement,button.anchoredPosition.y),animTime)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }    

    }

    // bu fonksiyon aynı zamanda satın alım yapıldığında da devreye girecek
    // onlar için de iki tane clicked yazcaz setint falan sonun da bunu ekle
    public void UpdateMarketMenu()
    {
        coinText.text = PlayerPrefs.GetInt(Constants.DATA.COIN,0).ToString();

        chooseCoin.text = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.CHOOSECOIN,0).ToString();

        deleteSkill.text = PlayerPrefs.GetInt(Constants.MARKETPURCHASES.DELETESKILL,0).ToString();

    }
    public void ClickedPlayLevels()
    {
        DOTween.Clear();

        // buttondan ilgili seviyenin buildindexini alıp oraya gitmmesini sağlicaz

        //  SceneManager.LoadScene(PlayerPrefs.GetInt(Constants.DATA.CURRENT_LEVEL,2));
    }

    public void ClickedPlayEndless()
    {
        DOTween.Clear();

        SceneManager.LoadScene(PlayerPrefs.GetInt(Constants.DATA.ENDLESS_SCENE,1));
    }


    public void ClickedPlayLevel1()
    {
        DOTween.Clear();

        SceneManager.LoadScene(2);
    }

    public void ClickedPlayLevel2()
    {
        DOTween.Clear();

        SceneManager.LoadScene(3);
    }

    public void ClickedPlayLevel3()
    {
        DOTween.Clear();

        SceneManager.LoadScene(4);
    }

    public void ClickedPlayLevel4()
    {
        DOTween.Clear();

        SceneManager.LoadScene(5);
    }

    public void ClickedPlayLevel5()
    {
        DOTween.Clear();

        SceneManager.LoadScene(6);
    }

}
