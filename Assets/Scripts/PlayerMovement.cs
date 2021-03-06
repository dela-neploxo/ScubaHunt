﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    #region Variables

    // Movement variables
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;
    public Vector3 velocity;
    public float sprintModifier;
    bool isSprinting;

    //FOV
    public Transform weaponParent;
    public Camera normalCam;
    float baseFOV;
    float sprintFOVModifier = 1.25f;
    Vector3 weaponOrigin;

    //weaponBob
    private Vector3 weaponParentCurrentPos;
    private Vector3 targetWeaponBobPosition;
    private float movementCounter;
    private float idleCounter;
    #endregion
    void Start() {
        baseFOV = normalCam.fieldOfView;
        weaponOrigin = weaponParent.transform.localPosition;
        weaponParentCurrentPos = weaponOrigin;
    }
    void Update() {
        //Axles
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");


        //Controls
        bool sprint = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetButtonDown("Jump");
        bool crouch = Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);


        //States
        isSprinting = sprint && velocity.z > 0;
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        bool isJumping = jump && isGrounded;
        bool isCrouching = crouch && !isSprinting && !isJumping && isGrounded;


        //Movement
        if (isGrounded && velocity.y < 0) {
            velocity.y = -4f;
        }

        float adjustedSpeed = speed;
        if (isSprinting) {
            adjustedSpeed *= sprintModifier;
        }
        Vector3 move = (transform.right * velocity.x + transform.forward * velocity.z);
        if (move.magnitude > 1) {
            move.Normalize();
        }
        controller.Move(move * adjustedSpeed * Time.deltaTime);

        if (isJumping) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        

        //FOV
        if (isSprinting) {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV * sprintFOVModifier, Time.deltaTime * 8f);
        } else {
            normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView, baseFOV, Time.deltaTime * 8f);
        }
        //Head Bob
        if (!isGrounded)
        {
            //airborne
            HeadBob(idleCounter, 0.015f, 0.015f);
            idleCounter += 0;
            weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime  * 2f* 0.2f);
        }
        else if (velocity.x == 0 && velocity.z == 0)
        {
            //idling
            HeadBob(idleCounter, 0.015f, 0.015f);
            idleCounter += Time.deltaTime;
            weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime  * 1f* 0.2f);
        }
        else if (!isSprinting && (velocity.x != 0 || velocity.z != 0))
        {
            //walking
            HeadBob(movementCounter, 0.015f, 0.015f);
            movementCounter += Time.deltaTime * 5f;
            weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 12f * 0.2f);
        }
        else
        {
            //sprinting
            HeadBob(movementCounter, 0.025f, 0.025f);
            movementCounter += Time.deltaTime * 6.75f;
            targetWeaponBobPosition.z -= 0.5f;
            weaponParent.localPosition = Vector3.MoveTowards(weaponParent.localPosition, targetWeaponBobPosition, Time.deltaTime * 12f * 0.2f);
        }
    }
    #region Private methods
    void HeadBob(float p_z, float p_x_intensity, float p_y_intensity)
    {
        float t_aim_adjust = 1f;
        targetWeaponBobPosition = weaponParentCurrentPos + new Vector3(Mathf.Cos(p_z) * p_x_intensity * t_aim_adjust, Mathf.Sin(p_z * 2) * p_y_intensity * t_aim_adjust, 0);
    }
    #endregion
}
