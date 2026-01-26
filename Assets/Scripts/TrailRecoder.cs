using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;
[ExecuteAlways]
public class TrailRecoder : MonoBehaviour
{
    // 前提：エディタ環境で実行
    // 害虫の動きとサイズをあらかじめエディター上で設定できるようにしておく → Editor拡張を使ってオブジェクトの動きを記録する
    
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
        if (!Application.isPlaying)
        {
            // マウスドラッグとレイキャストを用いたドラッグ操作判定
            if (Mouse.current.leftButton.isPressed) //ドラッグされているなら
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                // 自分自身がヒットしているなら → 追記する必要あり
                if (Physics2D.Raycast(ray.origin, ray.direction))
                {
                    Debug.Log("当たった: "); //あんまりよろしいやり方ではないな
                    // Hierarchy で選択されているオブジェクトを取得し、それの軌跡を記録するほうが良いか？
                }
            }
        }
    }
}
