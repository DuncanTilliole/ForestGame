using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxScript : MonoBehaviour
{
    public float acceleration = 1f;
    private Animator animator;
    private Rigidbody rigidbody;
    private bool isJumping;

    [SerializeField] private float movementSpeed;

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;


    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Play("Fox_Jump_InAir");
        else if (Input.GetKey(KeyCode.DownArrow))
            Play("Fox_Falling");
        else if (Input.GetKey(KeyCode.RightArrow))
            Play("Fox_Run_Right");
        else if (Input.GetKey(KeyCode.LeftArrow))
            Play("Fox_Run_Left");

        acceleration += 0.01f;
        //transform.Translate(Vector3.forward * acceleration * Time.deltaTime);

        PlayerMovement();
    }

    private void Play (string animation)
    {
        animator.Play(animation);
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;


        charController.SimpleMove(Vector3.ClampMagnitude(forwardMovement + rightMovement, 1.0f) * movementSpeed);

        if ((vertInput != 0 || horizInput != 0) && OnSlope())
            charController.Move((Vector3.down * charController.height / 2 * slopeForce * Time.deltaTime) * acceleration);

        JumpInput();
    }

    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move((Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime) * acceleration);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }
}
