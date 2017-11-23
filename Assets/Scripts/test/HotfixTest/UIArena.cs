//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using XLua;

///// <summary>
///// 说明：xlua热更使用范例，必要规则和注意点展示
///// 
///// 注意：
/////     1）标记Hotfix标签的子类CS脚本中“出现”的所有方法和访问器默认可热更，和父类没有任何关系
/////     2）所有可能热更的override方法，如果使用base.xxx调用了基类的实现，最好另外设立接口暴露该调用行为
/////     3）为了降低热更时书写代码工作量，尽量将函数功能细分，复杂的函数体分成多个子函数书写
/////     4）对于泛型方法：
/////			A）慎用一切泛型方法，不带泛型约束的泛型参数的泛型方法如果不能以Lua用另外的方式实现，则没法热更
/////			B）如果需要支持，则在XLuaHelper对泛型方法执行代理后再导出
/////     5）对于所有委托，除了消息系统：
/////			A）要么所有委托对象预先以成员变量的方式缓存---不要直接把函数名当做函数的委托实参使用，GC也高
/////			B）要么所有非匿名委托类型打上[CSharpCallLua]标签，如GameObjectPool.GetGameObjectDelegate
/////			C）要么在所有以委托做参数的函数接口改写为以Delegate传参，以支持委托的反射创建机制---如消息系统
/////			D）如果以上三点没有一点做到，则委托没法在热更中使用
///// 
///// @by wsh 2017-09-07
///// </summary>   

//[Hotfix]
//[LuaCallCSharp]
//public class UIArena : BaseUI
//{
//    private const int RIVAL_COUNT = 5;

//    private UILabel m_titleNameLbl = null;
//    private UILabel m_insLbl = null;
//    private UILabel m_leftTimesDesLbl = null;
//    private UILabel m_leftTimesLbl = null;
//    private UILabel m_winDesLbl = null;
//    private UILabel m_winTimesLbl = null;
//    private UILabel m_awardDesLbl = null;
//    private UILabel m_soldierLbl = null;
//    private UILabel m_shopLbl = null;
//    private UILabel m_resetLbl = null;

//    private UISprite m_barBgSpt = null;
//    private UISprite m_barSpt = null;
//    private UIGrid m_rivalGrid = null;
//    private UIGrid m_awardGrid = null;
//    //private UIPanel m_rivalPanel = null;

//    private GameObject m_myRank = null;
//    private GameObject[] boxArr1 = null;
//    private GameObject[] boxArr2 = null;
//    private GameObject[] boxTipArr = null;
//    private GameObject redPointGo = null;
//    private GameObject[] rankValueType = new GameObject[4];
//    private GameObject shopRedPoint = null;

//    private UserIconItem m_userIconItem = null;
//    private ArenaRivalItem[] m_rivalList = new ArenaRivalItem[RIVAL_COUNT];
//    private List<BagIconItem> m_awardItemList = new List<BagIconItem>(3);
//    private ArenaPanelData m_panelData = null;
     
//    private bool bBackByLineup = false;
//    private Callback<ArenaPanelData> updatePanelInfo;
//    private Callback<List<ArenaRivalData>> updateRivalInfo;
//    private Callback<ArenaPanelData> updateLeftTimes;
//    private Callback<int> setBoxState;
//    private Callback clearData;
//    private GameObjectPool.GetGameObjectDelegate onBagItemLoad;

//    protected override void SetCallback()
//    {
//        // 消息回调：
//        updatePanelInfo = UpdatePanelInfo;
//        updateRivalInfo = UpdateRivalInfo;
//        updateLeftTimes = UpdateLeftTimes;
//        setBoxState = SetBoxState;
//        clearData = ClearData;
//        // 其它回调：
//        onBagItemLoad = OnBagItemLoad;
//    }

//    // 命名说明：以类名做前缀，避免继承树很深时出现命名混乱和冲突
//    protected void UIArena_BaseDoAwake()
//    {
//        base.DoAwake();
//    }

//    protected override void DoAwake()
//    {
//        // 注意点示例：暴露基类调用行为
//        UIArena_BaseDoAwake();

//        // 注意点示例：函数功能细分
//        InitLabel();
//        InitGo();
//        InitText();
//        InitPrefab();
//    }

//    void InitLabel()
//    {
//        m_titleNameLbl = lblDict["titleNameLbl"];
//        m_insLbl = lblDict["insLbl"];
//        m_leftTimesDesLbl = lblDict["leftTimesDesLbl"];
//        m_leftTimesLbl = lblDict["leftTimesLbl"];
//        m_winDesLbl = lblDict["winDesLbl"];
//        m_winTimesLbl = lblDict["winTimesLbl"];
//        m_awardDesLbl = lblDict["awardDesLbl"];
//        m_soldierLbl = lblDict["soldierLbl"];
//        m_shopLbl = lblDict["shopLbl"];
//        m_resetLbl = lblDict["resetLbl"];
//    }

//    void InitGo()
//    {
//        m_barBgSpt = goDict["barBgSpt"].GetComponent<UISprite>();
//        m_barSpt = goDict["barSpt"].GetComponent<UISprite>();
//        m_myRank = goDict["myRank"];
//        m_rivalGrid = goDict["rivalGrid"].GetComponent<UIGrid>();
//        m_awardGrid = goDict["awardGrid"].GetComponent<UIGrid>();
//        //m_rivalPanel = goDict["rivalPanel"].GetComponent<UIPanel>();
//        redPointGo = goDict["redPoint"];
//        shopRedPoint = goDict["shopRedPoint"];
        
//        boxArr1 = new GameObject[3];
//        boxArr2 = new GameObject[3];
//        boxTipArr = new GameObject[3];
//        for (int i = 1; i <= boxArr1.Length; i++)
//        {
//            boxArr1[i - 1] = goDict["box" + i + "Btn"];
//            boxArr2[i - 1] = goDict["box" + i + "Btn2"];
//            boxTipArr[i - 1] = goDict["box" + i + "BtnTip"];
//        }

//        for (int i = 0; i < rankValueType.Length; i++)
//        {
//            rankValueType[i] = goDict["rankValueType" + (i + 1)];
//            rankValueType[i].SetActive(false);
//        }
//    }

//    void InitText()
//    {
//        m_titleNameLbl.text = Language.instance.GetString(300);
//        m_insLbl.text = Language.instance.GetString(324);
//        m_leftTimesDesLbl.text = Language.instance.GetString(301);
//        m_awardDesLbl.text = Language.instance.GetString(302);
//        m_winDesLbl.text = Language.instance.GetString(303);
//        m_soldierLbl.text = Language.instance.GetString(306);
//        m_shopLbl.text = Language.instance.GetString(307);
//        m_resetLbl.text = Language.instance.GetString(308);
//    }

//    void InitPrefab()
//    {
//        GameObject iconPrefab = ResourceMgr.instance.LoadUIPrefab(TheGameIds.PLAYER_ICON_ITEM);
//        if (iconPrefab != null)
//        {
//            GameObject iconGo = NGUITools.AddChild(m_myRank, iconPrefab);
//            if (iconGo != null)
//            {
//                iconGo.transform.localPosition = new Vector3(-128f, 0, 0);
//                iconGo.transform.localScale = Vector3.one;
//                m_userIconItem = NGUITools.AddMissingComponent<UserIconItem>(iconGo);
//            }
//        }

//        GameObject rivalPrefab = ResourceMgr.instance.LoadUIPrefab(TheGameIds.ARENA_RIVAL_ITEM);
//        if (rivalPrefab != null)
//        {
//            for (int i = 0; i < m_rivalList.Length; i++)
//            {
//                GameObject go = NGUITools.AddChild(m_rivalGrid.gameObject, rivalPrefab);
//                if (go != null)
//                {
//                    m_rivalList[i] = go.AddComponent<ArenaRivalItem>();
//                }
//            }
//        }
//    }

//    //private void OnGUI()
//    //{
//    //    if (GUI.Button(new Rect(100, 100, 150, 80), "cs BroadcastMsg1"))
//    //    {
//    //        ArenaPanelData testdata = new ArenaPanelData();
//    //        testdata.rank = 8888;
//    //        Messenger.Broadcast(MessageName.MN_ARENA_PERSONAL_PANEL, testdata);
//    //    }

//    //    if (GUI.Button(new Rect(100, 200, 150, 80), "cs BroadcastMsg2"))
//    //    {
//    //        List<ArenaRivalData> testdata = new List<ArenaRivalData>();
//    //        for (int i = 0; i < 33; i++)
//    //        {
//    //            testdata.Add(new ArenaRivalData());
//    //        }
//    //        Messenger.Broadcast(MessageName.MN_RESET_RIVAL, testdata);
//    //    }
//    //}

//    public void UIArena_BaseAddListener()
//    {
//        base.AddListener();
//    }
//    public override void AddListener()
//    {
//        // 说明：lua侧的使用逻辑参考UIArenaTest.lua
//        UIArena_BaseAddListener();
//        Messenger.AddListener(MessageName.MN_ARENA_PERSONAL_PANEL, updatePanelInfo);
//        Messenger.AddListener(MessageName.MN_RESET_RIVAL, updateRivalInfo);
//        Messenger.AddListener(MessageName.MN_ARENA_UPDATE, updateLeftTimes);
//        Messenger.AddListener(MessageName.MN_ARENA_BOX, setBoxState);
//        Messenger.AddListener(MessageName.MN_ARENA_CLEARDATA, clearData);
//    }

//    public void UIArena_BaseRemoveListener()
//    {
//        base.RemoveListener();
//    }
//    public override void RemoveListener()
//    {
//        UIArena_BaseRemoveListener();
//        Messenger.RemoveListener(MessageName.MN_ARENA_PERSONAL_PANEL, updatePanelInfo);
//        Messenger.RemoveListener(MessageName.MN_RESET_RIVAL, updateRivalInfo);
//        Messenger.RemoveListener(MessageName.MN_ARENA_UPDATE, updateLeftTimes);
//        Messenger.RemoveListener(MessageName.MN_ARENA_BOX, setBoxState);
//        Messenger.RemoveListener(MessageName.MN_ARENA_CLEARDATA, clearData);
//    }

//    public void UIArena_BaseOpen(object param, UIPathData pathData)
//    {
//        base.Open(param, pathData);
//    }
//    public override void Open(object param, UIPathData pathData)
//    {
//        UIArena_BaseOpen(param, pathData);
//        if (param != null)
//        {
//            bBackByLineup = (bool)param;
//        }

//        if(bBackByLineup == false)
//        {
//            Player.instance.arenaMgr.ReqArenaData();
//        }
//        Init();
//    }

//    public void Init()
//    { 
//    }

//    public void UIArena_BaseClose()
//    {
//        base.Close();
//    }
//    public override void Close()
//    {
//        // 说明：函数细分 + 暴露基类实现
//        ClearArenaRivalItem();
//        ClearAwardItemList();
//        ClearData();
//        UIArena_BaseClose();
//    }

//    void ClearArenaRivalItem()
//    {
//        for (int i = 0; i < m_rivalList.Length; i++)
//        {
//            ArenaRivalItem rival = m_rivalList[i];
//            if (rival == null)
//            {
//                continue;
//            }

//            rival.Release();
//        }
//    }

//    void ClearAwardItemList()
//    {
//        for (int i = 0; i < m_awardItemList.Count; i++)
//        {
//            BagIconItem item = m_awardItemList[i];
//            if (item == null)
//            {
//                continue;
//            }
//            UIGameObjectPool.instance.RecycleGameObject(ResourceMgr.RESTYPE.UI, item.objThis, TheGameIds.UI_BAG_ITEM_ICON);
//        }
//        m_awardItemList.Clear();
//    }

//    protected void UIArena_BaseNguiOnClick(GameObject go)
//    {
//        base.NguiOnClick(go);
//    }
//    protected override void NguiOnClick(GameObject go)
//    {
//        UIArena_BaseNguiOnClick(go);
//        switch (go.name)
//        {
//            case "leftTimeBtn":
//                ClickOnLeftTimeBtn();
//                break;
//            case "soldierBtn":
//                ClickOnSoldierBtn();
//                break;
//            case "shopBtn":
//                ClickOnShopBtn();
//                break;
//            case "resetBtn":
//                ClickOnResetBtn();
//                break;
//            case "box1Btn":
//                ClickOnBox1Btn();
//                break;
//            case "box2Btn":
//                ClickOnBox2Btn();
//                break;
//            case "box3Btn":
//                ClickOnBox3Btn();
//                break;
//        }
//    }

//    void ClickOnLeftTimeBtn()
//    {
//        Utils.FloatAlert(Language.instance.GetString(325, "{0}", m_panelData.timesLimit - m_panelData.todayChallengeTimes));
//    }

//    void ClickOnSoldierBtn()
//    {
//        bBackByLineup = true;
//        UIManager.Instance().ShowUI(TheUIPrefabIds.UI_LINEUP_MAIN, null, UIOpenMode.APPEND);
//    }

//    void ClickOnShopBtn()
//    {
//        Player.instance.shopMgr.isHaveOpenArenaShop = true;
//        UpdateReddot();
//        Player.instance.UserMgr.SetRedPointState(TheSysIds.ARENA, Player.instance.arenaMgr.isShowBoxRedPoint || Player.instance.arenaMgr.isShowTimesRedPoint);
//        UIManager.Instance().ShowUI(TheUIPrefabIds.UI_ARENA_SHOP);
//    }

//    void ClickOnResetBtn()
//    {
//        Player.instance.arenaMgr.ReqResetRival();
//    }

//    void ClickOnBox1Btn()
//    {
//        if (m_panelData.todayChallengeTimes < m_panelData.awardList[0].openCondition)
//        {
//            AwardArenaBoxInfo info = DictUtils.GetAwardArenaBoxInfoById(1);
//            if (info != null)
//            {
//                TakeAward(info.GetItemList(), 1);
//            }
//        }
//        else
//        {
//            Player.instance.arenaMgr.ReqTakeAward(1);
//        }
//    }

//    void ClickOnBox2Btn()
//    {
//        if (m_panelData.todayChallengeTimes < m_panelData.awardList[1].openCondition)
//        {
//            AwardArenaBoxInfo info = DictUtils.GetAwardArenaBoxInfoById(2);
//            if (info != null)
//            {
//                TakeAward(info.GetItemList(), 2);
//            }
//        }
//        else
//        {
//            Player.instance.arenaMgr.ReqTakeAward(2);
//        }
//    }

//    void ClickOnBox3Btn()
//    {
//        if (m_panelData.todayChallengeTimes < m_panelData.awardList[2].openCondition)
//        {
//            AwardArenaBoxInfo info = DictUtils.GetAwardArenaBoxInfoById(3);
//            if (info != null)
//            {
//                TakeAward(info.GetItemList(), 3);
//            }
//        }
//        else
//        {
//            Player.instance.arenaMgr.ReqTakeAward(3);
//        }
//    }

//    public void UpdatePanelInfo(ArenaPanelData panelData)
//    {
//        Logger.Log("------------------UpdatePanelInfo called in cs, rank = " + panelData.rank);
//        if (panelData == null || panelData.awardList == null)
//        {
//            return;
//        }
//        m_panelData = panelData;
//        UpdateReddot();
        
//        m_winTimesLbl.text = panelData.winTimes.ToString();
//        m_leftTimesLbl.text = (panelData.timesLimit - panelData.todayChallengeTimes).ToString();
//        int maxNum = panelData.awardList[panelData.awardList.Count - 1].openCondition;
//        float rate = panelData.todayChallengeTimes * 1.0f / maxNum;
//        int barLength = (int)(m_barBgSpt.width * (rate > 1 ? 1 : rate));
//        if (barLength == 0)
//        {
//            m_barSpt.gameObject.SetActive(false);
//        }
//        else
//        {
//            m_barSpt.gameObject.SetActive(true);
//            m_barSpt.width = barLength;
//        }

//        ShowRankValueType(panelData.rank);
//        if (m_userIconItem != null)
//        {
//            m_userIconItem.SetIcon(Player.instance.UserMgr.userData.icon);
//        }

//        UpdateBoxState(panelData.awardList, panelData);
//        UpdateDailyAwardItem(panelData.dailyItemList);
//        UpdateRivalInfo(panelData.rivalList);
//        Invoke("RefreshPanel", 0.1f);
//    }

//    public void UpdateLeftTimes(ArenaPanelData panelData)
//    {
//        Logger.Log("------------------UpdateLeftTimes called in cs, rank = " + panelData.rank);
//        m_panelData.timesLimit = panelData.timesLimit;
//        m_panelData.todayChallengeTimes = panelData.todayChallengeTimes;
//        m_panelData.remain_buy_arena_times = panelData.remain_buy_arena_times;
//        m_leftTimesLbl.text = (panelData.timesLimit - panelData.todayChallengeTimes).ToString();

//        UpdateReddot();
//    }

//    private void UpdateDailyAwardItem(List<BagItemData> itemList)
//    {
//        if (itemList == null)
//        {
//            return;
//        }

//        for (int i = 0; i < itemList.Count; i++)
//        {
//            UIGameObjectPool.instance.GetGameObject(ResourceMgr.RESTYPE.UI, TheGameIds.UI_BAG_ITEM_ICON, new GameObjectPool.CallbackInfo(onBagItemLoad, itemList[i], Vector3.zero, Vector3.one * 0.65f, m_awardGrid.gameObject));
//        }
//        m_awardGrid.Reposition();
//    }

//    public void OnBagItemLoad(GameObject go, object param1)
//    {
//        Logger.Log("------------------OnBagItemLoad called in cs, go = " + go);
//        if (go == null)
//        {
//            return;
//        }

//        BagIconItem item = NGUITools.AddMissingComponent<BagIconItem>(go);
//        if (item != null)
//        {
//            item.SetData((BagItemData)param1, true, false);
//            m_awardItemList.Add(item);
//        }
//    }

//    public static void OnBagItemLoad2(GameObject go, object param1)
//    {
//        Logger.Log("------------------static OnBagItemLoad2 called in cs, go = " + go);
//    }

//    private void UpdateRivalInfo(List<ArenaRivalData> rivalList)
//    {
//        Logger.Log("------------------UpdateRivalInfo called in cs, count = " + ((rivalList == null) ? "null" : rivalList.Count.ToString()));
//        if (rivalList == null)
//        {
//            return;
//        }

//        for (int i = 0; i < rivalList.Count; i++)
//        {
//            ArenaRivalData rivalData = rivalList[i];
//            if (rivalData == null)
//            {
//                continue;
//            }

//            if (i >= m_rivalList.Length)
//            {
//                continue;
//            }
//            m_rivalList[i].SetData(rivalData, m_panelData);
//        }
//        m_rivalGrid.Reposition();
//    }

//    private void UpdateBoxState(List<ArenaAwardData> awardList, ArenaPanelData data)
//    {
//        if (awardList == null)
//        {
//            return;
//        }

//        for (int i = 0; i < awardList.Count; i++)
//        {
//            ArenaAwardData awardData = awardList[i];
//            if (awardData == null)
//            {
//                continue;
//            }

//            bool canOpen = data.todayChallengeTimes >= awardData.openCondition;
//            boxTipArr[i].SetActive(canOpen);

//            bool isOpen = awardData.isOpen == 1;
//            boxArr1[i].SetActive(!isOpen);
//            boxArr2[i].SetActive(isOpen);
//        }
//    }

//    private void UpdateReddot() 
//    {
//        shopRedPoint.SetActive(!Player.instance.shopMgr.isHaveOpenArenaShop);
//        redPointGo.SetActive(m_panelData.timesLimit > m_panelData.todayChallengeTimes);
//    }

//    public void SetBoxState(int id)
//    {
//        Logger.Log("------------------SetBoxState called in cs, id = " + id);
//        boxArr1[id - 1].SetActive(false);
//        boxArr2[id - 1].SetActive(true);
//    }

//    private void TakeAward(List<BagItemData> itemList,int id)
//    {
//        UIManager.Instance().ShowUI(TheUIPrefabIds.UI_ARENA_AWARD, new object[]{ id - 1, itemList });
//    }

//    protected override bool showBlurBG()
//    {
//        return true;
//    }

//    private void RefreshPanel()
//    {
//        m_rivalGrid.Reposition();
//    }

//    private void ShowRankValueType(int num)
//    {
//        if (num <= 0)
//        {
//            rankValueType[0].SetActive(true);
//            for (int i = 1; i < rankValueType.Length; i++)
//            {
//                rankValueType[i].SetActive(false);
//            }
//            rankValueType[0].GetComponentInChildren<UISprite>().spriteName = "sz (0)";
//            return;
//        }

//        int count = 0;

//        //add by yy, Only for test, 排名 1W以上均为 9999 名
//        if (num > 9999)
//        {
//            num = 9999;
//        }
//        int tempNum = num;

//        while (tempNum > 0)
//        {
//            tempNum /= 10;
//            count++;
//        }

//        rankValueType[count - 1].SetActive(true);
//        for (int i = 0; i < rankValueType.Length; i++)
//        {
//            if (i != count - 1)
//            {
//                rankValueType[i].SetActive(false);
//            }
//        }

//        Utils.SetRankValue(rankValueType[count - 1].GetComponentsInChildren<UISprite>(true), num, "sz ");
//    }

//    private void ShowRuleUI()
//    {
//        MessageUIWenHaoTipsData tipData = new MessageUIWenHaoTipsData();
//        tipData.title = Language.instance.GetString(312);
//        tipData.msg = Language.instance.GetString(313);
//        UIManager.Instance().ShowUI(TheUIPrefabIds.UI_WEN_TIPS, tipData);
//    }

    
//    public override object GetRecoverParam()
//    {
//        return bBackByLineup;
//    }

//    public void ClearData()
//    {
//        Logger.Log("------------------ClearData called in cs");
//        bBackByLineup = false;
//    }
//}