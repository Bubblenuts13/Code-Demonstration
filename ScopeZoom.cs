using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeZoom : MonoBehaviour
{

    public GameObject player;
    public GameObject scopeCam;
    public GameObject scopeCamera;
    public GameObject spyGlassCanvas;
    public GameObject Hud;
    public GameObject pauseMenu;
    public Camera scopeView;
    //The text and effect gameobject may need to be activated/deactivated as well, or parented to the camera.
    public GameObject zoomText;
    public float startFov;
    private float fov;
    public float minFov;
    public float maxFov;
    public float adjustSpeed;
    public float rotateSpeed;
    private bool isScoped = false;
    private bool canAdjust;
    //private bool doOnce = false;
    private bool canScope = false;
    //public Transform camTransform;
    private Quaternion camTransform;
    //private bool isZooming = false;

    //public Vector3 maxRotationUp;
    //public Vector3 maxRotationDown;
    //public Quaternion maxRotationRight;
    //public Vector3 maxRotationLeft;

    private bool canMoveRight = true;
    private bool canMoveLeft = true;
    private bool canMoveUp = true;
    private bool canMoveDown = true;

    //private bool canRotate = true;
    
    // Start is called before the first frame update
    void Start()
    {
        fov = startFov;
        camTransform = scopeCamera.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        scopeView.fieldOfView = fov;
        if(isScoped == true)
        {
            scopeCamera.transform.eulerAngles = new Vector3(0, scopeCamera.transform.eulerAngles.y, scopeCamera.transform.eulerAngles.z);
            if (Input.GetKey(KeyCode.Space) && fov >= minFov)
            {
                fov -= adjustSpeed * Time.fixedDeltaTime * Time.timeScale;
            }
            if (Input.GetKey(KeyCode.LeftShift) && fov <= maxFov || Input.GetKey(KeyCode.RightShift) && fov <= maxFov)
            {
                fov += adjustSpeed * Time.fixedDeltaTime * Time.timeScale;
            }
            if (Input.GetKey(KeyCode.D) && canMoveRight == true)
            {
                scopeCamera.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * Time.timeScale);
                if (canMoveLeft == false)
                {
                    canMoveLeft = true;
                }
            }
            if (Input.GetKey(KeyCode.A) && canMoveLeft == true)
            {
                scopeCamera.transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime * Time.timeScale);
                if(canMoveRight == false)
                {
                    canMoveRight = true;
                }
            }
            if (Input.GetKey(KeyCode.W) && canMoveUp == true)
            {
                scopeCamera.transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime * Time.timeScale);
                if (canMoveDown == false)
                {
                    canMoveDown = true;
                }
            }
            if (Input.GetKey(KeyCode.S) && canMoveDown == true)
            {
                scopeCamera.transform.Rotate(Vector3.forward * -rotateSpeed * Time.deltaTime * Time.timeScale);
                if (canMoveUp == false)
                {
                    canMoveUp = true;
                }
            }
            if(pauseMenu.activeSelf == true)
            {
                if(spyGlassCanvas.activeSelf == true)
                {
                    spyGlassCanvas.SetActive(false);
                }
            }
            if(pauseMenu.activeSelf == false)
            {
                if(spyGlassCanvas.activeSelf == false)
                {
                    spyGlassCanvas.SetActive(true);
                }
            }


            if (Input.GetKeyDown(KeyCode.E))
            {
                //isScoped = false;
                Invoke("ScopeReset", 0.1f);
                zoomText.SetActive(true);
                player.SetActive(true);
                scopeCam.SetActive(false);
                Debug.Log("UnScope");
                Hud.SetActive(true);
                //since this text may be attached to the canvas on the player prefab, It may autoremove itself. I can remove this function later
                //I don't need to set up the instructional text or the camera since they will be within the scope came game object
            }
        }
        //if(Input.GetKeyUp(KeyCode.E) && isScoped == true)
        //{
        //    isScoped = false;
        //}
        if(canScope == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && isScoped == false)
            {
                zoomText.SetActive(false);
                player.SetActive(false);
                scopeCam.SetActive(true);
                isScoped = true;
                fov = startFov;
                scopeCamera.transform.rotation = camTransform;
                if(Hud.activeSelf == true)
                {
                    Hud.SetActive(false);
                    Debug.Log("Hud"); 
                }
                
                
                
            }
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        //if(other.gameObject.tag == "Player")
        //{

        //    if (Input.GetKeyDown(KeyCode.E) && isScoped == false && doOnce == false)
        //    {
        //        zoomText.SetActive(false);
        //        player.SetActive(false);
        //        scopeCam.SetActive(true);
        //        isScoped = true;
        //        fov = startFov;
        //        doOnce = true;
        //        Debug.Log("Scoping");
        //    }
        //}
        if (other.gameObject.tag == "RightCheck")
        {
            if(canMoveRight == true)
            {
                canMoveRight = false;
            }
            
            Debug.Log("CannotMoveRight");
        }
        if (other.gameObject.tag == "LeftCheck")
        {
            
            if (canMoveLeft == true)
            {
                canMoveLeft = false;
            }
            Debug.Log("CannotMoveLeft");
        }
        if (other.gameObject.tag == "TopCheck")
        {
           
            if (canMoveUp == true)
            {
                canMoveUp = false;
            }
            Debug.Log("CannotMoveUp");
        }
        if (other.gameObject.tag == "BottomCheck")
        {
            
            if (canMoveDown == true)
            {
                canMoveDown = false;
            }
            Debug.Log("CannotMoveDown");
        }
    }
    private void ScopeReset()
    {
        if(isScoped == true)
        {
            isScoped = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            zoomText.SetActive(false);
            Debug.Log("DeactivatingZoomText");
        }
        if(canScope == true)
        {
            canScope = false;
        }
        //if(other.gameObject.tag == "Fish")
        //{
        //    canMoveRight = true;
        //}
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (zoomText.activeSelf == false)
            {
                zoomText.SetActive(true);
                Debug.Log("ActivatingZoomText");
            }
            if(canScope == false)
            {
                canScope = true;
                Debug.Log("CanZoom");
            }
        }
        if(other.gameObject.tag == "RightCheck")
        {
            canMoveRight = false;
            Debug.Log("CannotMoveRight");
        }
        if (other.gameObject.tag == "LeftCheck")
        {
            canMoveLeft = false;
            Debug.Log("CannotMoveLeft");
        }
        if (other.gameObject.tag == "TopCheck")
        {
            canMoveUp = false;
            Debug.Log("CannotMoveUp");
        }
        if (other.gameObject.tag == "BottomCheck")
        {
            canMoveDown = false;
            Debug.Log("CannotMoveDown");
        }
    }
    //maybe ontriggerenter;
}
