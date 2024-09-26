using Cinemachine;
using UnityEngine;


[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapon System/Weapon")]
public class WeaponScriptable : ScriptableObject
{


    [Header("Shoot Settings")]
    public string weaponName;
    public int ammoCapacity;
    [Tooltip("I colpi sono al minuto")]
    public int fireRate;
    public int damage;
    public bool BulletSpread;
    public int damageMultiplier;
    public float trailDuration;
    public Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [Header("Shoot Effects")]
    public TrailRenderer bulletTrail;
    public ParticleSystem shootingEffect;
    public ParticleSystem bulletHoleEffect;

    [Header("Recoil settings")]
    public int snappiness;
    public int returnSpeed;

    [Header("Hipfire recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    [Header("Gun Recoil Animation")]
    public float gunRecoilX;
    public float gunRecoilY;
    public float gunRecoilZ;
    public float gunReturnSpeed;
    public float gunSnapiness;

    [Header("KickBack")]
    public float kickback;
    public float kickbackReturnSpeed;
    public float kickbackSnappiness;


    [Header("VFX")]
    public AudioClip fireShot;


}
