using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity, IGethit
{
    [SerializeField] private float DetectRadius = 10, durMove = 1, attackCooldown = 2f, durPushBack = 0.2f;
    [SerializeField] private LayerMask layerMask, obstacleLayerMask;



    private float tempSpeed, countdown = 0, attackCountdown = 1f;
    private bool isAttacking = false, isHurting = false, isDeath = false, isPushBack = false;
    private GameObject player = null;
    private Vector2 pushBackVelocity;
    private EnemyAttack atk;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        atk = GameObject.FindWithTag("EnemyAttack").GetComponent<EnemyAttack>();
        if (GameObject.FindWithTag("Player"))
        {
            player = GameObject.FindWithTag("Player");
        }

        tempSpeed = speed;
        atk.EndAttack();

    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        Move();
        Flip();
        Cooldown();
        UpdateAnimation();
    }
    protected override void Move()
    {
        if (isDeath || isAttacking || isHurting)
        {
            if (isPushBack)
            {
                rb.linearVelocity = pushBackVelocity;
                return;
            }
            rb.linearVelocity = movement.normalized * 0;
            return;
        }

        if (!DetectPlayer())
        {
            MoveRandom(); //nếu không có player trong vùng sẽ di chuyển ngẫu nhiên

        }
        else if (DectectObstacle()) // nếu có obstacle di chuyển ngẫu nhiên
        {
            MoveRandom();
        }
        else
        {
            movement = player.transform.position - this.transform.position;

            if (Vector2.Distance(player.transform.position, this.transform.position) <= 1) // Nếu tới khoảng cách nhất định enemy sẽ dừng để tán công
            {

                AttackAuto();
                rb.linearVelocity = movement.normalized * 0;
                return;
            }
        }


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
        //Tấn công
        if (isAttacking)
        {
            ani.CrossFade("Attack_1", 0f);
            return;
        }
        //Đứng yên
        if (movement.x == 0 && movement.y == 0 || speed == 0)
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

    bool DetectPlayer()
    {
        if (Physics2D.OverlapCircle(this.transform.position, DetectRadius, layerMask)) //  tạo 1 hình tròn để phát hiện player
        {
            Collider2D hit = Physics2D.OverlapCircle(this.transform.position, DetectRadius, layerMask);
            return true;
        }
        else
        {
            return false;
        }
    }

    bool DectectObstacle()
    {
        Vector2 dir = player.transform.position - this.transform.position;
        Debug.DrawRay(this.transform.position, dir, Color.red);
        //bắn tia ray từ enemy đến player, nếu giữa player và enemy chứa layer obstacle thì trả về true
        if (Physics2D.Raycast(this.transform.position, dir, dir.magnitude, obstacleLayerMask))
        {
            return true;
        }
        else return false;
    }

    void MoveRandom()
    {
        if (countdown > 0)
            countdown -= Time.deltaTime;
        if (countdown <= 0) // tạo hướng đi ngẫu nhiên khi countdown về 0
        {
            SetRandom();
            countdown = durMove;
        }
        else if (Physics2D.Raycast(this.transform.position, movement, movement.magnitude, obstacleLayerMask)) // tạo hướng đi ngẫu nhiên khi phát hiện tường
        {
            SetRandom();
            countdown = durMove;
        }
        movement = new Vector2(xDir, yDir);
    }

    void SetRandom()
    {
        xDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        yDir = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
    }

    public void Cooldown()
    {
        //Đếm ngược thời gian hồi chiêu
        if (attackCountdown > 0)
            attackCountdown -= Time.deltaTime;

    }

    void AttackAuto()
    {
        if (attackCountdown <= 0 && !isAttacking)
        {
            isAttacking = true;
            attackCountdown = attackCooldown;
        }
    }
    //Bật Attack trong animation event
    public void CallAttack()
    {
        atk.StartAttack();
    }

    public void EndAttack()
    {
        //Tắt animation tấn công
        isAttacking = false;

        //reset thời gian đợi rồi đánh
        attackCountdown = attackCooldown;
        //Tắt gameobject tấn công
        atk.EndAttack();
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

