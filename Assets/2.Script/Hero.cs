using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Hero : Creature
{

    [SerializeField]
    float attackRange;

    [SerializeField]
    float attackSpeed;

    [SerializeField]
    float rotationSpeed;

    [SerializeField]
    SpriteRenderer gun;

    [SerializeField]
    float NowHp;

    [SerializeField]
    Slider slider;

    LayerMask enemyLayer;
    private void Start()
    {
        enemyLayer = LayerMask.GetMask("ZombiePos_1") | LayerMask.GetMask("ZombiePos_2") | LayerMask.GetMask("ZombiePos_3");
        StartCoroutine(CO_Attack());

        NowHp = HP;
        defaultMaterial = sr[0].material;
    }
    public void Update()
    {
        if (NowHp <= 0)
        {
            Destroy(gameObject);
            GameManger.Instance.SetEnd();
            return;
        }


        slider.value = NowHp / HP;
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

    IEnumerator CO_Attack()
    {
        while (true)
        {
            if(FindNearestEnemy(out Collider2D target))
            {
                OnAttack(damage, target.gameObject);
                LookMonster(target);
                yield return new WaitForSeconds(attackSpeed);

            }
            else
                yield return new WaitForSeconds(0.5f);


        }
    }

    bool IsMonsterInRange(out Collider2D target)
    {
        target = Physics2D.OverlapCircle(transform.position, attackRange, enemyLayer);
        return target != null ? true: false;
    }

    bool FindNearestEnemy(out Collider2D target)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        Collider2D nearest = null;
        float minDistance = attackRange;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        target = nearest;

        return target != null ? true : false;
    }

    void LookMonster(Collider2D target)
    {
        float timeElapsed = 0f;
        while (timeElapsed < attackSpeed)
        {
            Vector2 direction = (target.gameObject.transform.position - gun.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            gun.transform.rotation = Quaternion.Lerp(gun.transform.rotation, Quaternion.Euler(0, 0, angle), timeElapsed / attackSpeed);
            timeElapsed += Time.deltaTime;
        }

    }

    void OnDrawGizmosSelected()
    {
        // 공격 범위를 시각적으로 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
