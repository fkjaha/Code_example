using UnityEngine;

public class BloodSplashSpawner : MonoBehaviour
{
    [SerializeField] private HPContainer hpContainer;
    [SerializeField] private ParticleSystem bloodParticles;

    private void Start()
    {
        hpContainer.OnDeath += PlayParticles;
    }

    private void PlayParticles()
    {
        ParticleSystem blood = Instantiate(bloodParticles, hpContainer.transform.position, Quaternion.identity);
        blood.Play(true);
        
        Debug.Log("Spawned");
    }
}
