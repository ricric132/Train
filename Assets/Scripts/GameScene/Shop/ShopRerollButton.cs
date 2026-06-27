using UnityEngine;

public class ShopRerollButton : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnMouseDown()
    {
        if (gameManager.mouseOverUI)
        {
            return;
        }
        gameManager.shopManager.BuyShopReroll();
    }
}
