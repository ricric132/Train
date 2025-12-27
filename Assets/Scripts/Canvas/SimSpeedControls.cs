using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimSpeedControls : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    List<float> speedValues = new List<float>() { 1, 1.5f, 2, 4, 8 };
    int trainSpeedIndex = 0;
    [SerializeField] TextMeshProUGUI trainSpeedText;
    int animSpeedIndex = 0;
    [SerializeField] TextMeshProUGUI animSpeedText;

    void Awake()
    {
        gameManager.trainMoveSimSpeed = speedValues[trainSpeedIndex];
        trainSpeedText.text =  "x"+speedValues[trainSpeedIndex];

        gameManager.animationSimSpeed = speedValues[animSpeedIndex];
        animSpeedText.text = "x" + speedValues[animSpeedIndex];

    }

    public void OnClickedTrainSpeed()
    {
        trainSpeedIndex = (trainSpeedIndex + 1)%speedValues.Count;
        gameManager.trainMoveSimSpeed = speedValues[trainSpeedIndex];
        trainSpeedText.text = "x" + speedValues[trainSpeedIndex];
    }


    public void OnClickedAnimSpeed()
    {
        animSpeedIndex = (animSpeedIndex + 1) % speedValues.Count;
        gameManager.animationSimSpeed = speedValues[animSpeedIndex];
        animSpeedText.text = "x" + speedValues[animSpeedIndex];

    }
}
