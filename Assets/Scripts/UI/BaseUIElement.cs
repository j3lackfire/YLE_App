using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[SelectionBase]
public class BaseUIElement : PooledObject {

    private UIScrollView uiScrollView;

    private RectTransform myRectTransform;
    private int myIndex;
    //UI and display field
    private Image thumbnailImage;
    private Text title;
    private Text indexNumber;

    private Text description;
    private Text mediaType;
    private Text creator;
    private Text airTime;
    private Text publicationService;

    [SerializeField]
    private YLEResponse.Data displayData;

    private Button thisButton;
    private Button gotoYLEButton;

    public override void OnFirstInit()
    {
        base.OnFirstInit();
        myIndex = -1;
        myRectTransform = GetComponent<RectTransform>();

        thumbnailImage = transform.Find("Thumbnail").GetComponent<Image>();
        title = transform.Find("Title").GetComponent<Text>();
        indexNumber = transform.Find("Index").GetComponent<Text>();

        description = transform.Find("Description").GetComponent<Text>();
        mediaType = transform.Find("MediaType").GetComponent<Text>();
        creator = transform.Find("Creator").GetComponent<Text>();
        airTime = transform.Find("AirTime").GetComponent<Text>();
        publicationService = transform.Find("PublicationService").GetComponent<Text>();

        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => 
        {
            ElementInteract(false, true);
        });

        gotoYLEButton = transform.Find("GotoYLE_Button").GetComponent<Button>();
        gotoYLEButton.onClick.AddListener(() =>
        {
            Application.OpenURL(YLEHelper.GetYleUrlByID(displayData.id));
        });
    }

    public void Init(UIScrollView _uiScrollView, RectTransform _contentPanel, int _index, float _anchorPosY,YLEResponse.Data _data)
    {
        uiScrollView = _uiScrollView;
        myIndex = _index;
        indexNumber.text = (myIndex + 1).ToString();
        SetParentTransform(_contentPanel, _anchorPosY);
        SetResponseData(_data);
        ElementInteract(true, false);
    }

    public void MoveBy(float _deltaPositionY, float _animationTime)
    {
        myRectTransform.DOAnchorPosY(myRectTransform.anchoredPosition.y - _deltaPositionY, _animationTime);
    }

    private void SetParentTransform(RectTransform _rectTransform, float _anchorPosY)
    {
        myRectTransform.SetParent(_rectTransform);
        myRectTransform.anchoredPosition = new Vector3(0f, _anchorPosY, 0f);
        transform.localScale = Vector3.one;
    }

    private void SetResponseData(YLEResponse.Data _displayData)
    {
        displayData = _displayData;
        SetTextTitle();
        SetDescription();
        SetCreator();
        SetMediaType();
        SetPublicationEvent();
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
                    title.text = "Nimiton";
                }
            }
        }
    }

    private void SetCreator()
    {
        creator.text = displayData.creator.Length > 0 ? displayData.creator[0].name : "";
    }

    private void SetMediaType()
    {
        mediaType.text = displayData.type;
    }

    private void SetDescription()
    {
        if (!string.IsNullOrEmpty(displayData.description.fi))
        {
            description.text = displayData.description.fi;
        } else
        {
            if (!string.IsNullOrEmpty(displayData.description.sv))
            {
                description.text = displayData.description.sv;
            } else
            {
                description.text = "Ei ole kuvausta.";
            }
        }
    }

    private void SetPublicationEvent()
    {
        if (displayData.publicationEvent.Length > 0)
        {
            airTime.text = displayData.publicationEvent[0].startTime;
            publicationService.text = displayData.publicationEvent[0].service.id;
        } else
        {
            airTime.text = "";
            publicationService.text = "";
        }
    }

    public void SetThumnail(Sprite _thumbnail, float _width, float _height)
    {
        thumbnailImage.transform.localScale = new Vector3(_width/_height, 1f, 1f);
        thumbnailImage.sprite = _thumbnail;
    }

    //expland conent, show more stuffs
    //ElementInteract(true) -> to expand and false to minimize
    public void ElementInteract(bool _isMinimize, bool _doAnimation = false)
    {
        thisButton.interactable = _isMinimize;
        description.gameObject.SetActive(!_isMinimize);
        mediaType.gameObject.SetActive(!_isMinimize);
        creator.gameObject.SetActive(!_isMinimize);
        airTime.gameObject.SetActive(!_isMinimize);
        publicationService.gameObject.SetActive(!_isMinimize);
        gotoYLEButton.gameObject.SetActive(!_isMinimize);

        float newSizeDeltaY = _isMinimize ? uiScrollView.ElementHeight : uiScrollView.ElementHeightExpanded;

        if (_doAnimation)
        {
            myRectTransform.DOSizeDelta(new Vector2(myRectTransform.sizeDelta.x, newSizeDeltaY), 0.3f);
        } else
        {
            myRectTransform.sizeDelta = new Vector2(myRectTransform.sizeDelta.x, newSizeDeltaY);
        }
        if (!_isMinimize)
        {
            uiScrollView.ElementExpand(myIndex);
        }
    }

    public void HideUIElement()
    {
        ElementInteract(true, false);
        ReturnToPool();
    }
}
