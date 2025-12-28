using TMPro;
using UnityEngine;

public class NextStationClick : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI requirementsText; 
    GameManager gameManager;
    TrainManager trainManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        trainManager = gameManager.trainManager;
    }

    // Update is called once per frame
    void Update()
    {
        requirementsText.text = "";
        if (trainManager.HasDisembarkable())
        {
            requirementsText.text = "Passengers ready to disembark, cannot leave station";
        }
    }

    private void OnMouseDown()
    {
        if (gameManager.mouseOverUI)
        {
            return;
        }
        if (trainManager.HasDisembarkable())
        {
            return;
        }
        gameManager.NextStation();
    }
}
