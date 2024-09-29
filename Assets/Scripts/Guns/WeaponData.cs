using System.Collections;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponData : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoMagazine;
    [SerializeField] private TextMeshProUGUI damageUI;

    [Header("Weapon Settings")]
    [SerializeField] private WeaponScriptable equippedWeapon;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float rotationalSway = 5f;
    [SerializeField] private float positionalSway = 0.1f;
    [SerializeField] private float swaySmoothness = 7f;
    
    [Header("Shoot Settings")]
    [SerializeField] private Transform BulletSpawnPt;




    private Animator animator;
    private Transform cameraTransform;
    public AudioSource audioSource;
    private float lockTime;
    private int ammo;

    #region VARIABILI PER TENERE TRACCIA DELLA UI
    //ANCORA DA IMPLEMENTARE
    private float time;
    private float displayTime = 2f;
    #endregion

    //VARIABILI RECOIL CAM E ARMA
    private Vector3 targetRotation;
    private Vector3 rotation;
    private Vector3 gunRecoilDir;
    private Vector3 gunRotation;

    private Vector3 currentRecoil;
    private Vector3 targetRecoil;
    private Vector3 kickbackDir;
    private Vector3 kickbackTarget;
    private Vector3 CurrentKickBack;
    private Vector3 gunPositionDir;


    //variabili per il gun sway
    private float mouseX;
    private float mouseY;
    private Quaternion initialRotation = Quaternion.identity;
    private Vector3 initialPosition = Vector3.zero;

    private bool canShoot;
    private bool isShooting;



    private void Awake()
    {
        //animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
        ammo = equippedWeapon.ammoCapacity;
        kickbackDir = Vector3.zero;
        kickbackTarget = Vector3.zero;

    }

    private void Start()
    {
        damageUI.text = "";
        audioSource = GetComponent<AudioSource>();
        
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }

    private void Update()
    {

        //display del damage per un breve periodo
        if (time > 0)
        {
            time -= Time.deltaTime;

            if(time <= 0)
            {
                damageUI.text = "";
            }
        }

        ammoMagazine.text = ammo.ToString() + "/" + equippedWeapon.ammoCapacity + "\n " + equippedWeapon.weaponName;
        CheckInput();
        if (canShoot)
        {
            GunRecoilApplied();
        }
        else
        {
            
            Sway();
        }

    }



    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //animator.SetTrigger(RELOADING);
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //Animator.SetTrigger(WATCHING);
        }

        if (Input.GetMouseButton(0))
        {
            //isShooting = true;
            //animator.enabled = false;
            //animator.SetBool(IS_SHOOTING, true);
            FireUpdate();
            canShoot = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //animator.SetBool(IS_SHOOTING, false);
            //animator.enabled = true;
            canShoot = false;
        }
    }

    public void FireUpdate()
    {

        if(lockTime < Time.time && ammo > 0)
        {
            Shoot();
        }

        
        
    }

    private void Shoot()
    {

        float intervaltime = 60f / (float)equippedWeapon.fireRate;

        lockTime = Time.time + intervaltime;


        equippedWeapon.shootingEffect.Play();
        Vector3 direction = getSpreadDir();
        GetCameraRecoil();
        GetGunRecoilAnimation();
        GunRecoilApplied();
        PlayFireSound();


        if (Physics.Raycast(cameraTransform.position, direction, out RaycastHit hit))
        {
            //creo il proiettile
            TrailRenderer trail = Instantiate(equippedWeapon.bulletTrail, BulletSpawnPt.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
            //da fixare -> spawn trail sempre, sposta fuori il raycast;
            //             bullehole solo se hitto

            ammo -= 1;

            Debug.Log("DANNO INFLITTO:" + equippedWeapon.damage);

            IDamageable iDamageable = hit.collider.GetComponent<IDamageable>();
            if (iDamageable != null)
            {
                //time = displayTime;
                //damageUI.text = equippedWeapon.damage.ToString();
                //damageUI.color = Color.yellow;
                Debug.Log("OGGETTO ESISTEMTE");
                iDamageable.TakeDamage(equippedWeapon.damage);
            }


        }
    }

    //FUNZIONE PER LO SPREAD -> HIPFIRE MODE
    private Vector3 getSpreadDir()
    {
        
        Vector3 direction = cameraTransform.forward;

        if(equippedWeapon.BulletSpread)
        {
            
            direction += new Vector3(
                Random.Range(-equippedWeapon.bulletSpreadVariance.x, equippedWeapon.bulletSpreadVariance.x),
                Random.Range(-equippedWeapon.bulletSpreadVariance.y, equippedWeapon.bulletSpreadVariance.y),
                Random.Range(-equippedWeapon.bulletSpreadVariance.z, equippedWeapon.bulletSpreadVariance.z)
                );

            direction.Normalize();
        }

        


        return direction;
    }

    //FUNZIONE PER SPAWNARE PROIETTILE
    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit hit)
    {

        Trail.time = equippedWeapon.trailDuration;

        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        //sposto il proiettile dalla posizone attuale fino all'oggetto colpito
        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }

        //animator.SetBool(IS_SHOOTING, false);
        Trail.transform.position = hit.point;
        //creo un bulletHole
        Instantiate(equippedWeapon.bulletHoleEffect, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(Trail.gameObject, Trail.time);
    }


    //FUNZIONE PER CALCOLARE IL RECOIL
    private void GetCameraRecoil()
    {
        

        targetRotation += new Vector3(-equippedWeapon.recoilX,
                         Random.Range(-equippedWeapon.recoilY, equippedWeapon.recoilY),
                         Random.Range(-equippedWeapon.recoilZ, equippedWeapon.recoilZ));

    }

    //FUNZIONE PER AGGIORNARE LA ROTAZIONE DELLA IN BASE AL RECOIL APPLICATO
    public Vector3 CameraRecoilApllied()
    {
        //CAMERA RECOIL
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, equippedWeapon.returnSpeed * Time.deltaTime);
        rotation = Vector3.Slerp(rotation, targetRotation, equippedWeapon.snappiness * Time.deltaTime);

        return rotation;

    }

    private void GetGunRecoilAnimation()
    {
        // Imposta la direzione del rinculo
        targetRecoil += new Vector3(-equippedWeapon.gunRecoilX,  // Recoil verticale (alzare la canna)
                        Random.Range(-equippedWeapon.gunRecoilY, equippedWeapon.gunRecoilY),  // Recoil orizzontale casuale
                        Random.Range(-equippedWeapon.gunRecoilZ, equippedWeapon.gunRecoilZ));



        kickbackDir += new Vector3(0, 0, -equippedWeapon.kickback);
        



    }
    private void GunRecoilApplied()
    {

        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, equippedWeapon.gunReturnSpeed * Time.deltaTime);
        targetRecoil = Vector3.Slerp(targetRecoil, Vector3.zero, equippedWeapon.gunSnapiness * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRecoil);

        currentRecoil = Vector3.Lerp(currentRecoil, targetRecoil, equippedWeapon.gunSnapiness * Time.deltaTime);
        



        //kickback

        // Gestisci il ritorno della kickback
        kickbackDir = Vector3.Slerp(kickbackDir, Vector3.zero, Time.deltaTime * equippedWeapon.kickbackReturnSpeed);
        kickbackTarget = Vector3.Lerp(kickbackTarget, Vector3.zero, Time.deltaTime * equippedWeapon.kickbackReturnSpeed);



        // Sposta l'arma lungo l'asse Z per il kickback
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, kickbackTarget.z);

        kickbackTarget = Vector3.Lerp(kickbackTarget, kickbackDir, Time.deltaTime * equippedWeapon.kickbackSnappiness);


    }


    public void Reload()
    {
        ammo = equippedWeapon.ammoCapacity;
    }


    private void PlayFireSound()
    {
        if(equippedWeapon.fireShot != null && audioSource != null)
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.PlayOneShot(equippedWeapon.fireShot);
    }


    private void Sway()
    {
        if(!canShoot)
        {
            Vector2 input = gameInput.GetMouseVectorNormalized();

            float mouseX = input.x;
            float mouseY = input.y;

            //CALCOLO LA POSIZIONE E ROTAZIONE DA APPLICARE ALL'ARMA
            Vector3 positionOffset = new Vector3(mouseX, mouseY, 0) * positionalSway;
            Quaternion gunRotationOffset = Quaternion.Euler(new Vector3(-mouseY, -mouseX, mouseX) * rotationalSway);
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition - positionOffset, Time.deltaTime * swaySmoothness);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, initialRotation * gunRotationOffset, Time.deltaTime * swaySmoothness);




        }
    }

}

