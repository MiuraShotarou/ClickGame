using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using JetBrains.Annotations;

[ExecuteAlways, RequireComponent(typeof(Animator))]
public class TrailRecoder : MonoBehaviour
{
    Animator _animator;
    //AnimatorControllerからは、オブジェクト固有のものにしなくてはならない
    AnimatorController _animatorController;
    [SerializeField] AnimationClip _animationClip; //ここに充てられたアニメーションクリップのみを改造する
    // 前提：エディタ環境で実行
    // 害虫の動きとサイズをあらかじめエディター上で設定できるようにしておく → Editor拡張を使ってオブジェクトの動きを記録する
    
    // [SerializeField, Header("アニメーションを再生するフレーム数を設定してください")] int _endFlame;
    int _endFlame;
    Vector3 _pos = Vector3.zero;
    Queue<Vector2> _posQueue = new Queue<Vector2>();
    bool _isBeforePressed = false;
    // キーの始点と終点をどうやって設定するかが問題だ。→ 既に２つのキーフレームが打たれている状態
    // && このコンポーネントが付いているオブジェクトをドラッグで動かすと
    // 軌跡が記録されて両端のキーフレーム間で補間される処理
    void OnEnable()
    {
        EditorApplication.update += EditorUpdate; //このイベントに登録された関数は毎フレーム呼び出される
    }
    void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }
    // 
    void EditorUpdate()
    {
        if (!Application.isPlaying) //エディタ環境なら → 実行中の処理負荷防止
        {
            if (_animator) //AnimationClipとAnimatorControllerが設定されていなかったら → エディタ環境でのエラー防止
            {
                
            }
            bool mouseCurrentLeftButtonIsPressed = Mouse.current.leftButton.isPressed;
            // マウスドラッグとレイキャストを用いたドラッグ操作判定
            if (mouseCurrentLeftButtonIsPressed) //ドラッグされているならtrue
            {
                if (Selection.activeGameObject == gameObject //選択されているオブジェクトがこのオブジェクトなら
                    &&
                    _pos != transform.position) //オブジェクトがドラッグされはじめてからドラックが解除されるまで
                {
                    // 
                    _posQueue.Enqueue(transform.position);
                }
            }
            // if (Mouse.current.leftButton.wasReleasedThisFrame) //備考：wasPressedThisFrame wasReleasedThisFrameがはじめて呼び出される際、非常に高速な入力により押下/解放が見逃される可能性がある。どちらかの関数が呼び出されたあと次の入力システムの更新以降・デバイスが破棄されるまでこの問題は発生しなくなる。
            else if (_isBeforePressed) //前回のボタン押し込みがtrueで、今回のボタン押し込みがfalseだったら起動。
            {
                // キーフレーム補間を行う関数を実行
                CreateKeyFrame();
                _posQueue.Clear(); //キーフレーム関数の中にDequeueを追加すれば良い
            }
            _isBeforePressed = mouseCurrentLeftButtonIsPressed;
            _pos = transform.position;
        }
    }
    //エディタ画面でドラッグ操作を終了した時に一度だけ呼び出される。
    void CreateKeyFrame()
    {
        //AnimationClipがなかったら弾く
        _animationClip.ClearCurves(); //
        
        //あった場合、foreachを使って
        //①AnimationCurveに適切なキーフレームフレームを挿入し、
        //②_animationClipにAnimationCurveを適用させる。
        AnimationCurve animationCurveX;
        AnimationCurve animationCurveY;
        // キーフレームの時間調整
        for (int i = 0; i < _endFlame; i++) //
        {
            int flame = i;
            Vector2 deq = _posQueue.Dequeue(); //必然的にすべて空になるはず
            animationCurveX = AnimationCurve.Linear(flame, deq.x, flame, deq.x); //timeを計算しなくてはならない → 自動補完をする必要がある
            animationCurveY = AnimationCurve.Linear(flame, deq.y, flame, deq.y);
            _animationClip.SetCurve("", typeof(Transform), "localPosition.x", animationCurveX); //一旦Linearで検証してみる
            _animationClip.SetCurve("", typeof(Transform), "localPosition.y", animationCurveY);
        }
    }
}
