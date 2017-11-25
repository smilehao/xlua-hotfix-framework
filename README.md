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

	-- 注意:循环区间为闭区间[0,testList.Count - 1]
	-- 适用于列表子集（子区间）遍历
	for i = 0, testList.Count - 1 do
		print('testList', i, testList[i])
	end

	-- 说明：工作方式与上述遍历一样，使用方式上雷同lua库的ipairs，类比于cs的foreach
	-- 适用于列表全集（整区间）遍历，推荐，很方便
	-- 注意：同cs的foreach，遍历函数体不能修改i,v，否则结果不可预料
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

	-- 说明：同helper.list_ipairs
	for i, v in helper.dictionary_ipairs(testDic) do
		print('testDic', i, v)
	end
	
	
3、添加消息示例

	---------------------------------消息系统热更测试---------------------------------
	-- 用法一：使用cs侧函数作为回调，必须在XLuaMessenger导出，无法新增消息监听，不支持重载函数
	messenger.add_listener(CS.MessageName.MN_ARENA_PERSONAL_PANEL, self, self.UpdatePanelInfo)
	messenger.add_listener(CS.MessageName.MN_RESET_RIVAL, self, self.UpdateRivalInfo)
	
	-- 用法二：使用lua函数作为回调，必须在XLuaMessenger导出，可以新增任意已导出的消息监听
	messenger.add_listener(CS.MessageName.MN_ARENA_PERSONAL_PANEL, self, TestLuaCallback)
	messenger.add_listener(CS.MessageName.MN_RESET_RIVAL, self, TestLuaCallback2)
	
	-- 用法三：使用CS侧成员委托，无须在XLuaMessenger导出，可以新增同类型的消息监听，CS侧必须缓存委托
	messenger.add_listener(CS.MessageName.MN_ARENA_UPDATE, self.updateLeftTimes)
	
	-- 用法四：使用反射创建委托，无须在XLuaMessenger导出，CS侧无须缓存委托，灵活度高，效率低，支持重载函数
	-- 注意：如果该消息在CS代码中没有使用过，则最好打[ReflectionUse]标签，防止IOS代码裁剪
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
		-- 注意：对于扩展方法，必须使用成员函数方式调用，不能在cs侧那样使用静态函数调用
		local iconGo = self.m_myRank:AddChild(iconPrefab)--正确
		--local iconGo = CS.NGUITools.AddChild(self.m_myRank, iconPrefab)--错误
		if iconGo ~= null then
			iconGo.transform.localPosition = Vector3(-128, 0, 0)
			iconGo.transform.localScale = Vector3.one
			-- 注意：此处AddMissingComponent扩展方法实际上调用的是CS.XLuaHelper下的AddMissingComponent方法
			self.m_userIconItem = iconGo:AddMissingComponent(typeof(CS.UserIconItem))
		end
	end
	
	
7、回调热更示例（消息系统的回调除外）

	--	1、缓存委托
	--  2、Lua绑定（实际上是创建LuaFunction再cast到delegate），需要在委托类型上打[CSharpCallLua]标签--推荐
	--	3、使用反射再执行Lua绑定
	for i, item in helper.list_ipairs(itemList) do
		-- 方式一：使用CS侧缓存委托
		local callback1 = self.onBagItemLoad
		-- 方式二：Lua绑定
		local callback2 = util.bind(function(self, gameObject, object)
			self:OnBagItemLoad(gameObject, object)
		end, self)
		-- 方式三：
		--	1、使用反射创建委托---这里没法直接使用，返回的是Callback<,>类型，没法隐式转换到CS.GameObjectPool.GetGameObjectDelegate类型
		--	2、再执行Lua绑定--需要在委托类型上打[CSharpCallLua]标签
		-- 注意：
		--	1、使用反射创建的委托可以直接在Lua中调用，但作为参数时，必须要求参数类型一致，或者参数类型为Delegate--参考Lua侧消息系统实现
		--	2、正因为存在类型转换问题，而CS侧的委托类型在Lua中没法拿到，所以在Lua侧执行类型转换成为了不可能，上面才使用了Lua绑定
		--	3、对于Lua侧没法执行类型转换的问题，可以在CS侧去做，这就是[CSharpCallLua]标签的作用，xlua底层已经为我们做好这一步
		--	4、所以，这里相当于方式二多包装了一层委托，从这里可以知道，委托做好全部打[CSharpCallLua]标签，否则更新起来很受限
		--	5、对于Callback和Action类型的委托（包括泛型）都在CS.XLuaHelper实现了反射类型创建，所以不需要依赖Lua绑定，可以任意使用
		-- 静态函数测试
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
		-- 方式一：新建Lua协程，优点：可新增协程；缺点：使用起来麻烦
		print('----------async call----------')
		util.coroutine_call(corotineTest)(self, 4)--相当于CS的StartCorotine，启动一个协程并立即返回
		print('----------async call end----------')
		
		-- 方式二：沿用CS协程，优点：使用方便，可直接热更协程代码逻辑，缺点：不可以新增协程
		self:StartCoroutine(self:TestCorotine(3))
	end)
	
	
9、cs侧协程热更示例

	xlua.hotfix(CS.UIRankMain, 'TestCorotine', function(self, seconds)
		print('HOTFIX:TestCorotine ', self, seconds)
		--注意：这里定义的匿名函数是无参的，全部参数以闭包方式传入
		return util.cs_generator(function()
			local s = os.time()
			print('coroutine start3 : ', s)
			--注意：这里直接使用coroutine.yield，跑在self这个MonoBehaviour脚本中
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
		-- 使用Unity的协程相关API：实际上也是CS侧协程结束时调用回调，驱动Lua侧协程继续往下跑
		-- 注意：这里会在CS.CorotineRunner新建一个协程用来等待3秒，这个协程是和self没有任何关系的
		yield_return(CS.UnityEngine.WaitForSeconds(seconds))
		print('coroutine end1 : ', os.time())
		print('This message1 appears after '..os.time() - s..' seconds in lua!')
		
		local s = os.time()
		print('coroutine start2 : ', s)
		-- 使用异步回调转同步调用模拟yield return
		-- 这里使用cs侧的函数也是可以的，规则一致：最后一个参数必须是一个回调，回调被调用时表示异步操作结束
		-- 注意：
		--	1、如果使用cs侧函数，必须将最后一个参数的回调（cs侧定义为委托）导出到[CSharpCallLua]
		--	2、用cs侧函数时，返回值也同样通过回调（cs侧定义为委托）参数传回
		local boolRetValue, secondsRetValue = util.async_to_sync(lua_async_test)(seconds)
		print('coroutine end2 : ', os.time())
		print('This message2 appears after '..os.time() - s..' seconds in lua!')
		-- 返回值测试
		print('boolRetValue:', boolRetValue, 'secondsRetValue:', secondsRetValue)
	end
	-- 模拟Lua侧的异步回调
	local function lua_async_test(seconds, coroutine_break)
		print('lua_async_test '..seconds..' seconds!')
		-- TODO：这里还是用Unity的协程相关API模拟异步，有需要的话再考虑在Lua侧实现一个独立的协程系统
		yield_return(CS.UnityEngine.WaitForSeconds(seconds))
		coroutine_break(true, seconds)
	end
