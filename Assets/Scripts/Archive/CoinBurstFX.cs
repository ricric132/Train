using UnityEngine;

public class CoinBurstFX : MonoBehaviour
{
    public int particleCount = 25;
    ParticleSystem particleSystem;
    ParticleSystem.Particle[] particles;
    Vector3[] startingPos;
    public Transform meetSpot;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = new Vector3[particleCount];
        particles = new ParticleSystem.Particle[particleCount];
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Emit(particleCount);
        particleSystem.GetParticles(particles);

    }

    // Update is called once per frame
    void Update()
    {

        
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i].remainingLifetime <= 1)
            {
                if (startingPos[i] == Vector3.zero)
                {
                    startingPos[i] = particles[i].position;
                }
                particles[i].position = Vector3.Lerp(startingPos[i], meetSpot.position, (1 - particles[i].remainingLifetime));
            }
        }
        
    }
}
