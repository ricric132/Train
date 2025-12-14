using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] CameraManager cameraManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        cameraManager.OpenShopCam();
    }
}
