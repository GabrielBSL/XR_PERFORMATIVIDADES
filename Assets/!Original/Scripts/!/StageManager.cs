using UnityEngine;

public class StageManager : MonoBehaviour
{
    //================ SINGLETON LOGIC ================
    public static StageManager instance = null;
    public void SingletonCheck()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    public enum Stage
    {
        Intro, Tutorial,
        Jornada1, Aldeia1,
        Jornada2, Aldeia2,
        Jornada3, Aldeia3, Flutuacao
    }
    
    [Range(0f, 8f)] public float currentProgress;
    public Stage currentStage { get{ return (Stage) currentProgress; }}
    public float normalizedProgress{ get{ return currentProgress / 8; }}
    public float localProgress{ get{ return currentProgress % 1; }}
    [SerializeField] CubicBezier raftPath;

    public void Awake()
    {
        SingletonCheck();
    }
    public void Update()
    {
        DayNightManager.instance.time = normalizedProgress;
        raftPath.t = normalizedProgress;
        //Debug.Log($"currentProgress = \t{currentProgress}");
        //Debug.Log($"currentStage = \t{currentStage}");
        //Debug.Log($"normalizedProgress = \t{normalizedProgress}");
        //Debug.Log($"localProgress = \t{localProgress}\n");
    }
}
