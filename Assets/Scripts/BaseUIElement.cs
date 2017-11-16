using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIElement : MonoBehaviour {

    //UI and display field
    [SerializeField]
    private Image thumbnailImage;
    private Text title;
    private Text indexNumber;

    private YLEResponse.Data displayData;

    private void Awake()
    {
        thumbnailImage = transform.Find("Thumbnail").GetComponent<Image>();
        title = transform.Find("Title").GetComponent<Text>();
        indexNumber = transform.Find("Index").GetComponent<Text>();
    }

    public void SetResponse(int _index, YLEResponse.Data _displayData, Texture2D _thumbnail)
    {
        title.text = _displayData.title.fi;
        indexNumber.text = _index.ToString();
        thumbnailImage.transform.localScale = new Vector3((float)_thumbnail.width/ _thumbnail.height, 1f, 1f);
        thumbnailImage.sprite = Sprite.Create(
            _thumbnail,
            new Rect(0, 0, _thumbnail.width, _thumbnail.height),
            Vector2.zero);
    }
}
