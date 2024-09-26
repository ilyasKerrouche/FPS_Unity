using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowDamage : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifeTime = 1f;

    private TMP_Text damageText;

    void Start()
    {
        damageText = GetComponent<TMP_Text>();
        Destroy(gameObject, lifeTime); // Distruggi l'oggetto dopo il tempo di vita
    }

    void Update()
    {
        // Muovi il testo verso l'alto
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }

    public void SetDamage(float damage)
    {
        damageText.text = damage.ToString(); // Imposta il testo del danno
    }
}
