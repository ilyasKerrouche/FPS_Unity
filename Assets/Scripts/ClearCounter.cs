using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private GameObject visualGameObject;

    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider collider)
    {
        // Controlla se il giocatore ha il componente Player
        if (collider.transform.TryGetComponent(out Player player))
        {
            // Imposta isPlayerInRange a true quando il player entra nel trigger
            isPlayerInRange = true;
            visualGameObject.SetActive(true);


        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Controlla se il giocatore ha il componente Player
        if (collider.transform.TryGetComponent(out Player player))
        {
            // Imposta isPlayerInRange a false quando il player esce dal trigger
            isPlayerInRange = false;
            visualGameObject.SetActive(false);

        }
    }

    private void Update()
    {
        // Controlla se il player è nel raggio d'azione e se viene premuto il tasto 'E'
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("INTERACT");
            // Qui puoi aggiungere la logica dell'interazione
        }
    }
}
