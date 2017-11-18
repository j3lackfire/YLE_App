using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchUI : MonoBehaviour {
    private YleConnector yleConnector;
    private UIManager uiManager;

    private Button doSearchButton;
    private InputField searchField;

    public void Init()
    {
        yleConnector = Director.instance.YleConnector;
        uiManager = Director.instance.UiManager;
        doSearchButton = transform.Find("SearchButton").GetComponent<Button>();
        searchField = transform.Find("SearchQuery").GetComponent<InputField>();
    }

    private void OnEnable()
    {
        doSearchButton.onClick.AddListener(OnSearchButtonClicked);
    }

    private void OnDisable()
    {
        doSearchButton.onClick.RemoveListener(OnSearchButtonClicked);
    }

    private void OnSearchButtonClicked()
    {
        uiManager.ClearSearchResult();
        yleConnector.DoSearchFunction(searchField.text);
    }
}
