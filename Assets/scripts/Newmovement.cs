using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class Newmovement : MonoBehaviour
{
    [SerializeField]
    float speed = 7f;

    public float jumpspeed = 7f;

    private Keyboard keyboard;

    PlayerInput playerInput;

    public Animator anim;

    InputAction moveAction;
    InputAction jumpAction;

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

        velocity = Vector3.zero;

    }



    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        groundCheckDistance = (GetComponent<CapsuleCollider>().height / 2) + bufferCheckDistance;


        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector3.up * jumpspeed, ForceMode.Impulse);

            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
        }
       

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

        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //controller.Move(moveDir * speed * Time.deltaTime + (velocity*Time.deltaTime));

            rb.linearVelocity = new Vector3(moveDir.x * speed, rb.linearVelocity.y, moveDir.z * speed);

            anim.SetFloat("Speed", 1);
        }
        else
        {
            anim.SetFloat("Speed", 0);
            
            if (grounded)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }

        }
    
    
    }   
    



}
