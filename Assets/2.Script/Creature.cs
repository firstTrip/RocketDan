using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{

    public float HP;

    public float damage;

    public SpriteRenderer[] sr;

    public Material whiteFlash;

    public Material defaultMaterial;

    public float flashDuration = 0.2f;
    public virtual void GetDamage(float Damage)
    {
    }

    public virtual void OnAttack(float Damage, GameObject creature)
    {

    }

    public virtual IEnumerator Co_WhiteFlash()
    {
        yield return null;
    }
}
