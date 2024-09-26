using Cinemachine;
using UnityEngine;

public class targetObjectCamera : MonoBehaviour
{
    [Header("Lock System Prop")]
    [SerializeField] private Player player;
    [SerializeField] private Transform lineStartingPoint;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private GameObject fpsCamera;
    [SerializeField] private GameObject lockCamera;

    private bool isFpsCameraActive = true;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && !isFpsCameraActive)
        {
            setFpsCameraActive();
        }
    }
    //Questa funzione verra utilizzata per lockare la cam su oggetto se lo si sta guardando -> Lock system
    public void TargetObject(Vector3 moveDirection)
    {
        
        RaycastHit hit;
        float radius = 0.03f;
        float lineLenght = 10f;
        bool hitted;
        hitted = Physics.CapsuleCast(player.playerRigidbody.position, lineStartingPoint.position, radius, moveDirection, out hit);

        if (hitted && hit.collider != null)
        {
            Debug.DrawRay(lineStartingPoint.position, moveDirection * lineLenght, Color.green);
            GameObject hitObject = hit.collider.gameObject;


            if (Input.GetKeyDown(KeyCode.F) && isFpsCameraActive)
            {                                
                    setCameraLockActive();
                    virtualCamera.LookAt = hitObject.transform;

            }

        }
    }

    private void setCameraLockActive()
    {
        fpsCamera.gameObject.SetActive(false);
        lockCamera.gameObject.SetActive(true);

        isFpsCameraActive = false; 
    }
    private void setFpsCameraActive()
    {
        
        lockCamera.gameObject.SetActive(false);
        fpsCamera.gameObject.SetActive(true);

        isFpsCameraActive = true; 
    }
}