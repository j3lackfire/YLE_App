using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SearchUI : MonoBehaviour {
    private YleConnector yleConnector;
    private UIManager uiManager;

    private Button doSearchButton;
    private InputField searchField;

    private Text numberOfResult;
    private Text logo;

    private bool isFirstSearch = true;

    [SerializeField]
    private Vector2 searchFieldPos;
    [SerializeField]
    private Vector2 searchFieldSize;

    [SerializeField]
    private Vector2 searchButtonPos;
    [SerializeField]
    private Vector2 searchButtonSize;

    public void Init()
    {
        yleConnector = Director.instance.YleConnector;
        uiManager = Director.instance.UiManager;
        doSearchButton = transform.Find("SearchButton").GetComponent<Button>();
        searchField = transform.Find("SearchQuery").GetComponent<InputField>();

        numberOfResult = transform.Find("NumberOfResult").GetComponent<Text>();
        numberOfResult.text = "";
        logo = transform.Find("LOGO").GetComponent<Text>();

        isFirstSearch = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
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
        //this is the first search result
        if (isFirstSearch)
        {
            isFirstSearch = false;
            logo.gameObject.SetActive(false);
            RectTransform searchFieldRect = searchField.GetComponent<RectTransform>();
            searchFieldRect.DOAnchorPos(searchFieldPos, 0.3f);
            searchFieldRect.DOSizeDelta(searchFieldSize, 0.3f);

            RectTransform searchButtonRect = doSearchButton.GetComponent<RectTransform>();
            searchButtonRect.DOAnchorPos(searchButtonPos, 0.3f);
            searchButtonRect.DOSizeDelta(searchButtonSize, 0.3f);
            OnSearchButtonClicked();
        } else
        {
            uiManager.ClearSearchResult();
            yleConnector.DoSearchFunction(searchField.text);
        }
    }

    public void SetNumberOfResult(int _numberOfResults)
    {
        numberOfResult.text = "Noin " + _numberOfResults.ToString() + " tulosta";
    }
}
