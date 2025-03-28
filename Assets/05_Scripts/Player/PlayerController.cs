using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Attack_1 attack_1;
    [SerializeField] private Attack_2 attack_2;

    Rigidbody2D rb;
    Animator ani;


    float xDir = 0, attackCountdown = 0, _xDirCurrent;
    bool isGrounded = false, isDoubleJump = false, isAttacking_1 = false, isAttacking_2 = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.7f, groundLayer);
        Move();
        Jump();
        FLip();
        Attack();
        UpdateAnimation();

        rb.linearVelocity = new Vector2(speed * xDir, rb.linearVelocity.y);
    }
    void Move()
    {
        if (Input.GetKey(KeyCode.A))
        {
            xDir = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xDir = 1;
        }
        else
        {
            xDir = 0;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isDoubleJump = true;
        }
        else if (Input.GetKeyDown(KeyCode.W) && isDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isDoubleJump = false;
        }
    }

    void FLip()
    {
        if (xDir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _xDirCurrent = 1;
        }
        else if (xDir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _xDirCurrent = -1;
        }

        attack_2.xDirCurrent = _xDirCurrent;
    }

    void UpdateAnimation() // Cập nhật trạng thái animation
    {
        //Tấn công 1
        if (isAttacking_1)
        {
            ani.CrossFade("Attack 1", 0f);
            return;
        }
        //Tấn công 2
        else if (isAttacking_2)
        {
            ani.CrossFade("Attack 2", 0f);
            return;
        }

        //Nhảy
        if (!isGrounded)
        {
            if (rb.linearVelocity.y > 0)
                ani.CrossFade("Jump", 0f);
            else
                ani.CrossFade("Fall", 0f);
            return;
        }

        //Đứng yên
        if (xDir == 0)
        {
            ani.CrossFade("Idle", 0f);
            return;
        }

        //Chạy
        if (xDir != 0)
        {
            ani.CrossFade("Run", 0f);
            return;
        }
    }

    public void EndAttack()
    {
        //Tắt animation tấn công
        isAttacking_1 = false;
        isAttacking_2 = false;

        //Tắt tấn công
        attack_1.EndAttack();
    }

    void Attack()
    {
        //Tính thời gian tấn công
        if (attackCountdown > 0)
            attackCountdown -= Time.deltaTime;
        //Tấn công 1
        if (Input.GetKeyDown(KeyCode.J) && attackCountdown <= 0)
        {
            isAttacking_1 = true;
            attackCountdown = 0.2f;
        }

        //Tấn công 2
        else if (Input.GetKeyDown(KeyCode.K) && attackCountdown <= 0)
        {
            isAttacking_2 = true;
            attackCountdown = 0.2f;
        }
    }

    public void Attack_1()
    {
        attack_1.StartAttack();
    }

    public void Attack_2()
    {
        attack_2.StartAttack();
    }
}
