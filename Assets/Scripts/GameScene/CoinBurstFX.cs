using UnityEngine;

public class CoinBurstFX : MonoBehaviour
{
    public ParticleSystem particleSystem;

    bool started = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void StartEffect(int particleCount)
    {
        particleSystem.Emit(particleCount);
        started = true; 
    }

    // Update is called once per frame
    void Update()
    {
        if(particleSystem.particleCount == 0 && started)
        {
            Destroy(gameObject);
        }
    }
}
