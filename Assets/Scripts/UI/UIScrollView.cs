using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIScrollView : MonoBehaviour {
    private UIManager uiManager;
    private YleConnector yleConnector;
    private RectTransform thisRectTransform;

    private float spacing = 25f;

    private int numberOfElementUntilEnd = -1;
    private int numberOfElementPerPage;

    private float elementHeight = 200f;
    private float elementHeightExpanded = 500f;

    private RectTransform contentPanel;
    
    [SerializeField]
    private List<BaseUIElement> baseUIElementsList;
    //index of all the element that is currently expanding.
    private List<int> expandedElementList;

    private Sprite defaultThumbnail;
    private Sprite loadingThumbnail;

    public void Init()
    {
        yleConnector = Director.instance.YleConnector;
        uiManager = Director.instance.UiManager;

        thisRectTransform = GetComponent<RectTransform>();
        baseUIElementsList = new List<BaseUIElement>();
        expandedElementList = new List<int>();

        contentPanel = transform.Find("Viewport/Content").GetComponent<RectTransform>();
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, spacing);

        numberOfElementPerPage = (int)(thisRectTransform.sizeDelta.y / elementHeight);

        defaultThumbnail = Resources.Load<Sprite>("YLE_Default_Thumbnail");
        loadingThumbnail = Resources.Load<Sprite>("YLE_Loading_Thumbnail");
    }

    private void Update()
    {
        CalculateLowestElementIndex();
        if (numberOfElementUntilEnd > 0 && numberOfElementUntilEnd <= 3 && !yleConnector.isLoading)
        {
            yleConnector.GetNextResultInQueue();
        }
    }

    public void ClearAllView()
    {
        for (int i = 0; i < baseUIElementsList.Count; i ++)
        {
            baseUIElementsList[i].HideUIElement();
        }
        baseUIElementsList.Clear();
        expandedElementList.Clear();
        contentPanel.anchoredPosition = Vector2.zero;
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, spacing);
    }

    public BaseUIElement AddUIElement(YLEResponse.Data _data)
    {
        BaseUIElement uiElement = PrefabsManager.SpawnPrefab<BaseUIElement>("BaseUIElement", "Prefabs/");
        float elementYPos = spacing + baseUIElementsList.Count * (spacing + elementHeight) + expandedElementList.Count * (elementHeightExpanded - elementHeight);
        uiElement.Init(this, contentPanel, baseUIElementsList.Count, -elementYPos, _data);
        //add to my list for easy manager
        baseUIElementsList.Add(uiElement);
        //resize the content panel accordingly
        contentPanel.sizeDelta += new Vector2(0, spacing + elementHeight);
        //set the thumbnail
        if (_data.HasThumbnail())
        {
            //set the loading thumbnail so I can load them on later.
            uiElement.SetThumnail(loadingThumbnail, 256f, 256f);
        } else
        {
            uiElement.SetThumnail(defaultThumbnail, 256f, 256f);
        }
        return uiElement;
    }

    public void ElementExpand(int _index)
    {
        if (expandedElementList.Contains(_index))
        {
            return;
        }
        expandedElementList.Add(_index);
        contentPanel.DOSizeDelta(new Vector2(contentPanel.sizeDelta.x, contentPanel.sizeDelta.y + elementHeightExpanded - elementHeight), 0.3f);
        for (int i = _index + 1; i < baseUIElementsList.Count; i ++)
        {
            baseUIElementsList[i].MoveBy(elementHeightExpanded - elementHeight, 0.3f);
        }
    }

    public void SetElementThumbnail(int _elementIndex, Texture2D _tex)
    {
        Sprite thumbnail = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), Vector2.zero);
        baseUIElementsList[_elementIndex].SetThumnail(thumbnail, _tex.width, _tex.height);
    }

    private void CalculateLowestElementIndex()
    {
        numberOfElementUntilEnd = (int)((contentPanel.sizeDelta.y - contentPanel.anchoredPosition.y) / (elementHeight + spacing)) - numberOfElementPerPage;
    }
}
