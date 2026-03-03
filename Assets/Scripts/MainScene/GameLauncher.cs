using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLauncher : MonoBehaviour
{
    [SerializeField] ConductorSelector conductorSelector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchGame()
    {
        RunBuffData.selectedConductor = conductorSelector.GetActiveConductor();
        SceneManager.LoadScene("GameScene");
    }
}
