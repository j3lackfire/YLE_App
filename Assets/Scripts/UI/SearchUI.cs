using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchUI : MonoBehaviour {
    private YleConnector yleConnector;

    private Button doSearchButton;
    private InputField searchField;

    private void Awake()
    {
        yleConnector = Director.instance.YleConnector;
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
        yleConnector.DoSearchFunction(searchField.text);
    }
}
