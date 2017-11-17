using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScrollView : MonoBehaviour {
    [SerializeField]
    private RectTransform contentPanel;

    public void Init()
    {
        contentPanel = transform.Find("Viewport/Content").GetComponent<RectTransform>();

        BaseUIElement uiElement = AddUIElement();
        uiElement.SetParentTransform(contentPanel);
        //uiElement.transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public BaseUIElement AddUIElement()
    {
        return PrefabsManager.SpawnPrefab<BaseUIElement>("BaseUIElement", "Prefabs/");
    }
}
