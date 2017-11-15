using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class YleConnector : MonoBehaviour {

    private void Awake()
    {

    }
	
	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(TestFunction());
        }
    }

    private IEnumerator TestFunction()
    {
        Debug.Log("<color=cyan> Test function </color>");
        UnityWebRequest www = UnityWebRequest.Get("https://external.api.yle.fi/v1/programs/items.json?" + GlobalValues.GetPersonalKey());
        Debug.Log(Time.timeSinceLevelLoad);
        yield return www.SendWebRequest();
        Debug.Log(Time.timeSinceLevelLoad);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }

    }
}
