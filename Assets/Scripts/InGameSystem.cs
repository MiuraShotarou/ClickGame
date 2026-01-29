using System;
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
    int _CurrentWave {get => _currentWave; set {if (value > 3) { Debug.LogError("Wave値が4以上"); } _currentWave = value; }}
    void Update()
    {
        float timer = Time.time;
        if (timer > 60f)
        {
            _currentWave = (int)_currentState;
            _currentState = State.WaveEnd; //60秒経ったらWaitModeに入る → 現在のウェーブ数が何であるかはどこで管理すれば良いというのか①Textクラスで管理する②内部保持
        }
        switch (_currentState)
        {
            case State.WaveEnd:
                ObjectManager.Instance.GameEvent.TimeUp();
                ObjectManager.Instance.GameEvent.ShowWaveUI();
                _currentState = State.None;
                break;
            case State.WaveOne: //_currentWaveはTimeで管理する必要がある
                WaveOne(); //Timer始動
                break;
            case State.WaveTwo:
                WaveTwo();
                break;
            case State.WaveThree:
                WaveThree();
                break;
        }
        // Debug.Log(_currentState);
    }

    //Wave1
    void WaveOne()
    {
        Debug.Log("WaveOneになったお");
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
    WaveEnd,
    Interval,
}
