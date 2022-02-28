using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    private PlayerController playerController;
    private InputAction move;
    [SerializeField] GameObject text;

    private Animator animator;
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 10f;
    public bool jump = false;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    private GameObject currentKey;
    private bool key = false, destroy = false, unlocked = false;
    private int keys = 0;

    [SerializeField]
    private Camera playerCamera;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        playerController = new PlayerController();
        text.GetComponent<Text>().text = "Keys: " + keys + "/5";
    }

    private void OnEnable()
    {
        playerController.Player.Jump.started += DoJump;
        playerController.Player.Interact.started += Interact;
        move = playerController.Player.Move;
        playerController.Player.Enable();
    }

    private void OnDisable()
    {
        playerController.Player.Jump.started -= DoJump;
        playerController.Player.Interact.started -= Interact;
        playerController.Player.Disable();
        
    }

    private void Update()
    {
        if (!IsGrounded())
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        animator.SetFloat("speed", rb.velocity.magnitude / maxSpeed);
        animator.SetBool("jump", jump);
    }

    private void FixedUpdate()
    {
        
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if(rb.velocity.y < 0f)
        {
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        }

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        }

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if(move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if(IsGrounded())
        {
            
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (key == true)
        {
            animator.SetTrigger("interact");
            keys += 1;
            text.GetComponent<Text>().text = "Keys: " + keys + "/5";
            SFXScript.instance.audio.PlayOneShot(SFXScript.instance.clip);
            Destroy(currentKey);
        }

        if(unlocked == true)
        {
            print("win");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            currentKey = collision.gameObject;
            key = true;
        }
        if (collision.gameObject.tag == "Door")
        {
            if(keys == 5)
            {
                unlocked = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Key")
        {
            key = false;
        }

        if(collision.gameObject.tag == "Door")
        {
            unlocked = false;
        }
    }
}