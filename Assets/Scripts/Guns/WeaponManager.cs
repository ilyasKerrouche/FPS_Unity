using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    [Header("Rig References")]
    [SerializeField] private Transform rightHandRef;
    [SerializeField] private Transform leftHandRef;
    [SerializeField] private GameObject rightRig;
    [SerializeField] private GameObject leftRig;



    private void Update()
    {

        //Codice per aggangicare le ossa all'arma
        leftRig.transform.position = leftHandRef.position;
        rightRig.transform.position = rightHandRef.position;

        leftRig.transform.rotation = leftHandRef.rotation;
        rightRig.transform.rotation = rightHandRef.rotation;
    }
}
