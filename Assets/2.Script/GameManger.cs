using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    private static GameManger instance = null;

    [SerializeField]
    GameObject wall;

    [SerializeField]
    BoxCollider2D truckColl;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public static GameManger Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void SetEnd()
    {
        truckColl.enabled = false;
        wall.SetActive(false);
    }
}
