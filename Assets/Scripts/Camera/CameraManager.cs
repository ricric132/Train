using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CamState
    {
        train,
        seatPan,
        shopMenu,
        mapCam
    }
    CamState camState = CamState.train;

    [SerializeField] CanvasManager canvasManager;

    //Base cam VARS
    [SerializeField] float scrollSpeed = 100;
    [SerializeField] GameObject baseCam;
    [SerializeField] Transform baseCamCentre;
    Vector3 baseCamOffset = Vector3.zero;
    
    //Seat panner VARS
    [SerializeField] GameObject seatPannerCam;

    //Shop cam VARS
    [SerializeField] GameObject shopMenuCam;

    //Map cam VARS
    [SerializeField] GameObject mapCam;
    float edgeThreshold = 0.2f;
    float mapCamPanSpeed = 20;


    /*
    priority guide:
    0 = off
    1 = base cam
    2 = on
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenMapCam();
        }

        switch (camState)
        {
            case CamState.train:
                baseCamOffset = new Vector3(baseCamOffset.x, Mathf.Clamp(baseCamOffset.y - Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed, -20, 20), -10);
                baseCam.transform.position = baseCamCentre.position + baseCamOffset;
                break;

            case CamState.seatPan:
                break;

            case CamState.shopMenu:
                break;

            case CamState.mapCam:
                UpdateMapCam();
                break;
        }
        
    }

    void UpdateMapCam()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 camVelocity = Vector3.zero;

        if (mousePos.x <= edgeThreshold * Screen.width)
        {
            camVelocity -= new Vector3(Time.deltaTime * mapCamPanSpeed, 0, 0);
        }
        else if(mousePos.x >= Screen.width * (1 - edgeThreshold))
        {
            camVelocity += new Vector3(Time.deltaTime * mapCamPanSpeed, 0, 0);

        }


        if (mousePos.y <= edgeThreshold * Screen.height)
        {
            camVelocity -= new Vector3(0, Time.deltaTime * mapCamPanSpeed, 0);

        }
        else if (mousePos.y >= Screen.height * (1 - edgeThreshold))
        {
            camVelocity += new Vector3(0, Time.deltaTime * mapCamPanSpeed, 0);
        }

        mapCam.transform.position += camVelocity;
    }

    public void PanTo(GameObject obj)
    {
        ResetToDefault();
        seatPannerCam.GetComponent<CinemachineCamera>().Priority = 2;
        seatPannerCam.GetComponent<CinemachineCamera>().Target.TrackingTarget = obj.transform;
        UpdateCamState(CamState.seatPan);
    }

    public void OpenShopCam()
    {
        ResetToDefault();
        shopMenuCam.GetComponent<CinemachineCamera>().Priority = 2;
        UpdateCamState(CamState.shopMenu);
    }

    public void OpenMapCam()
    {
        ResetToDefault();
        //mapCam.GetComponent<CinemachineCamera>().Priority = 2;
        UpdateCamState(CamState.mapCam);
    }

    public void ResetToDefault()
    {
        seatPannerCam.GetComponent<CinemachineCamera>().Priority = 0;
        shopMenuCam.GetComponent<CinemachineCamera>().Priority = 0; 
        mapCam.GetComponent<CinemachineCamera>().Priority = 0;
        UpdateCamState(CamState.train);
    }

    public void UpdateCamState(CamState state)
    {
        camState = state;
        //canvasManager.SetCanvasMode(state);
    }


}
