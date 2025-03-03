using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : Creature
{

    [SerializeField]
    float NowHp;

    [SerializeField]
    Slider slider;

    private void Start()
    {
        NowHp = HP;

        defaultMaterial = sr[0].material;
    }

    public void Update()
    {
        if (NowHp <= 0)
            Destroy(gameObject);


        slider.value = NowHp / HP;
    }

    public override IEnumerator Co_WhiteFlash()
    {
        foreach (var data in sr)
        {
            data.material = whiteFlash;
        }
        yield return new WaitForSeconds(flashDuration);

        foreach (var data in sr)
        {
            data.material = defaultMaterial;
        }
    }

    public override void GetDamage(float Damage)
    {
        NowHp -= Damage;
        StartCoroutine(Co_WhiteFlash());
    }

    public override void OnAttack(float Damage, GameObject creature)
    {
        creature.GetComponent<Creature>().GetDamage(Damage);
    }
}
