using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerManager : MonoBehaviour
{
    int positive;
    int negative;

    int upgradeMoney;

    CanvasManager canvasManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasManager = FindFirstObjectByType<CanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPositive(int num)
    {
        positive += num;
        canvasManager.UpdatePositiveText(positive);
    }

    public int GetPositive()
    {
        return positive;
    }

    public void AddNegative(int num)
    {
        negative += num;
        canvasManager.UpdateNegativeText(negative); 
    }

    public void UpdateMoney(int amount)
    {
        upgradeMoney += amount;
    }

    public int GetMoney()
    {
        return upgradeMoney;
    }
}

public class RunStats
{

}
