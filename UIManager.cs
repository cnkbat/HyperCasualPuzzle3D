using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform levelSystemPanel, endlessPanel,marketPanel;

    Vector2 levelSystemPanelStart,endlessPanelStart,marketPanelStart;

    [SerializeField] List<RectTransform> slideArrows;

    [SerializeField] float slideInTime;
     
    void Start()
    {
        levelSystemPanelStart = levelSystemPanel.anchoredPosition;

        endlessPanelStart = endlessPanel.anchoredPosition;

        marketPanelStart = marketPanel.anchoredPosition;

        endlessPanel.anchoredPosition = Vector2.zero;
        
    }
    public void OpenMarket()
    {
        levelSystemPanel.DOAnchorPos(levelSystemPanelStart,slideInTime);
        endlessPanel.DOAnchorPos(endlessPanelStart,slideInTime);
        marketPanel.DOAnchorPos(Vector2.zero,slideInTime).SetDelay(slideInTime/5);

    }

    public void OpenEndlessMenu()
    {
        levelSystemPanel.DOAnchorPos(levelSystemPanelStart,slideInTime);
        marketPanel.DOAnchorPos(marketPanelStart,slideInTime);
        endlessPanel.DOAnchorPos(Vector2.zero,slideInTime).SetDelay(slideInTime/5);
    }

    public void OpenLevelSystem()
    {
        marketPanel.DOAnchorPos(marketPanelStart,slideInTime);
        endlessPanel.DOAnchorPos(endlessPanelStart,slideInTime);
        levelSystemPanel.DOAnchorPos(Vector2.zero,slideInTime).SetDelay(slideInTime/5);
    }

}
