using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.EditorTools;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{

    private static PoolingManager instance = null;

    [SerializeField]
    GameObject[] objPrefabs;

    [SerializeField]
    Dictionary<string, GameObject> poolingObjDic = new Dictionary<string, GameObject>();

    [SerializeField]
    Dictionary<string, Queue<GameObject>> poolingObjQueueDic = new Dictionary<string, Queue<GameObject>>();

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

        PoolingManager.Instance.Initailize(100);

    }

    public static PoolingManager Instance
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

    public void Initailize(int _size)
    {
        for (int i = 0; i < objPrefabs.Length; ++i)
        {
            poolingObjDic.Add(objPrefabs[i].name, objPrefabs[i]);
            poolingObjQueueDic.Add(objPrefabs[i].name, new Queue<GameObject>());

            for (int j = 0; j < _size; ++j)
            {
                poolingObjQueueDic[objPrefabs[i].name].Enqueue(CreateNewObj(objPrefabs[i].name));
            }
        }

    }

    private GameObject CreateNewObj(string objName)
    {
        var NewObj = Instantiate(poolingObjDic[objName], transform, true);
        NewObj.gameObject.SetActive(false);

        return NewObj;
    }

    public static GameObject GetCreture(string objName)
    {
        if (PoolingManager.Instance.poolingObjQueueDic[objName].Count > 0)
        {
            var obj = PoolingManager.Instance.poolingObjQueueDic[objName].Dequeue();
            obj.transform.SetParent(PoolingManager.Instance.transform);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {

            var newObj = PoolingManager.Instance.CreateNewObj(objName);
            newObj.transform.SetParent(PoolingManager.Instance.transform);
            newObj.gameObject.SetActive(true);
            return newObj;

        }

    }


    public static void ReturnObj(string objName, GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(PoolingManager.Instance.transform);
        PoolingManager.Instance.poolingObjQueueDic[objName].Enqueue(obj);
    }
}
