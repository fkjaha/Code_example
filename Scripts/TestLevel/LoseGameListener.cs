using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGameListener : MonoBehaviour
{
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private HPContainer playerHp;

    private void Start()
    {
        playerHp.OnDeath += GameLost;
    }

    private void GameLost()
    {
        loseScreen.SetActive(true);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
