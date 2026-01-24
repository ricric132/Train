using Unity.Cinemachine;
using UnityEngine;

public class MapPanel : MonoBehaviour
{
    [SerializeField] CinemachineCamera bindedCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float widthRatio = Camera.main.aspect;
        transform.localScale = new Vector3(bindedCam.Lens.OrthographicSize * widthRatio * 2, bindedCam.Lens.OrthographicSize/2, 1);
        transform.localPosition = new Vector3(0, -3 * bindedCam.Lens.OrthographicSize / 4, 1);
    }
}
