using UnityEngine;

public class ContractGiver : MonoBehaviour
{
    GameManager gameManager;
    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnMouseDown()
    {
        if (gameManager.mouseOverUI)
        {
            return;
        }
        gameManager.SetupContractMenu();
        gameManager.ToggleContractMenu();
    }
}
