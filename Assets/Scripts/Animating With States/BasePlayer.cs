using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour, IDamagable<int>
{
    private int health;
    public enum AnimationType
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Die,
    }
    public AnimationType activeAnimType;

    public AnimationType GetActiveAnimType()
    {
        return activeAnimType;
    }

    [SerializeField] private Sprite[] idleAnim;
    [SerializeField] private Sprite[] walkAnim;
    [SerializeField] private Sprite[] atkAnim;
    [SerializeField] private Sprite[] hurtAnim;
    [SerializeField] private Sprite[] dieAnim;
    private SpriteAnimatorCustom anim;
    private bool faceR = true;
    private float moveInputX;
    public float GetMoveInput()
    {
        return moveInputX;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.SetFrameArray(idleAnim);
        activeAnimType = AnimationType.Idle;
        anim.PlayAnimationCustom(idleAnim, .1f);
    }

    // Update is called once per frame
    void Update()
    {
        moveInputX = Input.GetAxisRaw("Horizontal");
        Facing();
    }

    
    public void PlayCharacterAnimation(AnimationType animType)
    {
        if (animType != activeAnimType)
        {
            activeAnimType = animType;
            switch (animType)
            {
                default:
                case AnimationType.Idle:
                    //spriteAnim.SetFrameArray(idleAnim);
                    anim.PlayAnimationCustom(idleAnim, .1f);
                    break;
                case AnimationType.Walk:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(walkAnim, .1f);
                    break;
                case AnimationType.Attack:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(atkAnim, .1f, false);
                    break;
                case AnimationType.Hurt:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(hurtAnim, .1f, false);
                    break;
                case AnimationType.Die:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(dieAnim, .1f, false);
                    break;
            }
        }
    }

    public void TakeDmg(int dmgAmt)
    {
        health -= dmgAmt;
        if (health <= 0)
        {
            //gameObject.SetActive(false);
        }
    }

    private void Facing()
    {
        if (faceR && moveInputX < 0)
        {
            Flip();
        }
        else if (!faceR && moveInputX > 0)
        {
            Flip();
        }
    }
    private void Flip()
    {
        faceR = !faceR;
        /*
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        */
        Vector3 spriteScaler = GetComponentInChildren<SpriteRenderer>().transform.localScale;
        spriteScaler.x *= -1;
        GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScaler;

    }
}
