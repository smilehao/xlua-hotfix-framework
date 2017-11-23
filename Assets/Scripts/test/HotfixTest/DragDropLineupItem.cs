//using UnityEngine;
//using System.Collections;
//using XLua;

//[Hotfix]
//public class DragDropLineupItem : UIDragDropItem {

//    private Vector3 tmpPos = Vector3.zero;
//    private BoxCollider[] colliders = null;
//    private UILineupItem lineupItem1 = null;
//    private UILineupItem lineupItem2 = null;

//    protected override void OnEnable() {

//        base.OnEnable();

//        tmpPos = Vector3.zero;
//        colliders = gameObject.GetComponentsInChildren<BoxCollider>();
//        lineupItem1 = gameObject.GetComponent<UILineupItem>();

//    }
//    protected override void OnDisable() {

//        colliders = null;

//        base.OnDisable();
//    }


//    protected override void ResetPosition()
//    {
//    }

//    protected override void OnDragDropStart()
//    {
//        if (colliders != null)
//        {
//            for (int i = 0; i < colliders.Length; i++)
//            {
//                colliders[i].enabled = false;
//            }
//        }
//        tmpPos = gameObject.transform.localPosition;

//        lineupItem1.ShowShadow(true);

//        base.OnDragDropStart();
//    }

//    protected override void OnDragDropRelease(GameObject surface)
//    {
//        if (!UIManager.Instance().WindowIsOpen(TheUIPrefabIds.UI_LINEUP_MAIN))
//        {
//            Destroy(gameObject);
//            return;
//        }

//        if (colliders != null)
//        {
//            for (int i = 0; i < colliders.Length; i++)
//            {
//                colliders[i].enabled = true;
//            }
//        }

//        lineupItem1.ShowShadow(false);

//        if (IsLineupItemGo(surface, out lineupItem2))
//        {
//            if (lineupItem1 == null || lineupItem2 == null)
//            {
//                base.OnDragDropRelease(surface);

//                ResetPos();
//                return;
//            }

//            Messenger.AddListener<int>(MessageName.MN_TROOP_EXCHANGE, HandleResult);
//            Player.instance.LineupMgr.ReqExChangeTroop(lineupItem1.LineupType, lineupItem1.TroopId, lineupItem2.TroopId);
//        }
//        else
//        {
//            base.OnDragDropRelease(surface);

//            ResetPos();
//        }
//    }


//    private bool IsLineupItemGo(GameObject go, out UILineupItem lineupItem)
//    {
//        lineupItem = NGUITools.FindInParents<UILineupItem>(go);
//        return lineupItem != null;
//    }

//    private void ChangePos()
//    {
//        if (lineupItem2 != null)
//        {
//            gameObject.transform.localPosition = lineupItem2.transform.localPosition;
//            lineupItem2.transform.localPosition = tmpPos;
            
//            lineupItem1.SetPos();
//            lineupItem2.SetPos();
//        }
//    }

//    public void ResetPos()
//    {
//        gameObject.transform.localPosition = tmpPos;
//    }

//    private void HandleResult(int result)
//    {
//        Messenger.RemoveListener<int>(MessageName.MN_TROOP_EXCHANGE, HandleResult);

//        base.OnDragDropRelease(lineupItem2.gameObject);

//        if(result > 0)
//        {
//            ResetPos();
//            Utils.FloatAlert(Language.instance.GetErrorString(result));
//        }
//        else
//        {
//            ChangePos();
//        }
//    }
//}
