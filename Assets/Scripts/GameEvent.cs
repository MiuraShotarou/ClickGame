using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class GameEvent : MonoBehaviour
{
    float _showT_WaveTime = 2.5f;        //Waveの字が出現するまで
    float _unShowT_WaveTime = 1.5f;      //Waveの字が消滅するまで
    float _showT_WaveCountTime = 2f;   //1,2,3の字が出現するまで 
    float _unShowT_WaveCountTime = 1.5f; //1,2,3の字が消滅するまで
    float _showT_WaveStartTime = 0.5f;   //1,2,3の字が出現するまで
    float _unShowT_WaveStartTime = 0.5f; //1,2,3の字が消滅するまで
    public void TimeUp()
    {
        // Whistle(); //オーディオを鳴らすだけの関数
        ShowUI();
    }
    void Whistle()
    {
        ObjectManager.Instance.AudioSource.Play();
    }
    void ShowUI()
    {
        ObjectManager.Instance.T_TimeUpObj.transform.Translate(new Vector3(-1, 0, 0));
    }
    public void ShowWaveUI()
    {
        //DOTween
        Sequence sequence = DOTween.Sequence();
        //Wave & WaveCountの表示コード
        sequence.Append(ObjectManager.Instance.T_Wave.GetComponent<TextMeshProUGUI>().DOFade(1, _showT_WaveTime));
        sequence.Append(ObjectManager.Instance.T_WaveCount.GetComponent<TextMeshProUGUI>().DOFade(1, _showT_WaveCountTime));
        //Wave & WaveCountの消滅コード
        sequence.Append(ObjectManager.Instance.T_Wave.GetComponent<TextMeshProUGUI>().DOFade(0, _unShowT_WaveTime));
        sequence.Join(ObjectManager.Instance.T_WaveCount.GetComponent<TextMeshProUGUI>().DOFade(0, _unShowT_WaveCountTime));
        //WaveStart の表示
        sequence.Append(ObjectManager.Instance.T_WaveStart.GetComponent<TextMeshProUGUI>().DOFade(1, _showT_WaveStartTime));
        sequence.AppendInterval(0.5f);
        sequence.Append(ObjectManager.Instance.T_WaveStart.GetComponent<TextMeshProUGUI>().DOFade(0, _unShowT_WaveStartTime));
        sequence.AppendCallback(ObjectManager.Instance.InGameSystem.NextWave);
    }
}
