using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct YLEResponse {
    //meta data
    public MetaData meta;
    //data

    public Data[] data;
    //notification

    public Notification notification;

    public static YLEResponse FromJSON(string _jsonData)
    {
        return JsonUtility.FromJson<YLEResponse>(_jsonData);
    }

    //http://developer.yle.fi/tutorial-overview/index.html
    [System.Serializable]
    public struct MetaData
    {
        public string offset;
        public string limit;
        public int count;
        public string q;//query
    }

    [System.Serializable]
    public struct Data
    {
        public string id;
        public BiligualText title;
        public BiligualText description;
        public Image image;
        public Creator creator;

        [System.Serializable]
        public struct BiligualText
        {
            public string fi;
            public string sv;
        }

        [System.Serializable]
        public struct Image
        {
            public string id;
            public bool available;
        }

        [System.Serializable]
        public struct Creator
        {
            public string name;
            public string type;
        }
    }

    [System.Serializable]
    public struct Notification
    {

    }
}

