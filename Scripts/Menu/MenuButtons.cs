using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] private int zeroLevelIndex;

    public void LoadStartLevel()
    {
        SceneLoader.Instance.LoadScene(zeroLevelIndex);
    }
}
