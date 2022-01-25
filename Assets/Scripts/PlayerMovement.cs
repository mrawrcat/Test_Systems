using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Rigidbody2D rb2d;
    private float moveInput;
    private bool faceR = true;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        Facing();
        //rb2d.velocity = new Vector2(moveInput *= speed, rb2d.velocity.y);
        transform.position += transform.right * moveInput * speed * Time.deltaTime;
    }

    private void Flip()
    {
        faceR = !faceR;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void Facing()
    {
        if (faceR && moveInput < 0)
        {
            Flip();
        }
        else if (!faceR && moveInput > 0)
        {
            Flip();
        }
    }
}
