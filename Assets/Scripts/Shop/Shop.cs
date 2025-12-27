using UnityEngine;

public class Shop : MonoBehaviour
{    
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (gameManager.mouseOverUI)
        {
            return;
        }
        gameManager.ToggleShop();
    }
}
