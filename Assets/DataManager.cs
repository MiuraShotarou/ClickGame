using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    void Awake()
    {
        if (Instance ==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public int Score; //まだ使わない
}
