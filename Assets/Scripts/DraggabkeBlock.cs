using UnityEngine;
using UnityEngine.EventSystems; // イベントシステムを使うために必要

// Event Systemのインターフェースを3つ実装する
// IBeginDragHandler: ドラッグ開始時
// IDragHandler: ドラッグ中
// IEndDragHandler: ドラッグ終了時
public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    // インスペクターで「Lcube」「Icube」「cube1」「cube2」「Tcube」などを入力する
    public string blockID;

    private Camera mainCamera;
    private Vector3 offset; // ブロックの中心とクリック位置のズレ

    private void Awake()
    {
        // 頻繁に使うカメラをキャッシュしておく
        mainCamera = Camera.main;
    }

    // ドラッグが始まった瞬間に呼ばれる
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1. マウスのスクリーン座標を、ゲーム内のワールド座標に変換
        Vector3 mouseWorldPos = GetMouseWorldPosition(eventData.position);

        // 2. ブロックの中心とマウスの位置の差（ズレ）を計算して記録
        offset = transform.position - mouseWorldPos;
    }

    // ドラッグ中に常に呼ばれる
    public void OnDrag(PointerEventData eventData)
    {
        // マウスの現在のワールド座標を取得
        Vector3 mouseWorldPos = GetMouseWorldPosition(eventData.position);

        // 「マウスの現在の座標 ＋ ズレ」の位置に、ブロックを瞬間移動させる
        transform.position = mouseWorldPos + offset;
    }

    // ドラッグが終了した瞬間に呼ばれる（今は空でOK）
    public void OnEndDrag(PointerEventData eventData)
    {
        // いま離された場所から、一番近いマスの「番号(X, Y)」をGameManagerに計算してもらう
        Vector2Int gridPos = GameManager.Instance.GetGridPosition(transform.position);

        // もしそのマスが「枠の中」だったら
        if (GameManager.Instance.IsInsideGrid(gridPos.x, gridPos.y))
        {
            // そのマスの「真ん中の座標」にぴったりスナップさせる
            transform.position = GameManager.Instance.GetWorldPosition(gridPos.x, gridPos.y);

            GameManager.Instance.RegisterBlockID(gridPos.x, gridPos.y, this.blockID);

            Debug.Log($"マス [{gridPos.x}, {gridPos.y}] に {this.blockID} を登録しました！");
        }
        else
        {
            // 枠の外で離されたら、とりあえず元の位置（あるいはドラッグ前の位置）に戻すなどの処理
            Debug.Log("枠の外です！");
        }
    }

   
    // マウスのスクリーン座標（ピクセル）をワールド座標（ゲーム内メートル）に変換する
    private Vector3 GetMouseWorldPosition(Vector2 screenPosition)
    {
        // カメラからの距離を設定（2DなのでZ軸は固定）
        Vector3 playerScreenPos = new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(mainCamera.transform.position.z));

        // 座標変換を行う
        return mainCamera.ScreenToWorldPoint(playerScreenPos);
    }
}