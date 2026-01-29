using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

[ExecuteAlways, RequireComponent(typeof(Animator))]
public class TrailRecoder : MonoBehaviour
{
    Animator _animator;
    //AnimatorControllerからは、オブジェクト固有のものにしなくてはならない
    AnimatorController _animatorController; //CreateFlame関数でのみ使用する
    [SerializeField] AnimationClip _animationClip; //ここに充てられたアニメーションクリップのみを改造する
    // 前提：エディタ環境で実行
    // 害虫の動きとサイズをあらかじめエディター上で設定できるようにしておく → Editor拡張を使ってオブジェクトの動きを記録する
    
    // [SerializeField, Header("アニメーションを再生するフレーム数を設定してください")] int _endFlame;
    int _endFlame = 0;
    Queue<Vector2> _posQueue = new Queue<Vector2>();
    bool _isBeforePressed = false;
    bool _isDragging = false;
    // キーの始点と終点をどうやって設定するかが問題だ。→ 既に２つのキーフレームが打たれている状態
    // && このコンポーネントが付いているオブジェクトをドラッグで動かすと
    // 軌跡が記録されて両端のキーフレーム間で補間される処理
    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        _isBeforePressed = false;
        _endFlame = 0; //OnEnableで不具合が起きるようであれば、EditorUpdate内で !Selection.activeGameObject == gameObject のとき初期化をしても良い
        Debug.Log(Mathf.PerlinNoise(1, 10)); //
        Debug.Log(Mathf.PerlinNoise(1.1f, 10)); //
        Debug.Log(Mathf.PerlinNoise(1.2f, 10)); //
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    void OnSceneGUI(SceneView sceneView)
    {
        if (!Application.isPlaying) //エディタ環境なら → 実行中の処理負荷防止
        {
            bool isOnViewDragging = UnityEngine.Event.current.type == EventType.MouseDrag && UnityEngine.Event.current.button == 0; //SceneView上で左クリックボタンが押されたうえでマウスが動いたなら
            // ドラッグ操作判定
            if (isOnViewDragging
                &&
                Selection.activeGameObject == gameObject) //これはいる //左クリック && ドラッグ操作なら（マウスボタンを押してマウスを動かしたなら）
            {
                _isDragging = true;
            }
            else if (_isBeforePressed != Mouse.current.leftButton.isPressed) //前回のボタン押し込みがtrueで、今回のボタン押し込みがfalseだったら
            {
                _isDragging = false;
            }
            // _isDraggingを用いた処理分岐
            if (_isDragging) //ドラッグ中なら
            {
                // 軌跡を記録
                _posQueue.Enqueue(transform.position);
                _endFlame++; //queueと同じ大きさにしてみる
            }
            else if (_posQueue.Count > 0) // && !_isDragging
            {
                if (!_animationClip) //animClip
                {
                    Debug.LogWarning("AnimationClipがセットされていません");
                    _posQueue.Clear();
                }
                else
                {
                    // キーフレーム補間を行う関数を実行
                    CreateKeyFrame();
                    _endFlame = 0;
                    _posQueue.Clear();
                }
            }
            _isBeforePressed = Mouse.current.leftButton.isPressed;
        }
    }
    //エディタ画面でドラッグ操作を終了した時に一度だけ呼び出される。
    void CreateKeyFrame()
    {
        //AnimationClipがなかったら弾く
        if (AnimationUtility.GetCurveBindings(_animationClip).Length > 0)
        {
            _animationClip.ClearCurves(); //
        }
        //あった場合、foreachを使って
        //①AnimationCurveに適切なキーフレームフレームを挿入し、
        //②_animationClipにAnimationCurveを適用させる。
        float frameRate = _animationClip.frameRate;
        // frameRate /= 6;
        Debug.Log($"frameRate{frameRate}");
        AnimationCurve animationCurveX = AnimationCurve.Linear(0, 0, _endFlame / frameRate, 0); //EaseInOut
        AnimationCurve animationCurveY = AnimationCurve.Linear(0, 0, _endFlame / frameRate, 0);
        // キーフレームの時間調整
        for (int i = 0; i < _endFlame; i++) //
        {
            float time = i / frameRate; // / frameRate
            Vector2 deq = _posQueue.Dequeue(); //必然的にすべて空になるはず
            animationCurveX.AddKey(time, deq.x);
            animationCurveY.AddKey(time, deq.y);
            // Positionが変更されていないようだ
        }
        _animationClip.SetCurve("", typeof(Transform), "localPosition.x", animationCurveX); //一旦Linearで検証してみる
        _animationClip.SetCurve("", typeof(Transform), "localPosition.y", animationCurveY);
    }
}
