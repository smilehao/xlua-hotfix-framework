# xlua-framework
轻量级xlua热修复框架 
博客： http://www.cnblogs.com/SChivas/p/7893048.html

1、Lua创建和遍历泛型列表示例

	local helper = require 'common.helper'
	local testList = helper.new_list(typeof(CS.System.String))
	testList:Add('111')
	testList:Add('222')
	testList:Add('333')
	print('testList', testList, testList.Count, testList[testList.Count - 1])

	for i = 0, testList.Count - 1 do
		print('testList', i, testList[i])
	end

	for i, v in helper.list_ipairs(testList) do
		print('testList', i, v)
	end


2、Lua创建和遍历泛型字典示例

	local helper = require 'common.helper'
	local testDic = helper.new_dictionary(typeof(CS.System.Int32), typeof(CS.System.String))
	testDic:Add(111, 'aaa')
	testDic[222] = 'bbb'
	testDic[333] = 'ccc'
	print('testDic1', testDic, testDic.Count, testDic[333])--正确，注意：只有key为number类型才能够使用[]
	print('testDic2', testDic, testDic.Count, helper.try_get_value(testDic, 333))--正确，推荐，方式统一，不容易犯错

	testDic = helper.new_dictionary(typeof(CS.System.String), typeof(CS.System.Int32))
	testDic:Add('aaa', 111)
	testDic['bbbb'] = 222
	testDic['ccc'] = 333
	print('testDic3', testDic, testDic.Count, testDic['ccc'])--错误
	print('testDic4', testDic, testDic.Count, helper.try_get_value(testDic, 'ccc'))--正确

	for i, v in helper.dictionary_ipairs(testDic) do
		print('testDic', i, v)
	end
	
	
3、添加消息示例

	-- 用法一：使用cs侧函数作为回调
	messenger.add_listener(CS.MessageName.MN_ARENA_PERSONAL_PANEL, self, self.UpdatePanelInfo)
	messenger.add_listener(CS.MessageName.MN_RESET_RIVAL, self, self.UpdateRivalInfo)
	
	-- 用法二：使用lua函数作为回调
	messenger.add_listener(CS.MessageName.MN_ARENA_PERSONAL_PANEL, self, TestLuaCallback)
	messenger.add_listener(CS.MessageName.MN_RESET_RIVAL, self, TestLuaCallback2)
	
	-- 用法三：使用CS侧成员委托
	messenger.add_listener(CS.MessageName.MN_ARENA_UPDATE, self.updateLeftTimes)
	
	-- 用法四：使用反射创建委托
	messenger.add_listener(CS.MessageName.MN_ARENA_BOX, self, 'SetBoxState', typeof(CS.System.Int32))
	messenger.add_listener(CS.MessageName.MN_ARENA_CLEARDATA, self, 'ClearData')
	
	-- Lua消息响应
	local TestLuaCallback = function(self, param)
		print('LuaDelegateTest: ', self, param, param and param.rank)
	end

	local TestLuaCallback2 = function(self, param)
		print('LuaDelegateTest: ', self, param, param and param.Count)
	end
	
	
4、移除消息示例

	-- 用法一
	messenger.remove_listener(CS.MessageName.MN_ARENA_PERSONAL_PANEL, self, self.UpdatePanelInfo)
	messenger.remove_listener(CS.MessageName.MN_RESET_RIVAL, self, self.UpdateRivalInfo)
	
	-- 用法二
	messenger.remove_listener(CS.MessageName.MN_ARENA_PERSONAL_PANEL, self, TestLuaCallback)
	messenger.remove_listener(CS.MessageName.MN_RESET_RIVAL, self, TestLuaCallback2)
	
	-- 用法三
	messenger.remove_listener(CS.MessageName.MN_ARENA_UPDATE, self.updateLeftTimes)
	
	-- 用法四
	messenger.remove_listener(CS.MessageName.MN_ARENA_BOX, self, 'SetBoxState', typeof(CS.System.Int32))
	messenger.remove_listener(CS.MessageName.MN_ARENA_CLEARDATA, self, 'ClearData')


5、发送消息示例

	util.hotfix_ex(CS.UIArena, 'OnGUI', function(self)
		if Button(Rect(100, 300, 150, 80), 'lua BroadcastMsg1') then
			local testData = CS.ArenaPanelData()--正确
			--local testData = helper.new_object(typeof(CS.ArenaPanelData))--正确
			testData.rank = 7777;
			messenger.broadcast(CS.MessageName.MN_ARENA_PERSONAL_PANEL, testData)
		end
		
		if Button(Rect(100, 400, 150, 80), 'lua BroadcastMsg2') then
			local testData = helper.new_list(typeof(CS.ArenaRivalData))
			for i = 0, 22 do
				testData:Add(CS.ArenaRivalData())
			end
			messenger.broadcast(CS.MessageName.MN_RESET_RIVAL, testData)
		end
		
		if Button(Rect(100, 500, 150, 80), 'lua BroadcastMsg3') then
			local testData = CS.ArenaPanelData()
			testData.rank = 7777;
			messenger.broadcast(CS.MessageName.MN_ARENA_UPDATE, testData)
		end

		if Button(Rect(100, 600, 150, 80), 'lua BroadcastMsg4') then
			messenger.broadcast(CS.MessageName.MN_ARENA_BOX, 3)
		end
		self:OnGUI()
	end)
	
	
6、扩展方法示例

	-- 说明：静态函数调用使用'.'，成员函数调用使用':'
	local iconPrefab = CS.ResourceMgr.instance:LoadUIPrefab(CS.TheGameIds.PLAYER_ICON_ITEM)
	if iconPrefab ~= nil then
		local iconGo = self.m_myRank:AddChild(iconPrefab)--正确
		--local iconGo = CS.NGUITools.AddChild(self.m_myRank, iconPrefab)--错误
		if iconGo ~= null then
			iconGo.transform.localPosition = Vector3(-128, 0, 0)
			iconGo.transform.localScale = Vector3.one
			self.m_userIconItem = iconGo:AddMissingComponent(typeof(CS.UserIconItem))
		end
	end
	
	
7、回调热更示例（消息系统的回调除外）

	for i, item in helper.list_ipairs(itemList) do
		-- 方式一：使用CS侧缓存委托
		local callback1 = self.onBagItemLoad
		-- 方式二：Lua绑定
		local callback2 = util.bind(function(self, gameObject, object)
			self:OnBagItemLoad(gameObject, object)
		end, self)
		-- 方式三：
		local delegate = helper.new_callback(typeof(CS.UIArena), 'OnBagItemLoad2', typeof(CS.UnityEngine.GameObject), typeof(CS.System.Object))
		delegate(self.gameObject, nil)
		-- 成员函数测试
		local delegate = helper.new_callback(self, 'OnBagItemLoad', typeof(CS.UnityEngine.GameObject), typeof(CS.System.Object))
		local callback3 = util.bind(function(self, gameObject, object)
			delegate(gameObject, object)
		end, self)
		
		-- 其它测试：使用Lua绑定添加委托：必须[CSharpCallLua]导出委托类型，否则不可用
		callback5 = callback1 + util.bind(function(self, gameObject, object)
			print('callback4 in lua', self, gameObject, object)
		end, self)
		
		local callbackInfo = CS.GameObjectPool.CallbackInfo(callback3, item, Vector3.zero, Vector3.one * 0.65, self.m_awardGrid.gameObject)
		CS.UIGameObjectPool.instance:GetGameObject(CS.ResourceMgr.RESTYPE.UI, CS.TheGameIds.UI_BAG_ITEM_ICON, callbackInfo)
	
	
8、协程热更示例

	xlua.hotfix(CS.UIRankMain, 'Open', function(self, param, pathData)
		-- 方式一：新建Lua协程
		print('----------async call----------')
		util.coroutine_call(corotineTest)(self, 4)
		print('----------async call end----------')
		
		-- 方式二：沿用CS协程
		self:StartCoroutine(self:TestCorotine(3))
	end)
	
	
9、cs侧协程热更示例

	xlua.hotfix(CS.UIRankMain, 'TestCorotine', function(self, seconds)
		print('HOTFIX:TestCorotine ', self, seconds)
		return util.cs_generator(function()
			local s = os.time()
			print('coroutine start3 : ', s)
			coroutine.yield(CS.UnityEngine.WaitForSeconds(seconds))
			print('coroutine end3 : ', os.time())
			print('This message3 appears after '..os.time() - s..' seconds in lua!')
		end)
	end)
	
	
10、lua侧新建协程示例：本质上是在Lua侧建立协程，然后用异步回调驱动，

	local corotineTest = function(self, seconds)
		print('NewCoroutine: lua corotineTest', self)
		
		local s = os.time()
		print('coroutine start1 : ', s)
		yield_return(CS.UnityEngine.WaitForSeconds(seconds))
		print('coroutine end1 : ', os.time())
		print('This message1 appears after '..os.time() - s..' seconds in lua!')
		
		local s = os.time()
		print('coroutine start2 : ', s)
		local boolRetValue, secondsRetValue = util.async_to_sync(lua_async_test)(seconds)
		print('coroutine end2 : ', os.time())
		print('This message2 appears after '..os.time() - s..' seconds in lua!')
		print('boolRetValue:', boolRetValue, 'secondsRetValue:', secondsRetValue)
	end
	-- 模拟Lua侧的异步回调
	local function lua_async_test(seconds, coroutine_break)
		print('lua_async_test '..seconds..' seconds!')
		yield_return(CS.UnityEngine.WaitForSeconds(seconds))
		coroutine_break(true, seconds)
	end
