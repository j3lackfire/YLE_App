using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : BaseManager {
    private UIScrollView uiScrollView;

    public override void Init()
    {
        base.Init();
        uiScrollView = GetComponentInChildren<UIScrollView>();
        uiScrollView.Init();
    }
}
