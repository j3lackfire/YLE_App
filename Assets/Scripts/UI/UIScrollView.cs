using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollView : MonoBehaviour {
    private UIManager uiManager;
    private YleConnector yleConnector;
    private RectTransform thisRectTransform;

    [SerializeField]
    private float offset = 25f;

    [SerializeField]
    private int bottomIndex = -1;
    private int numberOfElementPerPage;

    private float elementHeight = 200f;
    private float elementHeightExpanded = 500f;

    private RectTransform contentPanel;
    [SerializeField]
    public List<BaseUIElement> baseUIElementsList;

    public void Init()
    {
        yleConnector = Director.instance.YleConnector;
        uiManager = Director.instance.UiManager;

        thisRectTransform = GetComponent<RectTransform>();

        contentPanel = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, offset);

        baseUIElementsList = new List<BaseUIElement>();
        BaseUIElement uiElement = PrefabsManager.SpawnPrefab<BaseUIElement>("BaseUIElement", "Prefabs/");

        numberOfElementPerPage = (int)(thisRectTransform.sizeDelta.y / elementHeight);
    }

    private void Update()
    {
        CheckForCurrentLowestIndexNumber();
        if (bottomIndex > 0 && bottomIndex >= baseUIElementsList.Count - 3 && !yleConnector.isLoading)
        {
            yleConnector.GetNextResultInQueue();
        }
    }

    public void ClearAllView()
    {
        for (int i = 0; i < baseUIElementsList.Count; i ++)
        {
            uiManager.SetDefaultThumbnail(i);
            baseUIElementsList[i].HideUIElement();
        }
        baseUIElementsList.Clear();
        contentPanel.anchoredPosition = Vector2.zero;
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, offset);
    }

    public BaseUIElement AddUIElement()
    {
        BaseUIElement uiElement = PrefabsManager.SpawnPrefab<BaseUIElement>("BaseUIElement", "Prefabs/");
        uiElement.SetParentTransform(contentPanel, baseUIElementsList.Count, offset);
        baseUIElementsList.Add(uiElement);
        uiElement.SetIndex(baseUIElementsList.Count);

        contentPanel.sizeDelta += new Vector2(0, offset + elementHeight);
        return uiElement;
    }

    private void CheckForCurrentLowestIndexNumber()
    {
        if (baseUIElementsList.Count == 0)
        {
            bottomIndex = -1;
        } else
        {
            bottomIndex = (int)(contentPanel.anchoredPosition.y / (elementHeight + offset)) + numberOfElementPerPage;
        }
    }
}
