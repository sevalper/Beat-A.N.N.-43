using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float speed;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime * Input.GetAxis("Vertical"));
        transform.Rotate(Vector3.up * turnSpeed * Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Vertical") == 0)
            anim.SetBool("Running", false);
        else
            anim.SetBool("Running", true);
    }

    private bool InRange(float value, float max, float min) { return value < max && value > min; }
}