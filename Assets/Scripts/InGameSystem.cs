using UnityEngine;
public class InGameSystem : MonoBehaviour
{
    // メモ
    // Moth → PixelsPerUnit 500 ~ 700
    // Fly → 600 ~ 800
    GameObject FlyObj600; //大きい
    GameObject FlyObj700;
    GameObject FlyObj800; //小さい
    GameObject MothObj500;
    GameObject MothObj600;
    GameObject MothObj700;
    State _currentState = State.None;
    int _currentWave = 0;
    int _frameCounter = 0;
    int _CurrentWave {get => _currentWave; set {if (value > 3) { Debug.LogError("Wave値が4以上"); } _currentWave = value; }}
    void Update()
    {
        switch (_currentState)
        {
            case State.Rule:
                ObjectManager.Instance.GameEvent.ActiveRulePanel();
                _currentState = State.None;
                break;
            case State.InWave:
                _frameCounter = Time.frameCount;
                if (_frameCounter > 60 * 60) //60fps * 60Sec == iMin
                {
                    _currentWave = (int)_currentState;
                    _currentState = State.WaveEnd; //60秒経ったらWaitModeに入る
                    _frameCounter = 0; //_frameCounterをリセット
                }
                break;
            case State.WaveOne:
                WaveOne(); //Timer始動
                break;
            case State.WaveTwo:
                WaveTwo();
                break;
            case State.WaveThree:
                WaveThree();
                break;
            case State.WaveEnd:
                ObjectManager.Instance.GameEvent.TimeUp();
                ObjectManager.Instance.GameEvent.ShowWaveUI();
                _currentState = State.None;
                break;
        }
        // Debug.Log(_currentState);
    }

    //Wave1
    void WaveOne()
    {
        Debug.Log("WaveOneになったお");
        //基本的なシステム → _timerに従って的を出す順番を決める。
        //しかし、実行するたびに誤差が出てしまう。これはフェアじゃない。
        //フレーム単位で出せるようにしたいな。
        
        // FlyObj600.transform.Translate();
        //横サインカーブ * 10
        //横一直線 * 1
        //横螺旋 * 1
        //横不規則 * 1
    }
    void WaveTwo()
    {
        
    }
    void WaveThree()
    {
        
    }
    // public void NextWave() => _currentState = (State)_currentWave++; Debug.Log("");
    public void NextWave()
    {
        _currentState = (State)(++_currentWave);
    }
}
public enum State
{
    None,
    WaveOne,
    WaveTwo,
    WaveThree,
    Rule,
    InWave,
    WaveEnd,
}
