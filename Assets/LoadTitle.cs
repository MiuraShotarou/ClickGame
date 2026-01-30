using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTitle : MonoBehaviour
{
    void Awake() => SceneManager.LoadScene("Title");
}
