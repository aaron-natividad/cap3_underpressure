using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataClear : MonoBehaviour
{
    private GameObject dataManager;

    private void OnEnable()
    {
        ContextScreen.OnContextFinish += DestroyManager;
    }

    private void OnDisable()
    {
        ContextScreen.OnContextFinish -= DestroyManager;
    }

    private void Awake()
    {
        dataManager = DataManager.instance.gameObject;
    }

    private void DestroyManager()
    {
        DataManager.instance = null;
        Destroy(dataManager);
    }
}
