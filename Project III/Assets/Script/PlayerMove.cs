using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        //body.velocity mô tả tốc độ của ví trí chứa rigidbody
        //Input.GetAxis("Horizontal") thay đổi giá trị với khoảng -1 và 1
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        //khi di chuyen thi nhan vat se quay theo huong minh di chuyen
        anim.SetBool("Run", horizontalInput != 0);//khi chạy thì animtion chạy
        anim.SetBool("Grounded", isGrounded());

        if (wallJumpCooldown < 0.2f)//khi nhay len tuong
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y/*ko thay đổi vị trí của y*/);
            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else body.gravityScale = 7;
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else wallJumpCooldown += Time.deltaTime; 
    }
    private void Jump()//hoat dong nhay
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);//nhan vat di theo truc y
            anim.SetTrigger("Jump");
        }else if (onWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            }
            wallJumpCooldown = 0;
        }
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
