using UnityEngine;

public class Shop : MonoBehaviour
{    
    GameManager gameManager;

    void Awake()
    {
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
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
