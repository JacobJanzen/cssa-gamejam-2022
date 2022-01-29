using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D characterRigidBody;
    private float moveHorizontal;
    private float moveVertical;
    private Vector2 currentVelocity;
    [SerializeField]
    private float movementSpeed = 3f;
    private bool isJumping = false;
    private bool alreadyJumped = false;
    [SerializeField]
    private float jumpForce = 300f;
    // Start is called before the first frame update
    void Start()
    {
        this.characterRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.moveHorizontal = Input.GetAxis("Horizontal"); // X-Axis
        this.moveVertical = Input.GetAxis("Vertical"); // Y-Axis
        this.currentVelocity = this.characterRigidBody.velocity;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJumping)
            {
                isJumping = true;
                alreadyJumped = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (this.moveHorizontal != 0)
        {
            this.characterRigidBody.velocity = new Vector2(this.moveHorizontal * this.movementSpeed, this.currentVelocity.y);
        }

        if (isJumping && !alreadyJumped)
        {
            this.characterRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            this.alreadyJumped = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            this.isJumping = false;
    }
}
