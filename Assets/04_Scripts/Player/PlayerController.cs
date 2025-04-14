using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Entity, IGethit, IGetHP
{

    [SerializeField] private float attackCooldown_1 = 0.5f;
    [SerializeField] private float attackCooldown_2 = 0.5f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private float durPushBack = 0.2f, durDash = 0.2f, speedDash = 20;
    [SerializeField] private GameObject dashEffectPrefab, popupTextPrefab;
    [SerializeField] private Slider HP_Bar, Attack1_Bar, Attack2_Bar, _Dash_Bar;
    [SerializeField] private FloatingJoystick joystick;

    private Attack_1 atk_1;
    private Attack_2 atk_2;
    private Vector2 pushBackVelocity;
    private GameObject _dashEffect;

    private float attackCountdown_1 = 0, attackCountdown_2 = 0, dashCountdown = 0, run_Countdown = 0, maxHP;
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

        maxHP = HP;
        HP_Bar.maxValue = HP;
        Attack1_Bar.maxValue = attackCooldown_1;
        Attack2_Bar.maxValue = attackCooldown_2;
        _Dash_Bar.maxValue = dashCooldown;
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
        UI();
    }

    protected override void Move()
    {
        if (isDeath || isHurting || isAttacking_2) //đứng yên
        {
            if (isPushBack)
            {
                rb.linearVelocity = pushBackVelocity;
                return;
            }

            rb.linearVelocity = Vector2.zero;
            return;
        }
        //Dash
        if (isDashing)
        {
            rb.linearVelocity = movement.normalized * speedDash;
            return;
        }
        //Lên xuống

        if (Input.GetKey(KeyCode.W) || joystick.Vertical > 0f)
            yDir = 1;
        else if (Input.GetKey(KeyCode.S) || joystick.Vertical < 0f)
            yDir = -1;
        else yDir = 0;

        //Trái phải
        if (Input.GetKey(KeyCode.D) || joystick.Horizontal > 0f)
            xDir = 1;
        else if (Input.GetKey(KeyCode.A) || joystick.Horizontal < 0f)
            xDir = -1;
        else xDir = 0;


        movement = new Vector2(xDir, yDir); // Xác định trái phải khi flip dễ hơn
        rb.linearVelocity = movement.normalized * speed;
        Run_SFX();
    }

    void Run_SFX()
    {
        if (isDashing || isAttacking_1 || isAttacking_2 || isHurting || isDeath)
            return;
        if (run_Countdown <= 0 && rb.linearVelocity != Vector2.zero)
        {
            AudioManager.ins.PlaySFX(AudioManager.ins.Run_SFXClip);
            run_Countdown = 0.5f;
        }

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
        if (rb.linearVelocity == Vector2.zero)
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

    void UI()
    {
        HP_Bar.value = HP;
        Attack1_Bar.value = Attack1_Bar.maxValue - attackCountdown_1;
        Attack2_Bar.value = Attack2_Bar.maxValue - attackCountdown_2;
        _Dash_Bar.value = _Dash_Bar.maxValue - dashCountdown;
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
        if (run_Countdown > 0)
            run_Countdown -= Time.deltaTime;
    }
    void AttackByKeyboard()
    {
        //Tấn công 1
        if (Input.GetKeyDown(KeyCode.J) && attackCountdown_1 <= 0 && !isAttacking_1 && !isAttacking_2)
        {
            isAttacking_1 = true;
            attackCountdown_1 = attackCooldown_1;
            AudioManager.ins.PlaySFX(AudioManager.ins.Skill1_SFXClip);
        }

        //Tấn công 2
        else if (Input.GetKeyDown(KeyCode.K) && attackCountdown_2 <= 0 && !isAttacking_1 && !isAttacking_2)
        {
            isAttacking_2 = true;
            attackCountdown_2 = attackCooldown_2;
            AudioManager.ins.PlaySFX(AudioManager.ins.Skill2_SFXClip);
        }
    }

    public void Attack1ByButton()
    {
        if (attackCountdown_1 <= 0 && !isAttacking_1 && !isAttacking_2)
        {
            isAttacking_1 = true;
            attackCountdown_1 = attackCooldown_1;
            AudioManager.ins.PlaySFX(AudioManager.ins.Skill1_SFXClip);
        }
    }

    public void Attack2ByButton()
    {
        if (attackCountdown_2 <= 0 && !isAttacking_1 && !isAttacking_2)
        {
            isAttacking_2 = true;
            attackCountdown_2 = attackCooldown_2;
            AudioManager.ins.PlaySFX(AudioManager.ins.Skill2_SFXClip);
        }
    }

    //Bật Attack trong animation event
    public void CallAttack_1()
    {
        StartCoroutine("Dashing", 0.05);
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
            StartCoroutine("Dashing", durDash);
            AudioManager.ins.PlaySFX(AudioManager.ins.Dash_SFXClip);
        }
    }

    public void DashByButton()
    {
        if (dashCountdown <= 0 && !isDashing)
        {
            dashCountdown = dashCooldown;
            StartCoroutine("Dashing", durDash);
            AudioManager.ins.PlaySFX(AudioManager.ins.Dash_SFXClip);
        }
    }

    IEnumerator Dashing(float DashTime)
    {
        isDashing = true;

        if (!isAttacking_1)
        {
            _dashEffect = Instantiate(dashEffectPrefab, this.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(DashTime);
        isDashing = false;
        Destroy(_dashEffect);
    }
    public void GetHP(float HP)
    {
        this.HP += HP;

        GameObject HPText = Instantiate(popupTextPrefab, this.transform.position, Quaternion.identity);
        HPText.GetComponent<PopUpText>().Init("+" + HP.ToString(), Color.green);
        Destroy(HPText, 1f);

        if (this.HP > maxHP)
        {
            this.HP = maxHP;
        }
    }
    public void Gethit(float damage)
    {
        HP -= damage;

        AudioManager.ins.PlaySFX(AudioManager.ins.Gethit_SFXClip);

        GameObject damageText = Instantiate(popupTextPrefab, this.transform.position, Quaternion.identity);
        damageText.GetComponent<PopUpText>().Init("-" + damage.ToString(), Color.red);
        Destroy(damageText, 1f);

        StartCoroutine("Hurting");
        if (HP <= 0)
        {
            StartCoroutine("Death");
            AudioManager.ins.PlaySFX(AudioManager.ins.Death_SFXClip);
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
        this.gameObject.tag = "Untagged";
        this.gameObject.layer = 0;
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        UIManager.ins.UISingle(4, 1);
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
