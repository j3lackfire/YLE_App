using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class BaseUIElement : PooledObject {

    private RectTransform myRectTransform;
    private int myIndex;
    //UI and display field
    private Image thumbnailImage;
    private Text title;
    private Text indexNumber;

    [SerializeField]
    private YLEResponse.Data displayData;

    public override void OnFirstInit()
    {
        base.OnFirstInit();
        myIndex = -1;
        myRectTransform = GetComponent<RectTransform>();
        thumbnailImage = transform.Find("Thumbnail").GetComponent<Image>();
        title = transform.Find("Title").GetComponent<Text>();
        indexNumber = transform.Find("Index").GetComponent<Text>();
    }

    public void SetParentTransform(RectTransform _rectTransform, int _index, float _offset)
    {
        myRectTransform.SetParent(_rectTransform);
        float buttonHeight = myRectTransform.sizeDelta.y;
        myRectTransform.anchoredPosition = new Vector3(0f, - _offset - _index * (_offset + buttonHeight), 0f);
        transform.localScale = Vector3.one;
    }

    public void SetIndex(int _index)
    {
        myIndex = _index;
        indexNumber.text = myIndex.ToString();
    }

    public void SetResponseData(YLEResponse.Data _displayData)
    {
        displayData = _displayData;
        SetTextTitle();
    }

    private void SetTextTitle()
    {
        if (!string.IsNullOrEmpty(displayData.title.fi))
        {
            title.text = displayData.title.fi;
        }
        else
        {
            if (!string.IsNullOrEmpty(displayData.title.und))
            {
                title.text = displayData.title.und;
            }
            else
            {
                if (!string.IsNullOrEmpty(displayData.title.sv))
                {
                    title.text = "[Ruotsissa] " + displayData.title.sv;
                }
                else
                {
                    title.text = "Untitled";
                }
            }
        }
    }

    public void SetThumnail(Sprite _thumbnail, float _width, float _height)
    {
        thumbnailImage.transform.localScale = new Vector3(_width/_height, 1f, 1f);
        thumbnailImage.sprite = _thumbnail;
    }

    public void HideUIElement()
    {
        ReturnToPool();
    }
}
