using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadInGame : MonoBehaviour
{
    public void OnLoadInGame() => SceneManager.LoadScene("InGame");
}
