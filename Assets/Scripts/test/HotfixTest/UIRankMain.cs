//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using XLua;

//[Hotfix]
//public class UIRankMain : BaseUI
//{
//    private GameObject[] big_tabList = new GameObject[2];
//    private GameObject[] big_tabList2 = new GameObject[2];

//    private GameObject ownTabState = null;
//    private GameObject guildTabState = null;

//    private GameObject[] tabList = new GameObject[7];
//    private GameObject[] tabList2 = new GameObject[7];

//    private UILabel insLbl = null;

//    private const int SHOW_COUNT = 8;
//    private UIGrid itemGrid = null;
//    private UIWrapContent wrapContent;
//    private List<Transform> tempTransList = new List<Transform>();

//    private GameObject playerItem = null;
//    private GameObject haveGuildState = null;
//    private GameObject noGuildState = null;
//    private UILabel noGuildLbl = null;
//    private GameObject[] rankValueType = new GameObject[4];
//    private UISprite playerSp = null;
//    private UISprite flagSp = null;
//    private UILabel playerNameLbl = null;
//    private UILabel advanceLbl = null;
//    private UILabel advanceValueLbl = null;
//    private UISprite levelSp = null;

//    private UserIconItem m_iconItem = null;
//    private int big_currTab = 0;
//    private int currTab = 0;

//    public List<RankData> m_rankDataList = new List<RankData>();
//    public List<RankGuildData> m_rankGuildDataList = new List<RankGuildData>();

//    protected override void SetCallback()
//    {
//        // 说明：NGUI的委托全部导出，这里可以不缓存
//        wrapContent.onInitializeItem = UpdateWrapItem;
//    }

//    protected void UIRankMain_BaseDoAwake()
//    {
//        base.DoAwake();
//    }
//    protected override void DoAwake()
//    {
//        UIRankMain_BaseDoAwake();

//        InitCmp();
//        InitGo();
//        InitLb();
//        InitArr();
//    }

//    void InitLb()
//    {
//        playerNameLbl = lblDict["playerNameLbl"];
//        advanceLbl = lblDict["advanceLbl"];
//        advanceValueLbl = lblDict["advanceValueLbl"];
//        noGuildLbl = lblDict["noGuildLbl"];
//        insLbl = lblDict["insLbl"];
//    }

//    void InitGo()
//    {
//        ownTabState = goDict["ownTabState"];
//        guildTabState = goDict["guildTabState"];
//        playerItem = goDict["playerItem"];
//        playerItem.SetActive(false);

//        haveGuildState = goDict["haveGuildState"];
//        noGuildState = goDict["noGuildState"];
//    }

//    void InitCmp()
//    {
//        wrapContent = goDict["itemGrid"].GetComponent<UIWrapContent>();
//        itemGrid = goDict["itemGrid"].GetComponent<UIGrid>();
//        flagSp = goDict["flagSp"].GetComponent<UISprite>();
//        levelSp = goDict["levelSp"].GetComponent<UISprite>();
//        playerSp = goDict["playerSp"].GetComponent<UISprite>();
//    }

//    void InitArr()
//    {
//        for (int i = 0; i < big_tabList.Length; i++)
//        {
//            big_tabList[i] = goDict["BigLuaTab" + i + "1"];
//            big_tabList2[i] = goDict["BigLuaTab" + i + "2"];
//            big_tabList[i].SetActive(false);
//        }

//        for (int i = 0; i < tabList.Length; i++)
//        {
//            tabList[i] = goDict["LuaTab" + i + "1"];
//            tabList2[i] = goDict["LuaTab" + i + "2"];
//            tabList[i].SetActive(false);
//        }

//        for (int i = 0; i < rankValueType.Length; i++)
//        {
//            rankValueType[i] = goDict["rankValueType" + (i + 1)];
//            rankValueType[i].SetActive(false);
//        }
//    }

//    public void UIRankMain_BaseAddListener()
//    {
//        base.AddListener();
//    }

//    public override void AddListener()
//    {
//        UIRankMain_BaseAddListener();
//        // 说明：消息系统支持反射，可以不缓存委托
//        Messenger.AddListener<int>(MessageName.MN_MY_RANKING_LIST_REQ, UpdateOwnList);
//        Messenger.AddListener<int>(MessageName.MN_GUILD_RANKING_LIST_REQ, UpdateGuildList);
//    }

//    public void UIRankMain_BaseRemoveListener()
//    {
//        base.RemoveListener();
//    }

//    public override void RemoveListener()
//    {
//        UIRankMain_BaseRemoveListener();
//        Messenger.RemoveListener<int>(MessageName.MN_MY_RANKING_LIST_REQ, UpdateOwnList);
//        Messenger.RemoveListener<int>(MessageName.MN_GUILD_RANKING_LIST_REQ, UpdateGuildList);
//    }

//    public void UIRankMain_BaseOpen(object param, UIPathData pathData)
//    {
//        base.Open(param, pathData);
//    }

//    public override void Open(object param, UIPathData pathData)
//    {
//        UIRankMain_BaseOpen(param, pathData);

//        InitLbl();
//        OnBigTabChange(big_currTab);
//        OnTabChange(currTab);
//        InstantiatePrefab();
//        Player.instance.rankMgr.ReqRankingList(currTab);
//        StartCoroutine(TestCorotine(3));
//    }

//    IEnumerator TestCorotine(int sec)
//    {
//        yield return new WaitForSeconds(sec);
//        Logger.Log(string.Format("This message appears after {0} seconds in cs!", sec));
//        yield break;
//    }


//    public void UIRankMain_BaseClose()
//    {
//        base.Close();
//    }

//    public override void Close()
//    {
//        for (int i = 0; i < tempTransList.Count; i++)
//        {
//            tempTransList[i].GetComponent<UIRankItem>().Release();
//        }
//        Relese();

//        UIManager.Instance().HideTakeOrder();
//        currTab = 0;
//        big_currTab = 0;
//        tempTransList.Clear();
//        Utils.CleanGrid(itemGrid.gameObject);

//        UIRankMain_BaseClose();
//    }

//    private void Relese()
//    {
//        if (m_iconItem != null)
//        {
//            UIGameObjectPool.instance.RecycleGameObject(ResourceMgr.RESTYPE.UI, m_iconItem.objThis, TheGameIds.PLAYER_ICON_ITEM);
//            m_iconItem = null;
//        }
//    }

//    private void InitLbl()
//    {
//        string[] big_tabListStr = Language.instance.GetString(2500).Split(',');
//        for (int i = 0; i < big_tabList.Length; i++)
//        {
//            if (big_tabListStr.Length - 1 < i)
//            {
//                return;
//            }
//            big_tabList[i].GetComponentInChildren<UILabel>(true).text = big_tabList2[i].GetComponentInChildren<UILabel>(true).text = big_tabListStr[i];
//        }

//        string[] tabListStr = Language.instance.GetString(2501).Split(',');
//        for (int i = 0; i < tabList.Length; i++)
//        {
//            if (tabListStr.Length - 1 < i)
//            {
//                return;
//            }
//            tabList[i].GetComponentInChildren<UILabel>(true).text = tabList2[i].GetComponentInChildren<UILabel>(true).text = tabListStr[i];
//        }

//        noGuildLbl.text = Language.instance.GetString(2505);
//    }

//    private void OnBigTabChange(int tab)
//    {
//        big_tabList[tab].SetActive(true);
//        big_tabList2[tab].SetActive(false);

//        for (int i = 0; i < big_tabList.Length; i++)
//        {
//            if (i != tab)
//            {
//                big_tabList[i].SetActive(false);
//                big_tabList2[i].SetActive(true);
//            }
//        }

//        big_currTab = tab;
//        if (big_currTab == 0)
//        {
//            guildTabState.SetActive(false);
//            ownTabState.SetActive(true);
//            OnTabChange((int)RankType.MY_BUILDING);
//        }
//        else
//        {
//            ownTabState.SetActive(false);
//            guildTabState.SetActive(true);
//            OnTabChange((int)RankType.GUILD_BUILDING);
//        }
//    }

//    private void OnTabChange(int tab)
//    {
//        tabList[tab].SetActive(true);
//        tabList2[tab].SetActive(false);

//        for (int i = 0; i < tabList.Length; i++)
//        {
//            if (i != tab)
//            {
//                tabList[i].SetActive(false);
//                tabList2[i].SetActive(true);
//            }
//        }

//        string[] tempStrArr = Language.instance.GetString(2502).Split(',');
//        insLbl.text = tempStrArr[tab];
//        currTab = tab;
//    }

//    private void InstantiatePrefab()
//    {
//        if (tempTransList.Count == 0)
//        {
//            GameObject itemPreafab = ResourceMgr.instance.LoadUIPrefab(TheGameIds.UI_RANK_ITEM);
//            for (int i = 0; i < SHOW_COUNT; i++)
//            {
//                GameObject go = NGUITools.AddChild(itemGrid.gameObject, itemPreafab);
//                if (go != null)
//                {
//                    go.AddMissingComponent<UIRankItem>();
//                    tempTransList.Add(go.transform);
//                    go.SetActive(false);
//                }
//            }
//        }
//    }

//    private void UpdateOwnList(int type)
//    {
//        noGuildState.SetActive(false);
//        haveGuildState.SetActive(true);
//        RankData rank_data = null;
//        if (!Player.instance.rankMgr.rankDataDict.TryGetValue(type, out rank_data))
//        {
//            return;
//        }
//        Relese();

//        playerItem.SetActive(true);
//        ShowRankValueType(rank_data.rank);

//        flagSp.gameObject.SetActive(false);
//        playerSp.gameObject.SetActive(false);

//        UIGameObjectPool.instance.GetGameObject(ResourceMgr.RESTYPE.UI, TheGameIds.PLAYER_ICON_ITEM, new GameObjectPool.CallbackInfo(OnIconLoad, rank_data.user_brief_info.use_icon, new Vector3(-110f, 0, 0), new Vector3(0.9f, 0.9f, 0.9f), haveGuildState));

//        playerNameLbl.text = rank_data.user_brief_info.name;
//        string[] strArr = Language.instance.GetString(2506).Split(',');
//        advanceLbl.text = strArr[currTab % 4];
//        advanceValueLbl.text = rank_data.score.ToString();

//        if (!Player.instance.rankMgr.rankDataListDict.TryGetValue(type, out m_rankDataList))
//        {
//            return;
//        }
//        itemGrid.Reposition();

//        int count = m_rankDataList.Count;
//        wrapContent.InitChildren(tempTransList, count);
//        wrapContent.RestToBeginning();
//    }

//    private void UpdateGuildList(int type)
//    {
//        Relese();

//        RankGuildData rank_guild_data = null;
//        playerItem.SetActive(true);
//        if (!Player.instance.rankMgr.guildRankDataDict.TryGetValue(type, out rank_guild_data))
//        {
//            noGuildState.SetActive(true);
//            haveGuildState.SetActive(false);
//        }
//        else
//        {
//            noGuildState.SetActive(false);
//            haveGuildState.SetActive(true);

//            ShowRankValueType(rank_guild_data.rank);

//            flagSp.gameObject.SetActive(true);
//            playerSp.gameObject.SetActive(true);
//            playerNameLbl.text = rank_guild_data.guild_brief_info.name;
//            Utils.SetGuildFlagAndIcon(rank_guild_data.guild_brief_info.icon, rank_guild_data.guild_brief_info.flag_icon, playerSp, flagSp, false);
//            string[] strArr = Language.instance.GetString(2506).Split(',');
//            advanceLbl.text = strArr[currTab % 4];
//            advanceValueLbl.text = rank_guild_data.score.ToString();
//        }

//        if (!Player.instance.rankMgr.guildRankDataListDict.TryGetValue(type, out m_rankGuildDataList))
//        {
//            return;
//        }
//        itemGrid.Reposition();
        
//        int count = m_rankGuildDataList.Count;
//        wrapContent.InitChildren(tempTransList, count);
//        wrapContent.RestToBeginning();
//    }

//    private void UpdateWrapItem(GameObject go, int wrapIndex, int realIndex)
//    {
//        if (big_currTab == 0)
//        {
//            if (realIndex > -1 && realIndex < m_rankDataList.Count)
//            {
//                RankData data = m_rankDataList[realIndex];
//                updateRankItem(data, go);
//            }
//        }
//        else
//        {
//            if (realIndex > -1 && realIndex < m_rankGuildDataList.Count)
//            {
//                RankGuildData data = m_rankGuildDataList[realIndex];
//                updateRankItem(data, go);
//            }
//        }
//    }

//    private void updateRankItem(object data, GameObject go)
//    {
//        if (data != null)
//        {
//            go.AddMissingComponent<UIRankItem>().SetData(data, currTab % 4);
//        }
//    }

//    protected void UIRankMain_BaseNguiOnClick(GameObject go)
//    {
//        base.NguiOnClick(go);
//    }

//    protected override void NguiOnClick(GameObject go)
//    {
//        if (go.name.Contains("bigTab"))
//        {
//            int tab;
//            if (int.TryParse(go.name.Substring(6, 1), out tab))
//            {
//                if (tab != big_currTab)
//                {
//                    OnBigTabChange(tab);
//                    Player.instance.rankMgr.ReqRankingList(big_currTab * 4);
//                }
//            }
//        }
//        else if(go.name.Contains("tab"))
//        {
//            int tab;
//            if (int.TryParse(go.name.Substring(3, 1), out tab))
//            {
//                if (tab != currTab)
//                {
//                    OnTabChange(tab);
//                    Player.instance.rankMgr.ReqRankingList(currTab);
//                }
//            }
//        }
//        UIRankMain_BaseNguiOnClick(go);
//    }

//    public void OnIconLoad(GameObject go, object param1)
//    {
//        if (go == null)
//        {
//            return;
//        }
//        int id = (int)param1;

//        m_iconItem = NGUITools.AddMissingComponent<UserIconItem>(go);
//        m_iconItem.SetIcon(id);
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
//            levelSp.gameObject.SetActive(false);
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

//        Utils.SetRankValue(rankValueType[count - 1].GetComponentsInChildren<UISprite>(true), num, "sz ", levelSp, "phb ");
//    }
//}
