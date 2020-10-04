using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("references")]
    [SerializeField]
    private GameObject bulletFX = null;
    [SerializeField]
    private GameObject cartrigeFX = null, blinkFX = null, rocketPrefab = null, bulletPrefab = null;
    [SerializeField]
    private Rigidbody rb=null;
    [SerializeField]
    private Camera cam=null;
    [SerializeField]
    private Transform shootFrom=null;
    [SerializeField]
    private Image TimeCooldownImage = null, TeleportCooldownImage = null, RocketCooldownImage = null;

    [Header("settings")]
    [SerializeField]
    private float moveSpeed = 4f;
    [SerializeField]
    private float bulletSpeed = 0f, TimeCooldown = 0f, TeleportCooldown = 0f, RocketCooldown = 0f, fireRate=0f,sprint=0f,walk=0f;
    [SerializeField]
    private const float _pressTimeTollerance = 0.3f;

    Vector3 movement, pointToLook;
    private float fireRateTimer, _pressTime, TimeCooldownTimer, TeleportCooldownTimer, RocketCooldownTimer;

    void Start()
    {
        AudioManager.instance.Play("music");
        fireRateTimer = fireRate;
        moveSpeed = 4f;
    }

    void Update()
    {
        if(ManageUI.GameState == ManageUI.GameStates.Playing)
        {

            //character steering
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.z = Input.GetAxisRaw("Vertical");
            movement.y = 0;
            Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                pointToLook = cameraRay.GetPoint(rayLength);
            }



            if (Input.GetButton("Fire1"))
            {
                if (ManageUI.ammo > 0 && fireRateTimer <= 0)
                {
                    Shoot();
                }

            }


            #region blinkOrSprint

            //if shift is held = sprint
            //if shift is tapped = blink
            if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_pressTime < _pressTimeTollerance)
            {
                _pressTime += Time.deltaTime;
            }
            else
            {
                    moveSpeed = sprint;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
                moveSpeed = walk;
                if (_pressTime < _pressTimeTollerance)
            {
                    if (TeleportCooldownTimer > TeleportCooldown)
                    {
                        Instantiate(blinkFX, transform.position, transform.rotation);
                        gameObject.transform.position = new Vector3(pointToLook.x, transform.position.y, pointToLook.z);
                        Instantiate(blinkFX, transform.position, transform.rotation);
                        //implement block blink outside of map maybye?
                        AudioManager.instance.Play("teleport");
                        TeleportCooldownTimer = 0;
                    }
            }

            _pressTime = 0f;
        }

            if(TeleportCooldown> TeleportCooldownTimer)
            {
                TeleportCooldownTimer += Time.deltaTime;
            }
            TeleportCooldownImage.fillAmount = TeleportCooldownTimer / TeleportCooldown;

            #endregion blinkOrSprint


            #region rocket
            if (Input.GetMouseButtonDown(1))
            {
                    if (RocketCooldownTimer > RocketCooldown)
                    {
                        ShootRocket();
                    }
            }

            if (RocketCooldown > RocketCooldownTimer)
            {
                RocketCooldownTimer += Time.deltaTime;
            }
            RocketCooldownImage.fillAmount = RocketCooldownTimer / RocketCooldown;
            #endregion rocket


            #region timeSlow
            if (Input.GetKey(KeyCode.Space)&& TimeCooldownTimer>0&& Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.3f;
                TimeCooldownTimer -= Time.deltaTime*6;
            }
            else
            {
                Time.timeScale = 1.0f;
            }

            if(TimeCooldownTimer < TimeCooldown&& !Input.GetKey(KeyCode.Space))
            {
                TimeCooldownTimer += Time.deltaTime;
            }
            TimeCooldownImage.fillAmount = TimeCooldownTimer/ TimeCooldown;
            #endregion timeslow

        }
        else //if not in playing zero rb
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (fireRateTimer > 0) fireRateTimer-=Time.deltaTime;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        transform.LookAt(new Vector3(pointToLook.x,transform.position.y,pointToLook.z));//rotate to mouse
    }

    void Shoot()
    {
        fireRateTimer = fireRate;
        ManageUI.ammo--;
        AudioManager.instance.Play("shoot");
        Instantiate(bulletFX, shootFrom.position, shootFrom.rotation);
        Instantiate(cartrigeFX, shootFrom.position, transform.rotation * Quaternion.Euler(0f, 90f, 0f));
        GameObject bullet = Instantiate(bulletPrefab, shootFrom.position, shootFrom.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(shootFrom.forward * bulletSpeed);
    }

    void ShootRocket()
    {
        AudioManager.instance.Play("missileFly");
        GameObject rocket = Instantiate(rocketPrefab, shootFrom.position, shootFrom.rotation);
        Rigidbody rb = rocket.GetComponent<Rigidbody>();
        rb.AddForce(shootFrom.forward * bulletSpeed / 2);
        RocketCooldownTimer = 0;
    }

}
