using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private SpriteRenderer sprite;

    private const float MIN_GRAVITY = 0.1f;
    private const float MAX_GRAVITY = 1.3f;
    private const float MAX_ALTITUDE = 250.0f;//min alt is assumed to be 0
    private Rigidbody2D characterRigidBody;
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 currentVelocity;
    [SerializeField]
    private float movementSpeed = 3f;
    private bool alreadyJumped = false;
    [SerializeField]
    private float jumpForce = 300f;

    public LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        this.characterRigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.moveHorizontal = Input.GetAxis("Horizontal"); // X-Axis
        this.moveVertical = Input.GetAxis("Vertical"); // Y-Axis
        this.currentVelocity = this.characterRigidBody.velocity;

        updateAnimation();

        if (IsGrounded())
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (!alreadyJumped && Input.GetKeyDown(KeyCode.Space))
        {
            alreadyJumped = true;
            this.characterRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        updateGravity();
        checkVoidFall();
    }

    private void FixedUpdate()
    {
        if (this.moveHorizontal != 0)
        {
            this.characterRigidBody.velocity = new Vector2(this.moveHorizontal * this.movementSpeed, this.currentVelocity.y);
        }
    }

    void updateAnimation(){
        animator.SetFloat("Speed", Mathf.Abs(currentVelocity.x));
        
        if(currentVelocity.x < -0.01){
            sprite.flipX = true;
        }else if(currentVelocity.x>0.01){
            sprite.flipX = false;
        }

        if(alreadyJumped){
            animator.SetBool("IsJumping", true);
        }else{
            animator.SetBool("IsJumping", false);
        }

    }

    void OnCollisionEnter2D()
    {
        if (IsGrounded())
        {
            alreadyJumped = false;
        }
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }

    void updateGravity(){
        this.characterRigidBody.gravityScale = MIN_GRAVITY + MAX_GRAVITY - ((this.transform.position.y/MAX_ALTITUDE)*MAX_GRAVITY);

        if (this.transform.position.y > MAX_ALTITUDE)
        {
            this.characterRigidBody.gravityScale = MIN_GRAVITY;
        }
        //Debug.Log(this.characterRigidBody.gravityScale);
    }

    void checkVoidFall(){
        if(this.transform.position.y < -20){
            this.characterRigidBody.position = new Vector2(0,0);
        }
    }
}