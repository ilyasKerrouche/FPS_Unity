using System;
using Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    //singleton pattern
    public static Player Instance { get; private set; }
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float verticalSpeed = 30f;
    [SerializeField] private float horizontalSpeed = 30f;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    

    //[SerializeField] private targetObjectCamera targetObjectCamera;


    #region VARIABILI GLOBALI
    private WeaponData weaponData;
    private bool isWalking;
    private bool hitted;
    public Rigidbody playerRigidbody;
    private Transform cameraTransform;

    private float mouseX;
    private float mouseY;


    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("there is more than one player instance");
        }
        Instance = this;


    }
    private void Start()
    {

        playerRigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        weaponData = GetComponentInChildren<WeaponData>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void LateUpdate()
    {
        
    }

    public bool IsWalking()
    {
        return isWalking;
    }


    #region FUNZIONE MOVIMENTO PLAYER    
    private void HandleMovement()
    {


        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        //la moveDir 'e data dalla combinazione di tasti + mouse(direzione mouse ottenuta dalla cam)
        moveDir = cameraTransform.forward * moveDir.z + cameraTransform.right * moveDir.x;
        moveDir.y = 0f;
        // Calcola la distanza di movimento in base alla velocità del giocatore
        float moveDistance = moveSpeed * Time.deltaTime;

        // Sposta il personaggio utilizzando il metodo MovePosition del Rigidbody
        Vector3 targetPosition = playerRigidbody.position + moveDir * moveDistance;

        playerRigidbody.MovePosition(targetPosition);

        // Verifica se il personaggio si sta muovendo
        isWalking = moveDir != Vector3.zero;




        //[CODICE DI SWITCH CAMERA]
        //if (targetObjectCamera != null)
        //{
        //    targetObjectCamera.TargetObject(cameraForward);

        //}
        //else
        //{
        //    Debug.LogError("targetObjectCamera non è assegnato.");
        //}


    }
    #endregion

    private void HandleRotation()
    {
        Vector2 deltaInput = gameInput.GetMouseVectorNormalized();

        Vector3 recoilRotation = weaponData.CameraRecoilApllied();

        // Calcola la rotazione basata sull'input del mouse
        mouseX += deltaInput.x * horizontalSpeed * Time.deltaTime;
        mouseY += deltaInput.y * verticalSpeed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        // mi salvo la rotazione del mouse
        Quaternion cameraRotation = Quaternion.Euler(-mouseY, mouseX, 0f);

        // combino la rotazione del mouse con la rotazione del recoil
        cameraRotation *= Quaternion.Euler(recoilRotation);

        playerRigidbody.MoveRotation(cameraRotation);




    }
}