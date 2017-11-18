using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class YleConnector : BaseManager {

    private UIManager uiManager;

    public delegate void DataReceivedDelegate(YLEResponse _yleResponse);
    public delegate void VoidDelegate();

    private string currentSearchQuery;
    private int currentSearchPosition;
    private int currentRequestingThumbnail = 0;

    [SerializeField]
    public YLEResponse yleResponse;
    [SerializeField]
    public List<YLEResponse.Data> responseDatasList = new List<YLEResponse.Data>();

    public bool isLoading = false;
    private bool isRequestingThumbnail = false;

    public override void Init()
    {
        base.Init();
        uiManager = director.UiManager;
        isLoading = false;
        isRequestingThumbnail = false;
    }

    public void DoSearchFunction(string _query)
    {
        currentSearchPosition = 0;
        currentRequestingThumbnail = 0;
        responseDatasList = new List<YLEResponse.Data>();
        StartCoroutine(SendSearchRequest(_query, 0, OnDataReceived));
    }

    public void GetNextResultInQueue()
    {
        currentSearchPosition += 10;
        StartCoroutine(SendSearchRequest(currentSearchQuery, currentSearchPosition, OnDataReceived));
    }

    private IEnumerator SendSearchRequest(string _query, int _offset, DataReceivedDelegate callback)
    {
        isLoading = true;
        UnityWebRequest www = UnityWebRequest.Get(
            YLEHelper.GetBaseURL()
            .SetSearchQuery(_query)
            .SetSearchLimit(10)
            .SetSearchOffset(_offset)
            .AddAuthorization()
        );

        Debug.Log(YLEHelper.GetBaseURL().SetSearchQuery(_query).SetSearchLimit(10).AddAuthorization());
        yield return www.SendWebRequest();
        isLoading = false;
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Network error !!!");
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("<color=cyan>Server response:</cyan>");
            Debug.Log(www.downloadHandler.text);
            callback(YLEResponse.FromJSON(www.downloadHandler.text));
        }
    }

    private void OnDataReceived(YLEResponse _yleResponse)
    {
        yleResponse = _yleResponse;
        for (int i = 0; i < yleResponse.data.Length; i++)
        {
            uiManager.AddUIElement(yleResponse.data[i]);
            responseDatasList.Add(yleResponse.data[i]);
        }
        if (!isRequestingThumbnail)
        {
            StartCoroutine(RequestThumbnail(currentRequestingThumbnail, OnThumbnailReceived));
        }
    }

    private IEnumerator RequestThumbnail(int _index, VoidDelegate callback)
    {
        isRequestingThumbnail = true;
        if (string.IsNullOrEmpty(responseDatasList[_index].image.id))
        {
            uiManager.SetDefaultThumbnail(_index);
            yield return null;
        } else
        {
            UnityWebRequest www_2 = UnityWebRequest.Get(YLEHelper.GetImage(256, 256, responseDatasList[_index].image.id));
            yield return www_2.SendWebRequest();
            Debug.Log(_index + " ," + Time.timeSinceLevelLoad + " , " + www_2.downloadHandler.text);
            Texture2D thumbnail = new Texture2D(256, 256);
            thumbnail.LoadImage(www_2.downloadHandler.data);
            thumbnail.Apply();
            uiManager.SetThumbnail(_index, thumbnail);
        }
        callback();
    }

    private void OnThumbnailReceived()
    {
        currentRequestingThumbnail++;
        if (currentRequestingThumbnail > responseDatasList.Count - 1)
        {
            isRequestingThumbnail = false;
            return;
        }
        StartCoroutine(RequestThumbnail(currentRequestingThumbnail, OnThumbnailReceived));
    }

}
