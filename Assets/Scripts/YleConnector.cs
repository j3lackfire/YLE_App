using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class YleConnector : BaseManager {

    private UIManager uiManager;

    public delegate void VoidDelegate();

    public Image image;


    [SerializeField]
    public YLEResponse yleResponse;

    public override void Init()
    {
        base.Init();
        uiManager = director.UiManager;
        image = FindObjectOfType<Image>();
    }

    public void DoSearchFunction(string _query)
    {
        Debug.Log("Do search function !!");
        StartCoroutine(TestFunction(_query, OnDataReceived));
    }

    private IEnumerator TestFunction(string _query, VoidDelegate callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(
            YLEHelper.GetBaseURL()
            .SetSearchQuery(_query)
            .SetSearchLimit(10)
            .AddAuthorization()
        );

        Debug.Log(
            YLEHelper.GetBaseURL()
            .SetSearchQuery(_query)
            .SetSearchLimit(10)
            .AddAuthorization()
        );
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            yleResponse = YLEResponse.FromJSON(www.downloadHandler.text);
            callback();
        }
    }

    private void OnDataReceived()
    {
        StartCoroutine(DataReceivedCoroutine(null));
    }

    private IEnumerator DataReceivedCoroutine(VoidDelegate callback)
    {
        UnityWebRequest www_2 = UnityWebRequest.Get(YLEHelper.GetImage(256,256,yleResponse.data[0].image.id));
        yield return www_2.SendWebRequest();
        
        Texture2D thumbnail = new Texture2D(256, 256);
        thumbnail.LoadImage(www_2.downloadHandler.data);
        thumbnail.Apply();
        
    }

}
