using UnityEngine;
using TMPro;

public class ObjectManager : MonoBehaviour
{
    //Script
    [SerializeField] public InGameSystem InGameSystem;
    [SerializeField] public GameEvent GameEvent;
    //Prefab
    [SerializeField] public GameObject FlyObj600;
    [SerializeField] public GameObject FlyObj700;
    [SerializeField] public GameObject FlyObj800;
    [SerializeField] public GameObject MothObj500;
    [SerializeField] public GameObject MothObj600;
    [SerializeField] public GameObject MothObj700;
    //GameObject
    [SerializeField] public GameObject P_RuleObj;
    [SerializeField] public GameObject T_ReadyObj;
    [SerializeField] public GameObject T_TimeUpObj;
    [SerializeField] public GameObject T_Wave;
    [SerializeField] public GameObject T_WaveCount;
    [SerializeField] public GameObject T_WaveStart;
    //Audio
    [SerializeField] public AudioSource AudioSource;
    //Animator
    [SerializeField] public Animator Animator;
    public static ObjectManager Instance;
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
    void Start()
    {
        Init();
    }
    void Init()
    {
        P_RuleObj.SetActive(true);
        T_ReadyObj.SetActive(true);      T_ReadyObj.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        T_TimeUpObj.SetActive(true);    T_TimeUpObj.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        T_Wave.SetActive(true);          T_Wave.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        T_WaveCount.SetActive(true);     T_WaveCount.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        T_WaveStart.SetActive(true);     T_WaveStart.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
    }
}
