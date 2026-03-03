using Coffee.UIExtensions;
using UnityEngine;

public class ParticleEffectSpawner : MonoBehaviour
{
    [SerializeField] UIParticleAttractor coinAttractor;

    public GameObject coinBurstPrefab;
    public Transform canvasTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCoinBurst(Vector2 canvasPos, int coinAmt)
    {   
        GameObject effectGO = Instantiate(coinBurstPrefab, canvasPos, Quaternion.identity, canvasTransform);
        effectGO.GetComponent<CoinBurstFX>().StartEffect(coinAmt);
        coinAttractor.AddParticleSystem(effectGO.GetComponent<CoinBurstFX>().particleSystem);
    }
}
