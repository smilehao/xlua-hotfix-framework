//using UnityEngine;
//using System.Collections.Generic;
//using modules;
//using Battle;
//using Role;
//using XLua;

//[Hotfix]
//public class UIBattleShop : BaseUI
//{
//    protected UIWrapContent m_wrapContent = null;
//    protected List<Transform> uiTransList = new List<Transform>(15);
//    protected List<UIShopCardItem> uiCardList = new List<UIShopCardItem>(15);
//    protected List<UICardData> dataCardList = new List<UICardData>();
//    protected List<UICardData> dataAllCardList = new List<UICardData>();
//    protected List<int> selCardList = new List<int>(10);

//    protected Role.Kingdom m_kingdom = Role.Kingdom.NONE;
//    protected int m_requireLevel = 1;
//    protected UIScrollView mScrollView = null;
//    protected UIBtnGroup m_requireLevelBtnGroup = null;

//    protected int m_requireCoin = 0;
//    protected int m_upgradeCoin = 0;
//    protected UILabel m_requireCoinLb = null;
//    protected Transform m_toolTipAnchorPoint;

//    protected UIShopCardItem m_curGetCard;
//    protected int m_curRequireCoin;
//    protected GameObject m_effectRoot;
//    protected GameObject m_resCostRoot;
//    protected UseGainResNode m_resNode;
//    protected int m_nInputLimit = UIInputLimit.ALL_OK;
//    protected int[] m_nUsableOnly = null;
//    protected GameObject m_upgradeRoot;
//    protected UILabel m_upgradeCoinLb = null;
//    protected UILabel m_upgradeInfoLb = null;
//    protected UISprite m_upgradeSpr = null;

//    protected override void DoAwake()
//    {
//        base.DoAwake();

//        InitGo();

//        AddListener();
//    }

//    private void AddListener()
//    {
//        Messenger.AddListener<int, ChangeReason>(MessageName.MN_BATTLE_SELF_COIN_CHG, SelfCoinChg);
//        Messenger.AddListener<Charactor>(MessageName.MN_BUILD_UP_LEVEL, UpdateBuildUpLevel);
//        Messenger.AddListener(MessageName.MN_ON_SHOP_GET_CARD, DoGetCard);
//        Messenger.AddListener(MessageName.MN_ON_UPDATE_PLAYER_CARD, DoUpdateCard);
//        Messenger.AddListener<int>(MessageName.MN_UI_INPUT_LIMIT, SetUIInputLimit);
//        Messenger.AddListener<bool, int>(MessageName.MN_SHOW_GUIDE_EFFECT, ShowGuideEffect);
//        Messenger.AddListener<int[]>(MessageName.MN_BATTLE_SHOP_TO_SET_USABLE_ONLY, ToSetUsableOnly);
//        Messenger.AddListener<int>(MessageName.MN_CHARACTOR_DIE, OnSBDie);
//    }

//    protected override void DoOnDestroy()
//    {
//        RemoveListener();

//        base.DoOnDestroy();
//    }

//    private void RemoveListener()
//    {
//        Messenger.RemoveListener<int, ChangeReason>(MessageName.MN_BATTLE_SELF_COIN_CHG, SelfCoinChg);
//        Messenger.RemoveListener<Charactor>(MessageName.MN_BUILD_UP_LEVEL, UpdateBuildUpLevel);
//        Messenger.RemoveListener(MessageName.MN_ON_SHOP_GET_CARD, DoGetCard);
//        Messenger.RemoveListener(MessageName.MN_ON_UPDATE_PLAYER_CARD, DoUpdateCard);
//        Messenger.RemoveListener<int>(MessageName.MN_UI_INPUT_LIMIT, SetUIInputLimit);
//        Messenger.RemoveListener<bool, int>(MessageName.MN_SHOW_GUIDE_EFFECT, ShowGuideEffect);
//        Messenger.RemoveListener<int[]>(MessageName.MN_BATTLE_SHOP_TO_SET_USABLE_ONLY, ToSetUsableOnly);
//        Messenger.RemoveListener<int>(MessageName.MN_CHARACTOR_DIE, OnSBDie);
//    }

//    protected void ShowGuideEffect(bool bShow, int nIndex)
//    {
//        switch (nIndex)
//        {
//            case 1:
//                {
//                    if (bShow)
//                    {
//                        UIShopCardItem uiShopCardItem = GetUICardByID(_Sequence.HumanPlot0.TO_BUY_CARD_0);
//                        if (uiShopCardItem != null)
//                        {
//                            UIEffectMgr.UIEffectData uiEffectData = new UIEffectMgr.UIEffectData();
//                            uiEffectData.goParent = uiShopCardItem.gameObject;
//                            uiEffectData.vPos = Vector3.zero;
//                            uiEffectData.vRot = Vector3.zero;
//                            uiEffectData.vScale = Vector3.one;
//                            uiEffectData.nIndex = 0;
//                            CreateUIEffect(TheGameIds.UI_GUIDE, uiEffectData);
//                        }
//                    }
//                    else
//                    {
//                        SetUIEffectActive_Asyn(TheGameIds.UI_GUIDE, 0, false);
//                    }
//                }
//                break;
//            case 2:
//                {
//                    if (bShow)
//                    {
//                        UIShopCardItem uiShopCardItem = GetUICardByID(2010);//用 GetUICardByID(_Sequence.HumanPlot0.TO_BUY_CARD_1) 会因为更新延时存在出错的风险
//                        if (uiShopCardItem != null)
//                        {
//                            UIEffectMgr.UIEffectData uiEffectData = new UIEffectMgr.UIEffectData();
//                            uiEffectData.goParent = uiShopCardItem.gameObject;
//                            uiEffectData.vPos = Vector3.zero;
//                            uiEffectData.vRot = Vector3.zero;
//                            uiEffectData.vScale = Vector3.one;
//                            uiEffectData.nIndex = 0;
//                            CreateUIEffect(TheGameIds.UI_GUIDE, uiEffectData);
//                        }
//                    }
//                    else
//                    {
//                        SetUIEffectActive_Asyn(TheGameIds.UI_GUIDE, 0, false);
//                    }
//                }
//                break;
//            case 4:
//                {
//                    if (bShow)
//                    {
//                        UIEffectMgr.UIEffectData uiEffectData = new UIEffectMgr.UIEffectData();
//                        uiEffectData.goParent = m_upgradeSpr.gameObject;
//                        uiEffectData.vPos = Vector3.zero;
//                        uiEffectData.vRot = Vector3.zero;
//                        uiEffectData.vScale = Vector3.one;
//                        uiEffectData.nIndex = 0;
//                        CreateUIEffect(TheGameIds.UI_GUIDE, uiEffectData);
//                    }
//                    else
//                    {
//                        SetUIEffectActive_Asyn(TheGameIds.UI_GUIDE, 0, false);
//                    }
//                }
//                break;
//            case 5:
//                {
//                    if (bShow)
//                    {
//                        UIShopCardItem uiShopCardItem = GetUICardByID(_Sequence.HumanPlot0.TO_BUY_CARD_0);
//                        if (uiShopCardItem != null)
//                        {
//                            Transform trSelBtn = uiShopCardItem.GetTrSelBtn();
//                            if (trSelBtn != null)
//                            {
//                                UIEffectMgr.UIEffectData uiEffectData = new UIEffectMgr.UIEffectData();
//                                uiEffectData.goParent = trSelBtn.gameObject;
//                                uiEffectData.vPos = Vector3.zero;
//                                uiEffectData.vRot = Vector3.zero;
//                                uiEffectData.vScale = Vector3.one;
//                                uiEffectData.nIndex = 0;
//                                CreateUIEffect(TheGameIds.UI_GUIDE, uiEffectData);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        SetUIEffectActive_Asyn(TheGameIds.UI_GUIDE, 0, false);
//                    }
//                }
//                break;
//            case 7:
//                {
//                    if (bShow)
//                    {
//                        UIEffectMgr.UIEffectData uiEffectData = new UIEffectMgr.UIEffectData();
//                        uiEffectData.goParent = m_requireCoinLb.gameObject;
//                        uiEffectData.vPos = new Vector3(0f, 15f, 0f);
//                        uiEffectData.vRot = Vector3.zero;
//                        uiEffectData.vScale = Vector3.one;
//                        uiEffectData.nIndex = 0;
//                        CreateUIEffect(TheGameIds.UI_GUIDE_CUBE, uiEffectData);
//                    }
//                    else
//                    {
//                        SetUIEffectActive_Asyn(TheGameIds.UI_GUIDE_CUBE, 0, false);
//                    }
//                }
//                break;
//            case 8:
//                {
//                    if (bShow)
//                    {
//                        UIShopCardItem uiShopCardItem = GetUICardByID(_Sequence.HumanPlot0.TO_BUY_CARD_1);
//                        if (uiShopCardItem != null)
//                        {
//                            Transform trSelBtn = uiShopCardItem.GetTrSelBtn();
//                            if (trSelBtn != null)
//                            {
//                                UIEffectMgr.UIEffectData uiEffectData = new UIEffectMgr.UIEffectData();
//                                uiEffectData.goParent = trSelBtn.gameObject;
//                                uiEffectData.vPos = Vector3.zero;
//                                uiEffectData.vRot = Vector3.zero;
//                                uiEffectData.vScale = Vector3.one;
//                                uiEffectData.nIndex = 0;
//                                CreateUIEffect(TheGameIds.UI_GUIDE, uiEffectData);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        SetUIEffectActive_Asyn(TheGameIds.UI_GUIDE, 0, false);
//                    }
//                }
//                break;
//            default:
//                break;
//        }
//    }

//    private void SetUIInputLimit(int nInputLimit)
//    {
//        m_nInputLimit = nInputLimit;
//    }

//    private void InitGo()
//    {
//        m_wrapContent = goDict["WrapContent"].GetComponent<UIWrapContent>();
//        m_effectRoot = goDict["EffectRoot"];
//        m_resCostRoot = goDict["ResCostRoot"];
//        m_upgradeRoot = goDict["UpgradeMaincityRoot"];

//        InitCardGo(m_wrapContent.transform);
//        m_wrapContent.onInitializeItem = OnInitializeItem;
        
//        Transform trans = goDict["ScrollView"].transform;
//        if (trans != null)
//        {
//            mScrollView = trans.GetComponent<UIScrollView>();
//        }

//        m_requireLevelBtnGroup = goDict["RequireLevelRoot"].AddMissingComponent<UIBtnGroup>();
//        if (m_requireLevelBtnGroup != null)
//        {
//            m_requireLevelBtnGroup.Original = goDict["Level_01_Btn"];
//            m_requireLevelBtnGroup.onClick = ClickOnRequireLevelBtnGroup;
//            m_requireLevelBtnGroup.CanClickOnDisable = true;
//            m_requireLevelBtnGroup.SetBindData("Level_01_Btn", 1);
//            m_requireLevelBtnGroup.SetBindData("Level_02_Btn", 2);
//            m_requireLevelBtnGroup.SetBindData("Level_03_Btn", 3);
//            m_requireLevelBtnGroup.SetBindData("Level_04_Btn", 4);
//        }

//        m_requireCoinLb = lblDict["RequireCoinLb"];
//        m_upgradeCoinLb = lblDict["UpgradeCoinLb"];
//        m_upgradeInfoLb = lblDict["UpgradeInfoLb"];
//        m_upgradeSpr = goDict["UpgradeBtn"].GetComponent<UISprite>();
//        m_toolTipAnchorPoint = goDict["ToolTipAnchorPoint"].transform;

//        m_nInputLimit = UIInputLimit.ALL_OK;
//    }

//    private void InitCardGo(Transform root)
//    {
//        if (root == null)
//        {
//            return;
//        }

//        uiTransList.Clear();
//        uiCardList.Clear();
//        Transform child = null;
//        for (int i = 0; i < root.childCount; i++)
//        {
//            child = root.GetChild(i);
//            UIShopCardItem card = InitCardItem(child);
//            if (card != null)
//            {
//                child.name = i.ToString();
//                card.clickOnCardItem = ClickOnCardList;
//                card.clickOnDetailBtn = ClickOnCardDetail;

//                uiCardList.Add(card);
//                uiTransList.Add(child);
//            }
//            else
//            {
//                child.gameObject.SetActive(false);
//            }
//        }
//    }

//    protected UIShopCardItem InitCardItem(Transform trans)
//    {
//        UIShopCardItem cardItem = trans.gameObject.AddMissingComponent<UIShopCardItem>();

//        if (cardItem != null)
//        {
//            cardItem.SetDraggable(false);
//            cardItem.longPressOnCardItem = LongPressOnCardItem;
//        }
//        return cardItem;
//    }

//    protected void LongPressOnCardItem(UIShopCardItem item, bool isDown)
//    {
//        if (item == null || UIShopCardItem.IsCurSelectedItem(item))
//        {
//            return;
//        }

//        if (isDown)
//        {
//            UIManager.instance.ShowUI(TheUIPrefabIds.UI_BATTLE_CARD_INFO, new object[] { item.id, m_toolTipAnchorPoint.position });
//        }
//        else
//        {
//            UIManager.instance.CloseUI(TheUIPrefabIds.UI_BATTLE_CARD_INFO);
//        }
//    }

//    protected void SetUsabel(UIShopCardItem uiCard, bool canUse)
//    {
//        if (uiCard == null)
//        {
//            return;
//        }

//        uiCard.SetGrey(!canUse);
//        uiCard.SetUsable(canUse);

//        if (!canUse && UIShopCardItem.IsCurSelectedItem(uiCard))
//        {
//            ResetSelectedItem();
//        }
//    }

//    protected void OnInitializeItem(GameObject go, int wrapIndex, int realIndex)
//    {
//        if (go == null || uiCardList == null || dataCardList == null)
//        {
//            return;
//        }

//        if (wrapIndex < 0 || wrapIndex >= uiCardList.Count)
//        {
//            return;
//        }

//        realIndex = Mathf.Abs(realIndex);
//        if (realIndex < 0 || realIndex >= dataCardList.Count)
//        {
//            return;
//        }

//        uiCardList[wrapIndex].wrapIndex = wrapIndex;
//        uiCardList[wrapIndex].realIndex = realIndex;
//        uiCardList[wrapIndex].SetCardData(dataCardList[realIndex]);

//        SetUsabel(uiCardList[wrapIndex], CanGet(uiCardList[wrapIndex]));

//        if (UIShopCardItem.IsCurSelectedItem(uiCardList[wrapIndex]))
//        {
//            // deselect the item when reuse
//            ResetSelectedItem();
//        }
//    }

//    public bool IsInitCard(UICardData card)
//    {
//        if (Utils.IsMainCityCardId(card.id))
//        {
//            return true;
//        }

//        CardInfo cardInfo = DictUtils.GetCardInfoById(card.id);
//        if (cardInfo.IsInitCard)
//        {
//            return true;
//        }
//        return false;
//    }

//    public override void Open(object param, UIPathData pathData)
//    {
//        // TODO：临时
//        // set card data
//        List<common.player_one_card_info> cardData = new List<common.player_one_card_info>();
//        List<IInfo> infoList = DictMgr.instance.GetInfoList(DictMgr.instance.cardInfoDict);
//        for (int i = 0; i < infoList.Count; i++)
//        {
//            CardInfo cardInfo = infoList[i] as CardInfo;
//            if (cardInfo == null)
//            {
//                continue;
//            }

//            common.player_one_card_info info = new common.player_one_card_info();
//            info.card_id = cardInfo.id;
//            cardData.Add(info);
//        }
//        User.instance.CardMgr.SetCardList(cardData);

//        AudioMgr.instance.PlayerUIAudio(AduioConfig.UI_SWITCH_NORMAL);

//        base.Open(param, pathData);
//        Role.Kingdom kingdom = (Role.Kingdom)param;
//        //Logger.Assert(kingdom != Role.Kingdom.NONE, "kingdom = Role.Kingdom.NONE");
//        List<UserCardInfo> cardList = User.instance.CardMgr.CardList;
        
//        m_kingdom = kingdom;
//        m_requireLevel = 1;
//        UICardData.SetCardData(cardList, dataAllCardList, IsInitCard);

//        UpdateRequireCoin();
//        UpdatePlayerCard();

//        FilterCardData();
//        RefreshCardList();
//        ResetSelectedItem();
//        RefreshUpgradeRoot();

//        if (m_requireLevelBtnGroup != null)
//        {
//            m_requireLevelBtnGroup.ResetToBeginning();
//        }
//        RefreshRequireLevelBtnGroup();
//        SwitchBattleMainBtn(false);

//        UICamera uiCamera = UICamera.FindCameraForLayer(gameObject.layer);
//        if (uiCamera)
//        {
//            uiCamera.tooltipDelay = 0.5f;
//        }

//        int maincityLevel = GetMaincityMaxLevel();
//        RequireLevel = maincityLevel;
//        m_requireLevelBtnGroup.ClickOn(maincityLevel);

//        m_nUsableOnly = null;
//    }

//    protected void SwitchBattleMainBtn(bool show)
//    {
//        BaseUI baseUI = UIManager.instance.GetUI(TheUIPrefabIds.UI_BATTLE_MAIN);
//        if (baseUI == null)
//        {
//            return;
//        }

//        GameObject shopBtn = baseUI.GetUIPartObject(UIPartType.CUSTOM, UIBattleMain.SHOP_BTN);
//        if (shopBtn != null)
//        {
//            shopBtn.SetActive(show);
//        }

//    }

//    protected void ClickOnRequireLevelBtnGroup(UIBtnGroup btnGroup)
//    {
//        if (btnGroup.CurrentBindData != null)
//        {
//            RequireLevel = (int)btnGroup.CurrentBindData;
//        }
//    }

//    public void CloseWin(bool bCheckInputLimit = true)
//    {
//        if(bCheckInputLimit && m_nInputLimit != UIInputLimit.ALL_OK && m_nInputLimit != UIInputLimit.BATTLE_SHOP_CLOSE_WIN)
//        {
//            return;
//        }

//        AudioMgr.instance.PlayerUIAudio(AduioConfig.UI_SWITCH_LIGHT);

//        UICardData.SetCardData((List<UserCardInfo>)null, dataAllCardList);
//        FilterCardData();
//        SwitchBattleMainBtn(true);

//        base.CloseWinFunc();
//    }

//    public int RequireLevel
//    {
//        get
//        {
//            return m_requireLevel;
//        }
//        set
//        {
//            if (m_requireLevel != value)
//            {
//                m_requireLevel = value;

//                FilterCardData();
//                RefreshCardList();
//                ResetSelectedItem();
//            }
//        }
//    }


//    public List<int> SelCardList
//    {
//        get
//        {
//            return selCardList;
//        }
//        set
//        {
//            selCardList = value;
//        }
//    }


//    protected void FilterCardData()
//    {
//        if (dataAllCardList == null || dataCardList == null)
//        {
//            return;
//        }

//        dataCardList.Clear();
//        for (int i = 0; i < dataAllCardList.Count; i++)
//        {
//            UICardData data = dataAllCardList[i];
//            if ((data.kingdom == m_kingdom || data.kingdom == Kingdom.Common) && data.requireLv == RequireLevel)
//            {
//                if (!selCardList.Contains(dataAllCardList[i].id))
//                {
//                    dataCardList.Add(data);
//                }
//            }
//        }
//    }

//    public void RefreshCardList(bool onlySelState = false)
//    {
//        if (uiCardList == null || uiCardList.Count == 0)
//        {
//            return;
//        }

//        if (onlySelState)
//        {
//            for (int i = 0; i < uiCardList.Count; i++)
//            {
//                if (!uiCardList[i].gameObject.activeSelf)
//                {
//                    continue;
//                }
                
//                SetUsabel(uiCardList[i], CanGet(uiCardList[i]));
//            }
//        }
//        else
//        {
//            RefreshWrapContent();
//        }
//    }

//    public UIShopCardItem GetUICardByID(int nCardID)
//    {
//        for (int i = 0; i < uiCardList.Count; i++)
//        {
//            if(uiCardList[i] == null)
//            {
//                continue;
//            }
//            if (uiCardList[i].id == nCardID)
//            {
//                return uiCardList[i];
//            }
//        }
//        return null;
//    }

//    private void ToSetUsableOnly(int[] nUsableOnly)
//    {
//        m_nUsableOnly = nUsableOnly;

//        RefreshCardList(true);
//    }

//    protected void RefreshWrapContent()
//    {
//        if (m_wrapContent != null)
//        {
//            int showCount = dataCardList == null ? 0 : dataCardList.Count;
//            if (showCount == 0)
//            {
//                m_wrapContent.gameObject.SetActive(false);
//            }
//            else
//            {
//                m_wrapContent.gameObject.SetActive(true);
//                m_wrapContent.maxPerLine = 3;
//                m_wrapContent.InitChildren(uiTransList, showCount);
//                m_wrapContent.RestToBeginning();
//            }
//        }
//    }

//    protected void RefreshRequireLevelBtnGroup()
//    {
//        if (m_requireLevelBtnGroup == null)
//        {
//            return;
//        }
        
//        int maincityLevel = GetMaincityMaxLevel();
//        for (int i = 1; i <= m_requireLevelBtnGroup.ChildCount; i++)
//        {
//            m_requireLevelBtnGroup.SetEnable(i, i <= maincityLevel);
//        }
//    }

//    private int GetMaincityMaxLevel()
//    {
//        Player selfPlayer = PlayerManager.instance.GetSelfPlayer();
//        if (selfPlayer == null)
//        {
//            return 0;
//        }

//        return selfPlayer.MaincityMaxLevel;
//    }

//    private void UpdateBuildUpLevel(Charactor cha)
//    {
//        if (cha == null || !PlayerManager.instance.CanBuildUpgrade || cha.CharaType() != CharactorType.MAINCITY)
//        {
//            return;
//        }
//        if (cha.MyUID() != PlayerManager.instance.GetSelfPlayerUID())
//        {
//            return;
//        }

//        m_requireLevelBtnGroup.ClickOn(cha.Level);
//        RefreshRequireLevelBtnGroup();
//        RefreshCardList(true);
//        RefreshUpgradeRoot();
//    }

//    protected void ClickOnCardList(UICardItem card)
//    {
//        if (m_nInputLimit != UIInputLimit.ALL_OK && m_nInputLimit != UIInputLimit.BATTLE_SHOP_SELECT_CARD)
//        {
//            return;
//        }

//        if (card != null && !UIShopCardItem.IsCurSelectedItem(card.id))
//        {
//            UIShopCardItem srcCard = card as UIShopCardItem;
//            if (srcCard != null)
//            {
//                srcCard.SetSelected(true);

//                if (mScrollView != null)
//                {
//                    mScrollView.RestrictTargetWithinBounds(srcCard.root, false);
//                }
                
//                UIManager.instance.ShowUI(TheUIPrefabIds.UI_BATTLE_CARD_INFO, new object[] { card.id, m_toolTipAnchorPoint.position });

//                if(CtlBattle.instance.GetBattleLogic().GetBattleMode() == Battle_Mode.Plot)
//                {
//                    SequenceMgr.Instance.TriggerEvent(_Sequence.SequenceEventType.SELECT_CARD_IN_BATTLE_SHOP, card.id);
//                }
//            }
//        }
//    }

//    public int RequireCoin
//    {
//        get
//        {
//            return m_requireCoin;
//        }
//        set
//        {
//            if (m_requireCoin != value)
//            {
//                m_requireCoin = value;
//                if (m_requireCoinLb != null)
//                {
//                    m_requireCoinLb.text = value.ToString();
//                }
//            }
//        }
//    }

//    public int UpgradeCoin
//    {
//        get
//        {
//            return m_upgradeCoin;
//        }
//        set
//        {
//            if (m_upgradeCoin != value)
//            {
//                m_upgradeCoin = value;
//                if (m_upgradeCoinLb != null)
//                {
//                    m_upgradeCoinLb.text = value.ToString();
//                }
//            }
//        }
//    }

//    protected bool CanGet(int cardID)
//    {
//        if (m_nUsableOnly != null)
//        {
//            bool bUsableOnly = false;
//            for (int i = 0; i < m_nUsableOnly.Length; i++)
//            {
//                if (m_nUsableOnly[i] == cardID)
//                {
//                    bUsableOnly = true;
//                    break;
//                }
//            }
//            if (! bUsableOnly)
//            {
//                return false;
//            }
//        }

//        return Utils.CanShopGetCard(PlayerManager.instance.GetSelfPlayerUID(), cardID);
//    }

//    protected bool CanGet(UIShopCardItem card)
//    {
//        //Logger.Assert(card != null, "card null!!!");
//        return CanGet(card.id);
//    }

//    protected void ClickOnCardDetail(UISrcCardItem uiCard)
//    {
//        Logger.Log("Click On Get!");
//        UIShopCardItem shopCard = uiCard as UIShopCardItem;
//        if (shopCard == null || !shopCard.IsUsable)
//        {
//            return;
//        }

//        m_curGetCard = shopCard;
//        m_curRequireCoin = RequireCoin;
//        ResetSelectedItem();
//        if (CanGet(m_curGetCard))
//        {
//            AudioMgr.instance.PlayerUIAudio(AduioConfig.UI_CARD_GET);

//            // 买卡
//            FrameCommand command = FrameCommandFactory.GetShopGetCard(PlayerManager.instance.GetSelfPlayerUID(), uiCard.id);
//            if (command != null)
//            {
//                command.Send();
//            }
//        }
//    }

//    public void ResetSelectedItem()
//    {
//        if (UIShopCardItem.curSelItem != null)
//        {
//            UIShopCardItem.curSelItem.SetSelected(false);
//        }

//        if (mScrollView != null)
//        {
//            mScrollView.RestrictWithinBounds(false, false, true);
//        }

//        UIManager.instance.CloseUI(TheUIPrefabIds.UI_BATTLE_CARD_INFO);
//    }

//    protected void DoGetCard()
//    {
//        UpdatePlayerCard();
//        ShowGetCardEffect();
//    }

//    protected void DoUpdateCard()
//    {
//        UpdatePlayerCard();
//    }

//    protected void UpdatePlayerCard()
//    {
//        UpdateRequireCoin();

//        List<BattleCard> battleCards = PlayerManager.instance.GetSelfPlayer().GetCardList();
//        if (battleCards != null)
//        {
//            selCardList.Clear();

//            for (int i = 0; i < battleCards.Count; i++)
//            {
//                if (battleCards[i] != null)
//                {
//                    selCardList.Add(battleCards[i].GetCardID());
//                }
//            }

//            FilterCardData();
//            RefreshCardList();
//            ResetSelectedItem();
//        }
//    }
    

//    protected void ShowGetCardEffect()
//    {
//        Vector3 startPosition = Vector3.zero;
//        if (m_curGetCard != null && m_effectRoot != null)
//        {
//            startPosition = m_curGetCard.root.position;
//            // 资源消耗
//            if (m_resCostRoot != null)
//            {
//                if (m_resNode.parent == null)
//                {
//                    Object prefabs = ResourceMgr.instance.SyncLoad(ResourceMgr.RESTYPE.UI, TheGameIds.UI_USE_OR_GAIN_RES).resObject;
//                    if (prefabs != null)
//                    {
//                        Transform trans = (GameObject.Instantiate(prefabs) as GameObject).transform;
//                        trans.parent = m_resCostRoot.transform;
//                        trans.localScale = Vector3.one;
//                        m_resNode = new UseGainResNode(trans);
//                    }
//                }

//                if (m_resNode.parent != null)
//                {
//                    m_resNode.parent.position = startPosition + new Vector3(0f, 0.2f, 0f);
//                    m_resNode.iconSpr.spriteName = "gold";
//                    m_resNode.useLbl.text = (-m_curRequireCoin).ToString();

//                    m_resNode.posAnim.ResetToBeginning();
//                    m_resNode.posAnim.PlayForward();
//                    m_resNode.alpha.ResetToBeginning();
//                    m_resNode.alpha.PlayForward();

//                    AudioMgr.instance.PlayerUIAudio(AduioConfig.ITEMFX_CRYSTAL_STORE);
//                }
//            }
//        }
//        BroadcastMsg(startPosition);
//    }

//    protected void BroadcastMsg(Vector3 startPosition)
//    {
//        Messenger.Broadcast(MessageName.MN_ON_PLAYERCARD_DO_GET, m_curGetCard != null ? m_curGetCard.id : -1, startPosition);
//        m_curGetCard = null;
//    }

//    protected void UpdateRequireCoin()
//    {
//        Player selfPlayer = PlayerManager.instance.GetSelfPlayer();
//        if (selfPlayer == null)
//        {
//            return;
//        }

//        RequireCoin = selfPlayer.ShopGetCardRequireCoin;
//    }

//    private void SelfCoinChg(int nChg, ChangeReason changeReason)
//    {
//        UpdateRequireCoin();
//        RefreshCardList(true);
//        RefreshUpgradeRoot();
//    }

//    private void OnSBDie(int charaID)
//    {
//        Player selfPlayer = PlayerManager.instance.GetSelfPlayer();
//        if (selfPlayer == null)
//        {
//            return;
//        }

//        if (selfPlayer.MaincityID != charaID)
//        {
//            return;
//        }
//        RefreshUpgradeRoot();
//    }

//    private void RefreshUpgradeRoot()
//    {
//        if (m_upgradeRoot == null)
//        {
//            return;
//        }

//        Player selfPlayer = PlayerManager.instance.GetSelfPlayer();
//        if (selfPlayer == null)
//        {
//            return;
//        }

//        Charactor chara = CharactorManager.instance.GetCharactor(selfPlayer.MaincityID);
//        if (chara == null)
//        {
//            return;
//        }

//        if (!chara.IsLive())
//        {
//            m_upgradeRoot.gameObject.SetActive(false);
//            return;
//        }
        
//        int maincityLevel = chara.Level;
//        if (maincityLevel > 0 && maincityLevel < BattleConst.MAINCITY_LEVEL)
//        {
//            UpgradeCoin = DictUtils.GetResUpLevelInfo(chara.CardID(), chara.Level + 1).CostCoin;
//            if (selfPlayer.Coin >= UpgradeCoin)
//            {
//                m_upgradeCoinLb.color = new Color(229f / 255f, 1.0f, 1.0f);
//                m_upgradeSpr.color = Color.white;
//                m_upgradeInfoLb.color = Color.white;
//                m_upgradeInfoLb.effectStyle = UILabel.Effect.Outline;
//            }
//            else
//            {
//                m_upgradeCoinLb.color = Color.red;
//                m_upgradeSpr.color = Color.black;
//                m_upgradeInfoLb.color = Color.black;
//                m_upgradeInfoLb.effectStyle = UILabel.Effect.None;
//            }
//            m_upgradeRoot.gameObject.SetActive(true);
//        }
//        else
//        {
//            m_upgradeRoot.gameObject.SetActive(false);
//        }
//    }

//    private void UpgradeMaincity()
//    {
//#if UNITY_EDITOR || UNITY_IOS
//        // 留bug给xlua修复
//        return;
//#else
//        if (CtlBattle.instance.GetBattleLogic().CanBuildUpgrade())
//        {
//            Player selfPlayer = PlayerManager.instance.GetSelfPlayer();
//            if (selfPlayer != null)
//            {
//                Charactor maincity = CharactorManager.instance.GetCharactor(selfPlayer.MaincityID);
//                if (maincity != null)
//                {
//                    CtlBattle.instance.GetMapLogic().TryBuildUpgrade(maincity.GetSmallNode(), selfPlayer);
//                }
//            }
//        }
//#endif
//    }

//    protected override void NguiOnClick(GameObject go)
//    {
//        if (go == null)
//        {
//            return;
//        }
//        switch (go.name)
//        {
//            case "BattleShopCloseBtn":
//                {
//                    CloseWin();
//                    InputMgr.instance.SkipCurFrame();
//                    return;
//                }
//            case "UpgradeBtn":
//                {
//                    if (m_nInputLimit == UIInputLimit.ALL_OK || m_nInputLimit == UIInputLimit.BATTLE_SHOP_UPGRADE_BTN)
//                    {
//                        UpgradeMaincity();
//                        InputMgr.instance.SkipCurFrame();
//                        return;
//                    }
//                }
//                break;
//        }
//        base.NguiOnClick(go);
//    }
//}