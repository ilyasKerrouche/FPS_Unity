using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCinteracte : MonoBehaviour, IDamageable
{

    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI healtStatus;
    [SerializeField] GameObject healthTextPrefab;

    private GameObject healthTextInstance;
    private bool isTalking;
    private int healt = 100;
    private const string IS_TALKING = "IsTalking";
    private const string IS_DEATH = "IsDeath";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        healt -= damage;
        Debug.Log(healt);

        if(healt <= 0)
        {
            animator.SetBool(IS_DEATH, true);
        }
            
    }

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if(collider.transform.TryGetComponent(out Player player))
    //    {
    //        isTalking = true;
    //        animator.SetBool(IS_TALKING, isTalking);
    //    }
    //}

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.transform.TryGetComponent(out Player player))
    //    {
    //        isTalking = false;
    //        animator.SetBool(IS_TALKING, isTalking);
    //    }

        
    //}




}
