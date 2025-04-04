using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Entity, IGethit
{

    [SerializeField] private float attackCooldown_1 = 0.5f;
    [SerializeField] private float attackCooldown_2 = 0.5f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float durPushBack = 0.2f, durDash = 0.2f, speedDash = 20;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private Slider HP_Bar;

    private Attack_1 atk_1;
    private Attack_2 atk_2;
    private Vector2 pushBackVelocity;

    private float attackCountdown_1 = 0, attackCountdown_2 = 0, dashCountdown = 0;
    private bool isAttacking_1 = false, isAttacking_2 = false, isHurting = false, isDeath = false, isPushBack = false, isDashing = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        atk_1 = GameObject.FindWithTag("Attack_1").GetComponent<Attack_1>();
        atk_2 = GameObject.FindWithTag("Attack_2").GetComponent<Attack_2>();

        atk_1.EndAttack();
        atk_2.EndAttack();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Flip();
        AttackByKeyboard();
        DashByKeyboard();
        Cooldown();
        UpdateAnimation();
        HP_Bar.value = HP;
    }

    protected override void Move()
    {
        if (isDeath || isHurting || isAttacking_1 || isAttacking_2) //đứng yên
        {
            if (isPushBack)
            {
                rb.linearVelocity = pushBackVelocity;
                return;
            }

            rb.linearVelocity = movement.normalized * 0;
            return;
        }

        if (isDashing)
        {
            rb.linearVelocity = movement.normalized * speedDash;
            return;
        }
        //Lên xuống
        if (Input.GetKey(KeyCode.W))
            yDir = 1;
        else if (Input.GetKey(KeyCode.S))
            yDir = -1;
        else yDir = 0;

        //Trái phải
        if (Input.GetKey(KeyCode.D))
            xDir = 1;
        else if (Input.GetKey(KeyCode.A))
            xDir = -1;
        else xDir = 0;
        movement = new Vector2(xDir, yDir); // Xác định trái phải khi flip dễ hơn
        rb.linearVelocity = movement.normalized * speed;

    }

    protected override void Flip()
    {
        if (movement.x > 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (movement.x < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override void UpdateAnimation()
    {
        //Chết
        if (isDeath)
        {
            ani.CrossFade("Death", 0f);
            return;
        }
        //Nhận damage
        if (isHurting)
        {
            ani.CrossFade("Hurt", 0f);
            return;
        }
        //Tấn công 1
        if (isAttacking_1)
        {
            ani.CrossFade("Attack_1", 0f);
            return;
        }
        //Tấn công 2
        else if (isAttacking_2)
        {
            ani.CrossFade("Attack_2", 0f);
            return;
        }

        //Đứng yên
        if (movement.x == 0 && movement.y == 0)
        {
            ani.CrossFade("Idle", 0f);
            return;
        }

        //Di chuyển
        if (movement.x != 0 || movement.y != 0)
        {
            ani.CrossFade("Run", 0f);
            return;
        }
    }

    public void Cooldown()
    {
        //Đếm ngược thời gian hồi chiêu
        if (attackCountdown_1 > 0)
            attackCountdown_1 -= Time.deltaTime;

        if (attackCountdown_2 > 0)
            attackCountdown_2 -= Time.deltaTime;

        if (dashCountdown > 0)
            dashCountdown -= Time.deltaTime;
    }
    void AttackByKeyboard()
    {
        //Tấn công 1
        if (Input.GetKeyDown(KeyCode.J) && attackCountdown_1 <= 0 && !isAttacking_1 && !isAttacking_2)
        {
            isAttacking_1 = true;
            attackCountdown_1 = attackCooldown_1;
        }

        //Tấn công 2
        else if (Input.GetKeyDown(KeyCode.K) && attackCountdown_2 <= 0 && !isAttacking_1 && !isAttacking_2)
        {
            isAttacking_2 = true;
            attackCountdown_2 = attackCooldown_2;
        }
    }

    //Bật Attack trong animation event
    public void CallAttack_1()
    {
        atk_1.StartAttack();
    }


    public void CallAttack_2()
    {
        atk_2.StartAttack();
    }

    public void EndAttack()
    {
        //Tắt animation tấn công
        isAttacking_1 = false;
        isAttacking_2 = false;

        //Tắt gameobject tấn công
        atk_1.EndAttack();
        atk_2.EndAttack();
    }

    void DashByKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.L) && dashCountdown <= 0 && !isDashing)
        {
            dashCountdown = dashCooldown;
            StartCoroutine("Dashing");
        }
    }

    IEnumerator Dashing()
    {
        isDashing = true;
        GameObject _dashEffect = Instantiate(dashEffect, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(durDash);
        isDashing = false;
        Destroy(_dashEffect);
    }
    public void Gethit(float damage)
    {
        HP -= damage;

        StartCoroutine("Hurting");
        if (HP <= 0)
        {
            StartCoroutine("Death");
        }
    }

    IEnumerator Hurting()
    {
        isHurting = true;
        yield return new WaitForSeconds(0.1f);
        isHurting = false;
    }
    IEnumerator Death()
    {
        isDeath = true;
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    public override void _PushBack(Vector3 pos) //Lấy hướng đẩy lùi và lưu vận tốc đẩy lùi
    {
        Vector2 dir = (this.transform.position - pos).normalized;
        pushBackVelocity = dir * 10;
        isPushBack = true;
        StartCoroutine("PushBacking");
    }

    IEnumerator PushBacking() // đợi để reset vận tốc
    {
        yield return new WaitForSeconds(durPushBack);
        pushBackVelocity = Vector2.zero;
        isPushBack = false;
    }
}
