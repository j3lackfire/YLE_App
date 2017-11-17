using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIElement : PooledObject {

    private RectTransform myRectTransform;
    //UI and display field
    private Image thumbnailImage;
    private Text title;
    private Text indexNumber;

    [SerializeField]
    private YLEResponse.Data displayData;

    public override void OnFirstInit()
    {
        base.OnFirstInit();
        myRectTransform = GetComponent<RectTransform>();
        thumbnailImage = transform.Find("Thumbnail").GetComponent<Image>();
        title = transform.Find("Title").GetComponent<Text>();
        indexNumber = transform.Find("Index").GetComponent<Text>();
    }

    public void SetParentTransform(RectTransform _rectTransform)
    {
        myRectTransform.SetParent(_rectTransform);
        myRectTransform.anchoredPosition = new Vector3(0f,0f,0f);
    }

    public void SetResponse(int _index, YLEResponse.Data _displayData, Texture2D _thumbnail)
    {
        displayData = _displayData;
        title.text = _displayData.title.fi != "" ? _displayData.title.fi : "[Routsissa] " + _displayData.title.sv;
        indexNumber.text = _index.ToString();
        thumbnailImage.transform.localScale = new Vector3((float)_thumbnail.width/ _thumbnail.height, 1f, 1f);
        thumbnailImage.sprite = Sprite.Create(
            _thumbnail,
            new Rect(0, 0, _thumbnail.width, _thumbnail.height),
            Vector2.zero);
    }
}
