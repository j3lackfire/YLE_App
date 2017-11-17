using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {

    public YleConnector YleConnector { get; private set; }
    public UIManager UiManager { get; private set; }

    /*
     * Singleton. ONLY use singleton for this.  
     * For everything else, access though THE director 
     */
    private static Director _instance;
    public static Director instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    [HideInInspector]
    private List<BaseManager> managersList;

    public void Awake()
    {
        #region singleton check
        if (instance == null)
        {
            instance = FindObjectOfType<Director>();
        }
        else
        {
            if (instance.gameObject == null)
            {
                instance = FindObjectOfType<Director>();
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
        }
        #endregion

        InitManager();
        PrefabsManager.PreparePrefabsManager();
    }

    public void Update()
    {
        for (int i = 0; i < managersList.Count; i++)
        {
            managersList[i].DoUpdate();
        }
    }

    private void InitManager()
    {
        managersList = new List<BaseManager>();

        //-------------- IMPORTANT STUFF HERE -------------
        //the order of prepare is also the order to init of the manager
        YleConnector = PrepareManager(YleConnector);
        UiManager = PrepareManager(UiManager);

        //-------------- END OF IMPORTANT STUFF -------------

        for (int i = 0; i < managersList.Count; i++)
        {
            managersList[i].Init();
        }
    }

    //super clever prepare manager function I think of xD
    private T PrepareManager<T>(T tManager) where T : BaseManager
    {
        T tempT = (T)FindObjectOfType(typeof(T));
        if (tManager != null)
        {
            Destroy(tManager.gameObject);
        }
        managersList.Add(tempT);
        return tempT;
    }
}
