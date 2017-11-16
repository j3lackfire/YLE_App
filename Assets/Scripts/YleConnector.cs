using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class YleConnector : MonoBehaviour {

    public delegate void VoidDelegate();

    public Image image;

    [SerializeField]
    public YLEResponse yleResponse;

    private void Awake()
    {
        image = FindObjectOfType<Image>();
    }

    public void GetAllItem()
    {
        StartCoroutine(TestFunction(OnDataReceived));
    }

    private IEnumerator TestFunction(VoidDelegate callback)
    {
        Debug.Log("<color=cyan> Test function at </color>" + Time.timeSinceLevelLoad);

        UnityWebRequest www = UnityWebRequest.Get(
            YLEHelper.GetBaseURL()
            .SetSearchQuery("Vietnam")
            .SetSearchLimit(10)
            .AddAuthorization()
            );

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
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
        Debug.Log(Time.timeSinceLevelLoad);
        StartCoroutine(DataReceivedCoroutine(null));
    }

    private IEnumerator DataReceivedCoroutine(VoidDelegate callback)
    {
        UnityWebRequest www_2 = UnityWebRequest.Get(YLEHelper.GetImage(256,256,yleResponse.data[0].image.id));
        Debug.Log(YLEHelper.GetImage(256, 256, yleResponse.data[0].image.id));
        yield return www_2.SendWebRequest();
        Debug.Log("Thumbnail received !!!");
        Texture2D thumbnail = new Texture2D(256, 256);
        thumbnail.LoadImage(www_2.downloadHandler.data);
        thumbnail.Apply();
        //((DownloadHandlerTexture)www_2.downloadHandler).texture,
        image.sprite = Sprite.Create(
            thumbnail,
            new Rect(0, 0, thumbnail.width, thumbnail.height),
            Vector2.zero);
    }

}
