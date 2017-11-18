using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager {
    private UIScrollView uiScrollView;
    private SearchUI searchUI;


    public override void Init()
    {
        base.Init();
        uiScrollView = GetComponentInChildren<UIScrollView>();
        uiScrollView.Init();

        searchUI = GetComponentInChildren<SearchUI>();
        searchUI.Init();
    }

    public void ClearSearchResult()
    {
        uiScrollView.ClearAllView();
    }

    public void AddUIElement(YLEResponse.Data _data)
    {
        uiScrollView.AddUIElement(_data);
    }

    public void SetThumbnail(int _index, Texture2D _tex)
    {
        uiScrollView.SetElementThumbnail(_index, _tex);
    }
}
