using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager {
    private UIScrollView uiScrollView;
    private SearchUI searchUI;

    [SerializeField]
    private Sprite defaultThumbnail;

    public override void Init()
    {
        base.Init();
        uiScrollView = GetComponentInChildren<UIScrollView>();
        uiScrollView.Init();

        searchUI = GetComponentInChildren<SearchUI>();
        searchUI.Init();

        defaultThumbnail = Resources.Load<Sprite>("YLE_Default_Logo");
        if (defaultThumbnail == null)
        {
            Debug.Log("Ehh ?");
        }
    }

    public void ClearSearchResult()
    {
        uiScrollView.ClearAllView();
    }

    public void AddUIElement(YLEResponse.Data _data)
    {
        uiScrollView.AddUIElement().SetResponseData(_data);
    }

    public void SetDefaultThumbnail(int _index)
    {
        uiScrollView.baseUIElementsList[_index].SetThumnail(defaultThumbnail, 256f, 256f);
    }

    public void SetThumbnail(int _index, Texture2D _tex)
    {
        Sprite thumbnail = Sprite.Create( _tex, new Rect(0, 0, _tex.width, _tex.height), Vector2.zero);
        uiScrollView.baseUIElementsList[_index].SetThumnail(thumbnail, _tex.width, _tex.height);
    }
}
