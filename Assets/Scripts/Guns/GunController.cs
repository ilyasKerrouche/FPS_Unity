using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunAnimatorController : MonoBehaviour
{

    private const string IS_SHOOTING = "IsShooting";
    private const string RELOADING = "IsReloading";
    private const string WATCHING = "IsWatching";
    private Animator Animator;

    private bool isShooting = false;

    private WeaponData weaponData;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        weaponData = GetComponent<WeaponData>();
    }
    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Animator.SetTrigger(RELOADING);
            weaponData.Reload();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Animator.SetTrigger(WATCHING);
        }

        //if(Input.GetMouseButton(0))
        //{
        //    isShooting=true;
        //    Animator.SetBool(IS_SHOOTING, isShooting);
        //    weaponData.FireUpdate(Time.deltaTime);
        //}
    }
}
