using System.Runtime.CompilerServices;
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
    [SerializeField] AnimationCurve scrollSmoothingCurve;
    float prevScrollVal = 0;
    float scrollTime = 0;
    float stopTime = 0;
    [SerializeField] float lerpDuration = 0.2f;
    float recentScroll = 0;
    float targetOffset = 0;
    Vector3 baseCamOffset = Vector3.zero;
    
    //Seat panner VARS
    [SerializeField] GameObject seatPannerCam;

    //Shop cam VARS
    [SerializeField] GameObject shopMenuCam;

    //Map cam VARS
    [SerializeField] GameObject mapCam;
    float edgeThreshold = 0.2f;
    float mapCamPanSpeed = 20;

    float camSpeedMult = 1;

    /*
    priority guide:
    0 = off
    1 = base cam
    2 = on
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CinemachineCore.GetBlendOverride += HandleBlendOverride;
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
                UpdateTrainCam();
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

    void UpdateTrainCam() // maybe should make it fixed update but probs not
    {
        float scrollVal = Input.mouseScrollDelta.y;
        if(scrollVal > 0)
        {
            stopTime = 0;
            if(prevScrollVal >= 0)
            {
                scrollTime += Time.deltaTime;
            }
            else
            {
                scrollTime = Time.deltaTime;
                targetOffset = baseCamOffset.y;
                prevScrollVal = scrollVal;
            } 
        }
        else if (scrollVal < 0)
        {
            stopTime = 0;
            if (prevScrollVal <= 0)
            {
                scrollTime += Time.deltaTime;
            }
            else
            {
                scrollTime = Time.deltaTime;
                targetOffset = baseCamOffset.y;
                prevScrollVal = scrollVal;
            }
        }
        else
        {
            stopTime += Time.deltaTime;
            if(stopTime >= 0.05f)
            {
                scrollTime = 0;
                targetOffset = baseCamOffset.y;
                prevScrollVal = scrollVal;
            }
        }

        //Debug.Log(Input.mouseScrollDelta.y);
        targetOffset = Mathf.Clamp(targetOffset - Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed, -20, 20);
        baseCamOffset = new Vector3(baseCamOffset.x, Mathf.Lerp(baseCamOffset.y, targetOffset, Mathf.Clamp(scrollTime/lerpDuration, 0, 1)), -10);
        baseCam.transform.position = baseCamCentre.position + baseCamOffset;
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

    public void PanTo(GameObject obj, float speed)
    { 
        camSpeedMult = speed;
        ResetToDefault();
        seatPannerCam.GetComponent<CinemachinePositionComposer>().Damping = Vector3.one * (1/speed);
        seatPannerCam.GetComponent<CinemachineCamera>().Priority = 2;
        seatPannerCam.GetComponent<CinemachineCamera>().Target.TrackingTarget = obj.transform;
        UpdateCamState(CamState.seatPan);
    }

    CinemachineBlendDefinition HandleBlendOverride(ICinemachineCamera fromVcam, ICinemachineCamera toVcam, CinemachineBlendDefinition defaultBlend, Object owner)
    {
        if (fromVcam.Name == baseCam.name && toVcam.Name == seatPannerCam.name)
        {
            Debug.Log("applied custom blend");
            CinemachineBlendDefinition customBlend = defaultBlend;
            customBlend.Time = 1 / camSpeedMult; //maybe switch to cut at certain speed threshhold
            return customBlend;
        }

        return defaultBlend;
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
        seatPannerCam.GetComponent<CinemachinePositionComposer>().Damping = Vector3.one;

        UpdateCamState(CamState.train);
    }

    public void UpdateCamState(CamState state)
    {
        camState = state;
        //canvasManager.SetCanvasMode(state);
    }


}
