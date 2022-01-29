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
    private bool alreadyJumped = false;
    [SerializeField]
    private float jumpForce = 300f;

    public LayerMask groundLayer;
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

        if (!alreadyJumped && Input.GetKeyDown(KeyCode.Space))
        {
            this.characterRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            alreadyJumped = true;
        }
        else if (IsGrounded())
        {
            alreadyJumped = false;
            transform.eulerAngles = new Vector3(0,0,0);
        }
    }

    private void FixedUpdate()
    {
        if (this.moveHorizontal != 0)
        {
            this.characterRigidBody.velocity = new Vector2(this.moveHorizontal * this.movementSpeed, this.currentVelocity.y);
        }
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = .6f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
