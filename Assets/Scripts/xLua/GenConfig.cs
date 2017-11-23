using System.Collections.Generic;
using System;
using UnityEngine;
using XLua;

public static class GenConfig
{
	//lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
	[LuaCallCSharp]
	public static List<Type> LuaCallCSharp = new List<Type>() {
		// unity
		typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(List<int>),
        typeof(Action<string>),
        typeof(UnityEngine.Debug),
        typeof(UnityEngine.UI.Text),
        typeof(Delegate),
        typeof(Dictionary<string, GameObject>),

		// NGUI
		//typeof(NGUITools),
  //      typeof(UIWidget),
  //      typeof(UIAtlas),
  //      typeof(UIAnchor),
  //      typeof(UICamera),
  //      typeof(UIFont),
  //      typeof(UIInput),
  //      typeof(UILabel),
  //      typeof(UIPanel),
  //      typeof(UIRoot),
  //      typeof(UISprite),
  //      typeof(UIButton),
  //      typeof(UIDragScrollView),
  //      typeof(UIGrid),
  //      typeof(UIPlayTween),
  //      typeof(UIScrollView),
  //      typeof(UISlider),
  //      typeof(UITable),
  //      typeof(UIToggle),
  //      typeof(UIWrapContent),
  //      typeof(SpringPosition),
  //      typeof(TweenAlpha),
  //      typeof(TweenColor),
  //      typeof(TweenPosition),
  //      typeof(TweenRotation),
  //      typeof(TweenScale),

		// UILogic
        //typeof(BaseItem),
        //typeof(BaseUI),
        //typeof(TheGameIds),
        //typeof(TheUIPrefabIds),
        //typeof(UIManager),
        //typeof(UI_Core.UIManagerHelper),
        //typeof(UIPathData),
        //typeof(UIGameObjectPool),

		// game logic
		//typeof(Battle.CtlBattle),
  //      typeof(ResourceMgr),
  //      typeof(GameObjectPool),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
		// unity
		typeof(Action),
        typeof(Callback),
        typeof(UnityEngine.Event),
        typeof(UnityEngine.Events.UnityAction),
        typeof(System.Collections.IEnumerator),

        // NGUI
        //typeof(UICenterOnChild.OnCenterCallback),
        //typeof(UIGrid.OnReposition),
        //typeof(UIProgressBar.OnDragFinished),
        //typeof(UIScrollView.OnDragNotification),
        //typeof(UITable.OnReposition),
        //typeof(UIToggle.Validate),
        //typeof(UIWrapContent.OnInitializeItem),
        //typeof(EventDelegate.Callback),
        //typeof(Localization.LoadFunction),
        //typeof(Localization.OnLocalizeNotification),
        //typeof(SpringPanel.OnFinished),
        //typeof(UIDrawCall.OnRenderCallback),
        //typeof(UIDrawCall.OnCreateDrawCall),
        //typeof(UIEventListener.BoolDelegate),
        //typeof(UIEventListener.FloatDelegate),
        //typeof(UIEventListener.KeyCodeDelegate),
        //typeof(UIEventListener.ObjectDelegate),
        //typeof(UIEventListener.VectorDelegate),
        //typeof(UIEventListener.VoidDelegate),
        //typeof(UIGeometry.OnCustomWrite),
        //typeof(UIWidget.OnDimensionsChanged),
        //typeof(UIWidget.OnPostFillCallback),
        //typeof(UIWidget.HitCheck),
        //typeof(SpringPosition.OnFinished),
        //typeof(UICamera.GetKeyStateFunc),
        //typeof(UICamera.GetAxisFunc),
        //typeof(UICamera.GetAnyKeyFunc),
        //typeof(UICamera.GetMouseDelegate),
        //typeof(UICamera.GetTouchDelegate),
        //typeof(UICamera.RemoveTouchDelegate),
        //typeof(UICamera.OnScreenResize),
        //typeof(UICamera.OnCustomInput),
        //typeof(UICamera.OnSchemeChange),
        //typeof(UICamera.MoveDelegate),
        //typeof(UICamera.VoidDelegate),
        //typeof(UICamera.BoolDelegate),
        //typeof(UICamera.FloatDelegate),
        //typeof(UICamera.VectorDelegate),
        //typeof(UICamera.ObjectDelegate),
        //typeof(UICamera.KeyCodeDelegate),
        //typeof(UICamera.GetTouchCountCallback),
        //typeof(UICamera.GetTouchCallback),
        //typeof(UIInput.OnValidate),
        //typeof(UILabel.ModifierFunc),
        //typeof(UIPanel.OnGeometryUpdated),
        //typeof(UIPanel.OnClippingMoved),
        //typeof(UIPanel.OnCreateMaterial),

        // resource
        //typeof(ResourceMgr.LoadCameraPathDelegate),
        //typeof(ResourceMgr.LoadCommonAtlasDelgate),
        //typeof(ResourceMgr.LoadUIDelegate),
        //typeof(ResourceMgr.LoadAtlasDelegate),
        //typeof(ResourceMgr.LoadUIEffectDelegate),
        //typeof(ResourceMgr.LoadImageDelegate),
        //typeof(ResourceMgr.LoadImageAlphaDelegate),
        //typeof(ResourceMgr.LoadBundleDelegate),
        
        // game logic
    };

	//黑名单
	[BlackList]
	public static List<List<string>> BlackList = new List<List<string>>()  {
		// unity
		new List<string>(){"UnityEngine.WWW", "movie"},
		new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
		new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
		new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
		new List<string>(){"UnityEngine.Light", "areaSize"},
		new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
		#if !UNITY_WEBPLAYER
		new List<string>(){"UnityEngine.Application", "ExternalEval"},
		#endif
		new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
		new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
		new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
		new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
		new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
		new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
		new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
		new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
		new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
		new List<string>(){"UnityEngine.UI.Text", "OnRebuildRequested"},

		// NGUI
		new List<string>(){"UIInput", "ProcessEvent", "UnityEngine.Event"},
		new List<string>(){"UIWidget", "showHandles"},
		new List<string>(){"UIWidget", "showHandlesWithMoveTool"},
		new List<string>(){"UIWidget", "OnDrawGizmos"},
	};
}
