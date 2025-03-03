using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    TextMeshPro tmp;

    [SerializeField]
    Color alpha;

    [SerializeField]
    float speed;
    [SerializeField]
    float alphaSpeed;

    private void Start()
    {
        Invoke("Return", 2f);
    }

    public void SetDamage(float damage)
    {
        tmp.text = damage.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        tmp.color = alpha;
    }

    void Return()
    {
        PoolingManager.ReturnObj("DamageText",this.gameObject);
    }
}
