using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    string LayerName;

    [SerializeField]
    Transform SpawnPosition;

    [SerializeField]
    int maxMonsterCount = 500;
    [SerializeField]
    int nowCnt = 0;

    private void Awake()
    {
        StartCoroutine(CO_SpawnMonster());

        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
    }

    IEnumerator CO_SpawnMonster()
    {

        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (nowCnt > maxMonsterCount)
            {
                yield return new WaitForSeconds(1f);

            }
            else
            {
                int randZombie = UnityEngine.Random.Range(1, 5);

                var obj = PoolingManager.GetCreture("ZombieMelee_"+ randZombie);
                obj.gameObject.layer = LayerMask.NameToLayer(LayerName);
                obj.tag = LayerName;
                obj.transform.position = SpawnPosition.position;
                obj.gameObject.GetComponent<Zombie>().SetZombieName("ZombieMelee_" + randZombie);
                nowCnt++;

                float rand = UnityEngine.Random.Range(0, 2.5f);
                yield return new WaitForSeconds(rand);

            }

        }

    }
}
