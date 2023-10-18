using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float gravity = -9.81f;
    public float speed = 10.0f;
    public Transform transformCamera;
    public Transform pivot;
    public GameObject bullet;
    public Transform[] Guns;

    private float ySpeed;
    private bool isJump;
    private bool shot = false;
    private bool isSwitchingGun = false;

    private int _currentGun = 0;
    public int newGun = -1;

    void Start() { }

    void Update()
    {
        var controller = GetComponent<CharacterController>();
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        float rotation = Input.GetAxis("Mouse X");


        Vector3 inputDirection = new Vector3(horizontal, 0.0f, vertical);
        Vector3 movement = new Vector3(horizontal * speed, gravity, vertical * speed);
        
        var inputAngle = horizontal < 0.0f ? -Vector3.Angle(Vector3.forward, inputDirection) : Vector3.Angle(Vector3.forward, inputDirection);
        var animator = GetComponent<Animator>();
        animator.SetFloat("direction", inputAngle / 180.0f);
        animator.SetFloat("idle", inputDirection.magnitude);
        movement = Quaternion.AngleAxis(transformCamera.rotation.eulerAngles.y, Vector3.up) * movement;


        if (newGun != -1 && _currentGun != newGun && !isSwitchingGun )
        {
            StartCoroutine(SwitchGun(animator, newGun));
        }

        controller.Move(movement * Time.deltaTime);
        controller.transform.Rotate(Vector3.up, rotation);
        if(Input.GetMouseButtonDown(0) && shot == false && !isSwitchingGun)
        {
            StartCoroutine(StartShotAnimation(animator));
        }
    }

    IEnumerator StartShotAnimation(Animator animator)
    {
        shot = true;
        animator.SetTrigger("shot");
        yield return new WaitForSeconds(0.1f);
        SpawnProjectile();
        yield return new WaitForSeconds(0.75f);
        shot = false;
    }

    IEnumerator SwitchGun(Animator animator, int gun)
    {
        isSwitchingGun = true;
        animator.SetTrigger("SwitchGun");
        yield return new WaitForSeconds(0.9f);
        Guns[_currentGun].gameObject.SetActive(false);
        _currentGun = newGun;
        Guns[_currentGun].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isSwitchingGun = false;
    }

    void SpawnProjectile()
    {
        var bulletInstance = Instantiate(bullet, pivot.position, pivot.rotation);
        //Debug.Break();
        bulletInstance.SetActive(true);
        var gaun = Guns[_currentGun].GetComponent<Gun>();
        bulletInstance.transform.Rotate(Vector3.left, gaun.Angle);
        bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * gaun.Force);

    }

    private void FixedUpdate() { }

    private void LateUpdate() { }
}
