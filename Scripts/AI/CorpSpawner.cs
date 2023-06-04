using Unity.Mathematics;
using UnityEngine;

public class CorpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject corpPrefab;
    [SerializeField] private HPContainer hpContainer;

    private void Start()
    {
        hpContainer.OnDeath += SpawnCorp;
    }

    private void SpawnCorp()
    {
        Instantiate(corpPrefab, hpContainer.transform.position, quaternion.identity);
    }
}
