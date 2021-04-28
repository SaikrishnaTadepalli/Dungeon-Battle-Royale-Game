using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float bulletSpeed = 2f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float startDashTime = 1f;
    [SerializeField] private bool canDash = true;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;
    [SerializeField] private float dashRecoil = 0f;
    [SerializeField] private float health = 100f;
    [SerializeField] private Rigidbody2D bulletPrefab;
    [SerializeField] private GameObject shootPoint;

    Vector2 movement;
    Vector2 mousePosition;
    Vector2 dashDirection;

    private float dashTime = 0;

    private void Start()
    {
        rb = this.transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashTime <= 0 && canDash)
        {
            dashTime = startDashTime;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        // movement = Vector3.Normalize(movement);

        mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 lookDirection = mousePosition - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = angle;

        if (dashTime > 0)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            dashTime -= Time.deltaTime;
        }
        else
        {
            dashDirection = (Vector2)Vector3.Normalize(lookDirection);
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            canDash = true;
        }
    }

    public void SetDashSpeed(int newDashSpeed)
    {
        dashSpeed = newDashSpeed;
    }

    private void Shoot() 
    {
        Vector2 moveDirection = (Vector2)Vector3.Normalize(mousePosition - rb.position);
        Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(bulletPrefab, shootPoint.transform.position, transform.rotation);

        bulletClone.velocity = (Vector2)(moveDirection * bulletSpeed);

        Debug.Log("" + moveDirection);
    }
}
