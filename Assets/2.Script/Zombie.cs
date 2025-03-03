using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Creature
{

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    Animator anim;

    [SerializeField]
    float speed;

    [SerializeField]
    float jumpForce;
    [SerializeField]
    int layerNum;

    [SerializeField]
    string groundName;

    [SerializeField]
    string zombieName;

    [SerializeField]
    float attackCoolTime = 3f;
    private void Start()
    {
        switch(LayerMask.LayerToName(this.gameObject.layer))
        {
            case "ZombiePos_1":
                groundName = "Ground_1";
                break;

            case "ZombiePos_2":
                groundName = "Ground_2";
                break;

            case "ZombiePos_3":
                groundName = "Ground_3";
                break;

        }
        defaultMaterial = sr[0].material;
    }

    private void FixedUpdate()
    {
        if(HP <=0)
        {
            anim.Play("Die");
            return;
        }

        layerNum = this.gameObject.layer;
        RaycastHit2D hitMonster = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(-0.7f, 0.3f, 0), Vector2.left,0.25f, LayerMask.GetMask(LayerMask.LayerToName(layerNum)));
       // RaycastHit2D[] hitMonsterAll = Physics2D.RaycastAll(this.gameObject.transform.position + new Vector3(-0.5f, 0.75f, 0), Vector2.left, 0.5f);
        RaycastHit2D hitCreture = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(-0.5f,0.6f,0), Vector2.left, 1f,(LayerMask.GetMask("Box") | (LayerMask.GetMask("Hero"))));
        RaycastHit2D hitUp = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(-0.5f, 0.6f, 0), Vector2.up, 1f, LayerMask.GetMask(LayerMask.LayerToName(layerNum)));

        RaycastHit2D hitDown = Physics2D.Raycast(this.gameObject.transform.position + new Vector3(-0.5f, 0.6f, 0), Vector2.down, 1f, LayerMask.GetMask(LayerMask.LayerToName(layerNum)) | LayerMask.GetMask(groundName));

        if (hitMonster)
        {
            if(!hitUp)
            {

                if(hitDown)
                     OnJump();

            }
            else
            {
                if (hitDown)
                    OnMove();
            }
        }
        else
        {
            if (hitCreture)
            {
                if(this.gameObject)
                {
                    if(!isAttack)
                    {
                        anim.Play("Attack");
                        OnAttack(damage, hitCreture.collider.gameObject);
                    }
                }
            }
            else
            {
                if(hitDown)
                    OnMove();
            }
        }

       
    }

    void OnMove()
    {
        rb.velocity = Vector3.left * speed;
    }

    void OnJump()
    {
        rb.velocity = new Vector2(-2, jumpForce);
    }

    public override void GetDamage(float Damage)
    {
        HP -= Damage;
        var obj = PoolingManager.GetCreture("DamageText");
        obj.transform.position = this.gameObject.transform.position + new Vector3(0, 1f, 0);
        obj.gameObject.GetComponent<DamageText>().SetDamage(Damage);
        StartCoroutine(Co_WhiteFlash());

    }

    public override void OnAttack(float Damage, GameObject creature)
    {
        StartCoroutine(CO_OnAttack());
        creature.GetComponent<Creature>().GetDamage(Damage);
    }

    public void SetZombieName(string name)
    {
        zombieName = name;
    }

    bool isAttack = false;
    IEnumerator CO_OnAttack()
    {
        isAttack = true;
        yield return new WaitForSeconds(attackCoolTime);
        isAttack = false;

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


    void OnDie()
    {
        PoolingManager.ReturnObj(zombieName, this.gameObject);

    }
}
