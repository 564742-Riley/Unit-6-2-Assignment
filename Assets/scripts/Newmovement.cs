using UnityEngine;
using UnityEngine.InputSystem;


public class Newmovement : MonoBehaviour
{
    

    float speed;
    private float walkSpeed = 7f;

    float sprintSpeed = 12f;

    public float jumpspeed = 7f;

    private Keyboard keyboard;

    PlayerInput playerInput;

    public Animator anim;

    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;

    public CharacterController controller;
    public Transform cam;

    public Vector3 velocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    float xvel, yvel;

    Rigidbody rb;
    private bool grounded = false;
    public float groundCheckDistance;
    private float bufferCheckDistance = 0.1f;
    public LayerMask groundLayer;
    bool isJumping;
   


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;
        anim = GetComponent<Animator>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Sprint");

        velocity = Vector3.zero;
        isJumping = false;

    }



    // Update is called once per frame
    void Update()
    {

        print("grounded=" + grounded);

        MovePlayer();
        PlayerSprint();
        PlayerJump();

        groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;

        //check for landing
        if (isJumping && grounded && rb.linearVelocity.y < 0 )
        {
            anim.SetBool("isJumping", false);
            isJumping=false;
            return;
        }


        /*if (Input.GetKeyDown(KeyCode.Space) && grounded && (isJumping==false) )
        {
            rb.AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
            isJumping=true;
            return;
        }*/


       

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, groundCheckDistance))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        

    }


    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();

        // If NO movement input → instantly stop all horizontal momentum
        if (direction.magnitude < 0.1f)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            anim.SetFloat("Speed", 0);
            return;
        }

        float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);

        anim.SetFloat("Speed", 1);
    }

    void PlayerSprint()
    {
        if(sprintAction.IsPressed() && moveAction.ReadValue<Vector2>().magnitude > 0.1f)
        {
            speed = sprintSpeed;
            anim.SetFloat("Speed", 2);

        }
        else
        {
            speed = walkSpeed;
        }


    }
    void PlayerJump()
    {
        if (jumpAction.WasPressedThisFrame() && grounded && (isJumping == false))
        {
            rb.AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
            isJumping = true;
            return;
        }
    }

}
