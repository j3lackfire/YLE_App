using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class YleConnector : MonoBehaviour {

    public delegate void VoidDelegate();

    public Image image;

    public BaseUIElement[] baseUIElements;

    [SerializeField]
    public YLEResponse yleResponse;

    private void Awake()
    {
        image = FindObjectOfType<Image>();
        baseUIElements = FindObjectsOfType<BaseUIElement>();
    }

    public void GetAllItem()
    {
        StartCoroutine(TestFunction(OnDataReceived));
    }

    private IEnumerator TestFunction(VoidDelegate callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(
            YLEHelper.GetBaseURL()
            .SetSearchQuery("Moomin")
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
        for (int i = 0; i < baseUIElements.Length; i ++)
        {
            baseUIElements[i].SetResponse(i + 1, yleResponse.data[i], thumbnail);
        }
    }

}
