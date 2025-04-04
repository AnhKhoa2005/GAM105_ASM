using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float HP = 100;
    [SerializeField] protected float speed = 5;

    protected float xDir;
    protected float yDir;

    protected Vector2 movement = Vector2.zero;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected Animator ani;
    protected abstract void Move();
    protected abstract void Flip();
    protected abstract void UpdateAnimation();
    public abstract void _PushBack(Vector3 pos);
}
