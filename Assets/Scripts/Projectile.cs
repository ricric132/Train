using System.Collections;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    Vector3 startingPoint;
    Transform target;
    float timeTravelled = 0;
    float totalTime = 1;

    public bool arrived;

    Camera startCam;
    Camera targetCam;

    public void Launch(Vector3 _start, Transform _target, float timeTaken = 1) // these are world positions
    {
        startCam = GameManager.Instance.cameraManager.FindObjectCamera(_start);
        startingPoint = startCam.WorldToScreenPoint(_start);
        transform.position = startingPoint;
        

        targetCam = GameManager.Instance.cameraManager.FindObjectCamera(_target.gameObject);
        target = _target;
        totalTime = timeTaken;  
    }

    public IEnumerator LaunchAndWait(Vector3 _start, Transform _target, float timeTaken = 1)
    {
        Launch(_start, _target, timeTaken);

        while (!arrived)
        {
            yield return null;
        }

        yield return true; 
    }

    // Update is called once per frame
    void Update()
    {
        if (arrived && timeTravelled >= totalTime + 1)
        {
            Destroy(gameObject);
        }
        timeTravelled += Time.deltaTime;
        Vector3 targetPos = targetCam.WorldToScreenPoint(target.position);
        transform.position = Vector3.Lerp(startingPoint, targetPos, curve.Evaluate(Mathf.Clamp(timeTravelled/totalTime, 0, 1)));

        if (timeTravelled >= totalTime)
        {
            arrived = true;
        }
    
    }

    
}
