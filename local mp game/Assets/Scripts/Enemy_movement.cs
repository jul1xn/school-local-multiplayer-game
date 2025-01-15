using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_movement : MonoBehaviour
{
    public Collider2D colliderLeft;
    public Collider2D colliderRight;
    public float moveSpeed = 2f;

    private bool isMovingRight = false;

    private void Start()
    {
        isMovingRight = false;
    }

    private void Update()
    {
        if (isMovingRight)
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector2.right);
        }
        else
        {
            transform.Translate(moveSpeed * Time.deltaTime * Vector2.left);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == colliderLeft)
        {
            isMovingRight = true;
        }
        else if (other == colliderRight)
        {
            isMovingRight = false;
        }
    }
}