# ZimbraCSharpClient

從 [Zimbra](https://sourceforge.net/p/zimbra/code/HEAD/tree/)  的 ZimbraCSharpClient 加入其他的功能 Call Zimbra SOAP

## Zimbra.Client 為 Zimbra 的 .NET 專案
目前加入功能如下，

| 功能 | 說明 | Services |
| ------ | ------ | ------ |
| SearchCalendarResources | 找資源，例如會議室、車子 ... | Account |
| SearchGal | 依關鍵字 Search Email 資訊 | Account |
| GetShareInfo | 取得使用者分享的 Folder 資訊，在gss是 folderPath=/Calendar  | Account |
| GetWorkingHours | 取得可上班時間(似乎沒什麼用) | Mail |
| GetFreeBusy | 取得每個人的Free or Busy 時間 | Mail |
| CreateAppointment | Book 會議室 | Mail |
| Search | Search 某個人的行事曆資訊 | Mail | 
| GetMsg | 透過 Message Id 取得會議相關資訊(一般是取得與會人員) | Mail | 
| CheckRecurConflicts | 檢查行事曆是否有衝突，一般用在 book 之前，先check | Mail |
| CancelAppointment | 取消會議(組織者才能使用) | Mail |
| SendInviteReply | 回覆會議(ACCEPT,DECLINE,TENTATIVE) | Mail |
| CreateMountpoint | 透過 Email 取得分享的行事曆，並加入掛載到自已身上 | Mail |
| AutoComplete | 透過關鍵字尋找 Email 資訊  | Mail |
 
### 2017/6/3 修改 AppointmentRequestParams 增加  Resources 屬性，建立 Appointment 時，可增加資源，例如 公務車、投影機 等等…

## Zimbra.Client.Test 為測試專案
### Initialize Method 設定 Zimbra 的登入資訊 
### AssignUserToken Method 透過帳密取回 Token 

- AccountTests.cs 
```C#
[TestMethod]
[Description("取得會議室清單(Location)")]
public void SearchCalendarResourcesRequestLocationTest()
{
	var calendarAttributes =
		"fullName,email,zimbraCalResLocationDisplayName,zimbraCalResCapacity,zimbraCalResContactEmail,notes,zimbraCalResType";
	var searchMeetingRooms = new SearchCalendarResourcesRequest(calendarAttributes);
	searchMeetingRooms.ProcToXmlDocument = (conditionFilters) =>
	{
		var condE = conditionFilters.OwnerDocument.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
		condE.SetAttribute(AccountService.A_ATTR, AccountService.V_ATTR_CALRES_TYPE);
		condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE_EQ);
		condE.SetAttribute(AccountService.A_VALUE, AccountService.V_LOCATION);
		conditionFilters.AppendChild(condE);
	};
	ZmailRequest.ApiRequest = searchMeetingRooms;
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as SearchCalendarResourcesResponse;
	var crList = resp?.CalendarResourceList;
	if (crList != null)
	{
		foreach (var cr in crList)
		{
			Console.WriteLine(cr.id);
			Console.WriteLine(string.Join(Environment.NewLine, cr.AttributesList.Select(x => x.Key + "=" + x.Value).ToArray()));
		}
	}
}


[TestMethod]
[Description("取得資源清單(Equipment)")]
public void SearchCalendarResourcesRequestEquipmentTest()
{
	var calendarAttributes =
		"fullName,email,zimbraCalResLocationDisplayName,zimbraCalResCapacity,zimbraCalResContactEmail,notes,zimbraCalResType";
	var searchMeetingRooms = new SearchCalendarResourcesRequest(calendarAttributes);
	searchMeetingRooms.ProcToXmlDocument = (conditionFilters) =>
	{
		var condE = conditionFilters.OwnerDocument.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
		condE.SetAttribute(AccountService.A_ATTR, AccountService.V_ATTR_CALRES_TYPE);
		condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE_EQ);
		condE.SetAttribute(AccountService.A_VALUE, AccountService.V_EQUIPMENT);
		conditionFilters.AppendChild(condE);
	};
	ZmailRequest.ApiRequest = searchMeetingRooms;
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as SearchCalendarResourcesResponse;
	var crList = resp?.CalendarResourceList;
	if (crList != null)
	{
		foreach (var cr in crList)
		{
			Console.WriteLine(cr.id);
			Console.WriteLine(string.Join(Environment.NewLine, cr.AttributesList.Select(x => x.Key + "=" + x.Value).ToArray()));
		}
	}
}


[TestMethod]
[Description("取得資源清單(Equipment)&公務車")]
public void SearchCalendarResourcesRequestEquipmentCarTest()
{
	var calendarAttributes =
		"fullName,email,zimbraCalResLocationDisplayName,zimbraCalResCapacity,zimbraCalResContactEmail,notes,zimbraCalResType";
	var searchMeetingRooms = new SearchCalendarResourcesRequest(calendarAttributes);
	searchMeetingRooms.ProcToXmlDocument = (conditionFilters) =>
	{
		var condE = conditionFilters.OwnerDocument.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
		condE.SetAttribute(AccountService.A_ATTR, AccountService.V_ATTR_CALRES_TYPE);
		condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE_EQ);
		condE.SetAttribute(AccountService.A_VALUE, AccountService.V_EQUIPMENT);
		conditionFilters.AppendChild(condE);
		//公務車
		condE = conditionFilters.OwnerDocument.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
		condE.SetAttribute(AccountService.A_ATTR, "fullName");
		condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE_STARTWITH);
		condE.SetAttribute(AccountService.A_VALUE, "公務車");
		conditionFilters.AppendChild(condE);
	};
	ZmailRequest.ApiRequest = searchMeetingRooms;
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as SearchCalendarResourcesResponse;
	var crList = resp?.CalendarResourceList;
	if (crList != null)
	{
		foreach (var cr in crList)
		{
			Console.WriteLine(cr.id);
			Console.WriteLine(string.Join(Environment.NewLine, cr.AttributesList.Select(x => x.Key + "=" + x.Value).ToArray()));
		}
	}
}

[TestMethod]
[Description("依Token去Request，一般來說不會一直用帳密去登入，所以取得token後，就給 Request Context 就可以了哦!")]
public void AuthRequesFromTokenTest()
{
	var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
	var zRequestContext = new RequestContext();
	var token = "取得的Token";
	zRequestContext.AuthToken = token;
 
	var calendarAttributes ="fullName,email";
	var searchMeetingRooms = new SearchCalendarResourcesRequest(calendarAttributes);
	//給 RequestContext 及 Request
	var zmailRequest = new RequestEnvelope(zRequestContext, searchMeetingRooms);
	var zResRequest = ZmailDispatcher.SendRequest(zmailRequest);
	var resp = zResRequest.ApiResponse as SearchCalendarResourcesResponse;
	var crList = resp?.CalendarResourceList;
	if (crList != null)
	{
		foreach (var cr in crList)
		{
			Console.WriteLine(cr.id);
			Console.WriteLine(string.Join(Environment.NewLine, cr.AttributesList.Select(x => x.Key + "=" + x.Value).ToArray()));
		}
	}
	
}

[TestMethod]
[Description("Search Email 帳號 Info")]
public void SearchCalTest()
{
	var searchString = "al";
	var searchCalReq = new SearchGalRequest(searchString);
	 
	ZmailRequest.ApiRequest = searchCalReq;
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as SearchGalResponse;
	var crList = resp?.CalendarResourceList;
	if (crList != null)
	{
		foreach (var cr in crList)
		{
			Console.WriteLine($"cnid:{cr.id}");
			Console.WriteLine($"fullName:{cr.AttributesList["fullName"]}, email:{cr.AttributesList["email"]}");
			//Console.WriteLine(string.Join(Environment.NewLine, cr.AttributesList.Select(x => x.Key + "=" + x.Value).ToArray()));
		}
	}
}


[TestMethod]
[Description("取得分享的 Folder 資訊，在gss是 folderPath=/Calendar ")]
public void GetShareInfoTest()
{
	var ownerEmail = "robin_wang@gss.com.tw";
	var getShareInfoRequest = new GetShareInfoRequest(ownerEmail);

	ZmailRequest.ApiRequest = getShareInfoRequest;
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as GetShareInfoResponse;
	var mid = resp.MountpointId;
	var ownerId = resp.OwnerId;
	//如果回傳mid為null的話，表示沒有加入被查詢人員的行事曆哦!
	//可透過 CreateMountpoint 建立
	Console.WriteLine($"MountpointId:{mid}");
	Console.WriteLine($"OwnerId:{ownerId}");
}

```

- MailTests.cs  
```C#
[TestMethod]
public void GetWorkingHoursRequestTest()
{
	var sdate = new DateTime(2017, 4, 7, 08, 0, 0);
	var edate = new DateTime(2017, 4, 7, 18, 0, 0);
	//多人請用逗號隔開
	var searchNames = "rainmaker_ho@gss.com.tw,alice_lai@gss.com.tw";
	ZmailRequest.ApiRequest = new GetWorkingHoursRequest(sdate, edate, searchNames);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as GetWorkingHoursResponse;
	var wkHours = resp?.Workinghours;
	if (wkHours != null)
	{
		foreach (var user in wkHours.Users)
		{
			Console.WriteLine(user.id);
			Console.WriteLine("Free");
			foreach (var f in user.Fs)
			{
				Console.WriteLine($" {f.s} - {f.e}");
			}
			Console.WriteLine("Busy");
			foreach (var b in user.Bs)
			{
				Console.WriteLine($" {b.s} - {b.e}");
			}
			Console.WriteLine("Unavailable");
			foreach (var u in user.Us)
			{
				Console.WriteLine($" {u.s} - {u.e}");
			}
		}
	}
}

[TestMethod]
[ExpectedException(typeof(ZimbraException), "查詢結束時間不可以小於等於開始時間")]
public void GetWorkingHoursRequestWithSameTimeTest()
{
	var sdate = new DateTime(2017, 4, 7, 18, 0, 0);
	var edate = new DateTime(2017, 4, 7, 18, 0, 0);
	//多人請用逗號隔開
	var searchNames = "rainmaker_ho@gss.com.tw";
	ZmailRequest.ApiRequest = new GetWorkingHoursRequest(sdate, edate, searchNames);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as GetWorkingHoursResponse;
	var wkHours = resp?.Workinghours;
	if (wkHours != null)
	{
		foreach (var user in wkHours.Users)
		{
			Console.WriteLine(user.id);
			Console.WriteLine("Free");
			foreach (var f in user.Fs)
			{
				Console.WriteLine($" {f.s} - {f.e}");
			}
			Console.WriteLine("Busy");
			foreach (var b in user.Bs)
			{
				Console.WriteLine($" {b.s} - {b.e}");
			}
			Console.WriteLine("Unavailable");
			foreach (var u in user.Us)
			{
				Console.WriteLine($" {u.s} - {u.e}");
			}
		}
	}
}

[TestMethod]
public void GetFreeBusyRequestTest()
{
	var sdate = new DateTime(2017, 4, 7, 08, 0, 0);
	var edate = new DateTime(2017, 4, 7, 18, 0, 0);
	//多人請用逗號隔開
	var searchNames = "rainmaker_ho@gss.com.tw";
	ZmailRequest.ApiRequest = new GetFreeBusyRequest(sdate, edate, searchNames);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as GetFreeBusyResponse;
	var wkHours = resp?.Workinghours;
	if (wkHours != null)
	{
		foreach (var user in wkHours.Users)
		{
			Console.WriteLine(user.id);
			Console.WriteLine("Free");
			foreach (var f in user.Fs)
			{
				Console.WriteLine($" {f.s} - {f.e}");
			}
			Console.WriteLine("Busy");
			foreach (var b in user.Bs)
			{
				Console.WriteLine($" {b.s} - {b.e}");
			}
			Console.WriteLine("Unavailable");
			foreach (var u in user.Us)
			{
				Console.WriteLine($" {u.s} - {u.e}");
			}
		}
	}
}


[TestMethod]
[ExpectedException(typeof(ZimbraException), "查詢結束時間不可以小於等於開始時間")]
public void GetFreeBusyRequestWithSameTimeTest()
{
	var sdate = new DateTime(2017, 4, 7, 08, 0, 0);
	var edate = new DateTime(2017, 4, 7, 08, 0, 0);
	//多人請用逗號隔開
	var searchNames = "rainmaker_ho@gss.com.tw";
	ZmailRequest.ApiRequest = new GetFreeBusyRequest(sdate, edate, searchNames);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as GetFreeBusyResponse;
	var wkHours = resp?.Workinghours;
	if (wkHours != null)
	{
		foreach (var user in wkHours.Users)
		{
			Console.WriteLine(user.id);
			Console.WriteLine("Free");
			foreach (var f in user.Fs)
			{
				Console.WriteLine($" {f.s} - {f.e}");
			}
			Console.WriteLine("Busy");
			foreach (var b in user.Bs)
			{
				Console.WriteLine($" {b.s} - {b.e}");
			}
			Console.WriteLine("Unavailable");
			foreach (var u in user.Us)
			{
				Console.WriteLine($" {u.s} - {u.e}");
			}
		}
	}
}



[TestMethod]
[Description("新增行事曆，外出")]
public void CreateAppointmentRequestSelfTest()
{
	var app = new AppointmentRequestParams();
	app.Subject = "到台灣銀行洽工 ";
	app.StartDate = new DateTime(2017, 4, 7, 08, 30, 0);
	app.EndDate = new DateTime(2017, 4, 7, 12, 00, 0);
	app.Organizer = new Attendee { Email = "rainmaker_ho@gss.com.tw", DisplayName = "亂馬客" };
	 
	ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as CreateAppointmentResponse;
	var appResp = resp?.AppointmentResponse;
	Console.WriteLine($"{appResp?.InviteMessageId}");
}

[TestMethod]
[Description("新增行事曆，訂會議室, 自已")]
public void CreateAppointmentRequestSelfBookTest()
{
	var app = new AppointmentRequestParams();
	app.Subject = "開會，新增行事曆，訂會議室 測試";
	app.StartDate = new DateTime(2017, 4, 6, 9, 30, 0);
	app.EndDate = new DateTime(2017, 4, 6, 10, 30, 0);
	app.Organizer = new Attendee { Email = "rainmaker_ho@gss.com.tw", DisplayName = "亂馬客" };
	app.Locations = new List<Attendee>{
		new Attendee{ DisplayName = "協志會議室-舞蝶館",Email = "room_xz_01@gss.com.tw"}
	};

	ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as CreateAppointmentResponse;
	var appResp = resp?.AppointmentResponse;
	Console.WriteLine($"{appResp?.InviteMessageId}");
}
 

[TestMethod]
[Description("新增行事曆，訂會議室, 還有其他人")]
public void CreateAppointmentRequestBookTest()
{
	var app = new AppointmentRequestParams();
	app.Subject = "開會，新增行事曆，訂會議室  ";
	app.StartDate = new DateTime(2017, 4, 6, 13,0, 0);
	app.EndDate = new DateTime(2017, 4, 6, 14, 00, 0);
	app.Organizer = new Attendee {DisplayName = "RM", Email = "rainmaker_ho@gss.com.tw"};
	app.Locations = new List<Attendee>{
		new Attendee{ DisplayName = "協志會議室-舞蝶館",Email = "room_xz_01@gss.com.tw"}
	};
	app.Attendees = new List<Attendee>{
		
		new Attendee { Email = "jennifer_yang@gss.com.tw", DisplayName = "丸子姐" }
	};
	ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as CreateAppointmentResponse;
	var appResp = resp?.AppointmentResponse;
	Console.WriteLine($"{appResp?.InviteMessageId}");
}

 

[TestMethod]
[Description("取得行事曆的資訊")]
public void SearchRequestTest()
{
	var sdate = new DateTime(2017, 4, 7, 08, 0, 0);
	var edate = new DateTime(2017, 4, 7, 18, 0, 0);
	var searchReqParams = new SearchRequestParams();
	searchReqParams.LocalEnd = edate;
	searchReqParams.LocalStart = sdate;
	ZmailRequest.ApiRequest = new SearchRequest(searchReqParams);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);

	//取出會議室的資訊
	var searchRes = zResquest.ApiResponse as SearchResponse;
	var appts = searchRes?.Appointments;
	if (appts != null)
	{
		foreach (var appt in appts.OrderBy(a=>a.StartTime))
		{
			Console.WriteLine($"{appt.Name}:Start:{appt.StartTime}, 組織者:{appt.Organizer.DisplayName}, {appt.Organizer.Email}");
		}
	}
}


[TestMethod]
[Description("取得行事曆包含參與人員的資訊")]
public void SearchRequestWithAttendeesTest()
{
	var sdate = new DateTime(2017, 4, 7, 08, 0, 0);
	var edate = new DateTime(2017, 4, 7, 18, 0, 0);
	var searchReqParams = new SearchRequestParams();
	searchReqParams.LocalEnd = edate;
	searchReqParams.LocalStart = sdate;
	ZmailRequest.ApiRequest = new SearchRequest(searchReqParams);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);

	//取出會議室的資訊
	var searchRes = zResquest.ApiResponse as SearchResponse;
	var appts = searchRes?.Appointments;
	if (appts != null)
	{
		foreach (var appt in appts.OrderBy(a => a.StartTime))
		{
			Console.WriteLine($"{appt.Name}:Start:{appt.StartTime}, 組織者:{appt.Organizer.DisplayName}, {appt.Organizer.Email}");
			Console.WriteLine("與會人員 *** ");
			ZmailRequest.ApiRequest = new GetMsgRequest(appt.InviteMessageId);
			var msgRequest = ZmailDispatcher.SendRequest(ZmailRequest);
			var msgResp = msgRequest.ApiResponse as GetMsgResponse;
			var attendees = msgResp?.Attendees;
			if (attendees != null)
			{
				foreach (var attendee in attendees)
				{
					Console.WriteLine($"  {attendee.DisplayName}, {attendee.Email}, {attendee.UserType}");
				}
			}
		}
	}
}

[TestMethod]
[Description("檢查行事曆是否有衝突，一般用在 book 之前，先check")]
public void CheckRecurConflictsRequestTest()
{
	var sdate = new DateTime(2017, 4, 11, 08, 30, 0);
	var edate = new DateTime(2017, 4, 11, 10, 0, 0);
	var roomEmail = "room_801@gss.com.tw,room_501@gss.com.tw";
   
	ZmailRequest.ApiRequest = new CheckRecurConflictsRequest(sdate, edate, roomEmail);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as CheckRecurConflictsResponse;
	var conflictUsers = resp?.BusyUsers;
	Console.WriteLine("衝突的Users ...");
	if (conflictUsers != null)
	{
		if (conflictUsers.Count > 0)
		{
			foreach (string email in conflictUsers)
			{
				Console.WriteLine($"  {email}");
			}
		}
		else
		{
			Console.WriteLine("沒有衝突 !!!");
		}
	}
}

[TestMethod]
[Description("回覆拒絕參加會議")]
public void SendInviteReplyRequestSelfTest()
{
	var param = new SendInviteReplyRequestParam();
	param.Subject = "討論WCF與EF架構";
	param.Body = "DECLINE:討論WCF與EF架構";
	//需要先取得 invId 不然測試會失敗
	param.Id = "32412-32411";
	param.Organizer = new Attendee {Email = "alice_lai@gss.com.tw"};
	param.Replier = new Attendee { Email = "rainmaker_ho@gss.com.tw" };
	param.Verb = SendInviteReplyRequestParam.ReplyVerbs.DECLINE;

	ZmailRequest.ApiRequest = new SendInviteReplyRequest(param);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as SendInviteReplyResponse;
	var invId = resp?.InvId;
	Console.WriteLine($"InvId:{invId}");
}


[TestMethod]
[Description("Book 會議室，查詢多人多天，可以使用那些設備的時間")]
public void GetFreeBusyRequestTimePeriodIntersectorTest()
{
	//取得會議室資訊
	var calendarAttributes =
		"fullName,email";
	var searchMeetingRooms = new SearchCalendarResourcesRequest(calendarAttributes);
   
	ZmailRequest.ApiRequest = searchMeetingRooms;
	var zResquestRoom = ZmailDispatcher.SendRequest(ZmailRequest);
	var respRoom = zResquestRoom.ApiResponse as SearchCalendarResourcesResponse;
	var crList = respRoom?.CalendarResourceList;
	 
	if (crList != null)
	{
		var crEmails = crList.Select(cr => cr.AttributesList.Where(ar => ar.Key.Equals("email")).Select(ar => ar.Value).FirstOrDefault());
		var sdate = DateTime.Now;
		var edate = sdate.AddDays(7);
		//多人請用逗號隔開
		var crEmailStrings = string.Join(",", crEmails.ToArray());
		var searchNames = "rainmaker_ho@gss.com.tw,teresa_hua@gss.com.tw," + crEmailStrings;
		ZmailRequest.ApiRequest = new GetFreeBusyRequest(sdate, edate, searchNames);
		var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
		var resp = zResquest.ApiResponse as GetFreeBusyResponse;
		var wkHours = resp?.Workinghours;
		if (wkHours != null)
		{
			var periods = new Dictionary<string, TimePeriodCollection>();
			var users = wkHours.Users.Where(u => !crEmails.Contains(u.id));

			//找出所有人的共同時間
			var periodIntersector = new TimePeriodIntersector<TimeRange>();
			var resultPeriods = new TimePeriodCollection();
			var idx = 0;
			foreach (var user in users)
			{
				Console.WriteLine(user.id);
				Console.WriteLine("Free");
				var userPeriods = new TimePeriodCollection();
			 
				foreach (var f in user.Fs)
				{
					Console.WriteLine($" {f.s} - {f.e}");
					userPeriods.Add(new TimeRange(f.s, f.e));
				}
				periods.Add(user.id, userPeriods);
				if (idx == 0)
				{
					resultPeriods.AddAll(user.Fs.Select(f => new TimeRange(f.s, f.e)));
				}
				else
				{
					resultPeriods.AddAll(user.Fs.Select(f => new TimeRange(f.s, f.e)));
					if (resultPeriods.Any())
					{
						var intersectedPeriods = periodIntersector.IntersectPeriods(resultPeriods, false);
						resultPeriods.Clear();
						resultPeriods.AddAll(intersectedPeriods.Select(p => new TimeRange(p.Start, p.End)));
					}
				}
				
				 
			}
		
			Console.WriteLine("...共同的時間...");
			foreach (var intersectedPeriod in resultPeriods)
			{
				Console.WriteLine("所有人共同的的時間 Period: " + intersectedPeriod);
			}

			//對應到會議室
			var crs = wkHours.Users.Where(u => crEmails.Contains(u.id));
			 
			foreach (var cr in crList)
			{
				var email = cr.AttributesList.Where(ar => ar.Key.Equals("email"))
						.Select(ar => ar.Value)
						.FirstOrDefault();
				var fullName = cr.AttributesList.Where(ar => ar.Key.Equals("fullName"))
						.Select(ar => ar.Value)
						.FirstOrDefault();

				Console.WriteLine($"{fullName}:{email}");
				var crHour = wkHours.Users.FirstOrDefault(u => u.id.Equals(email));
				if (crHour != null)
				{
					var crPeriods = new TimePeriodCollection();
					crPeriods.AddAll(resultPeriods.Select(p => new TimeRange(p.Start, p.End)));
					crPeriods.AddAll(crHour.Fs.Select(f => new TimeRange(f.s, f.e)));
					var intersectedPeriods = periodIntersector.IntersectPeriods(crPeriods, false);
					Console.WriteLine($"{fullName}:{email}...共同的時間...");
					foreach (var intersectedPeriod in intersectedPeriods)
					{
						Console.WriteLine("    Period: " + intersectedPeriod);
					}
				}
			}
		}

	}

	
}


[TestMethod]
[Description("透過 Email 取得分享的行事曆，並加入掛載到自已身上 ")]
public void CreateMountpointRequestTest()
{
	var ownerEmail = "perry_chang@gss.com.tw";
	var getShareInfoRequest = new GetShareInfoRequest(ownerEmail);

	ZmailRequest.ApiRequest = getShareInfoRequest;
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as GetShareInfoResponse;
	var mid = resp.MountpointId;
	var ownerId = resp.OwnerId;
	if (string.IsNullOrWhiteSpace(mid))
	{
		var ownerName = $"{resp.OwnerName}'s Calendar";
		var req = new CreateMountpointRequest();
		req.OwnerId = ownerId;
		req.MountDisplayName = ownerName;

		ZmailRequest.ApiRequest = req;
		zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
		var resp2 = zResquest.ApiResponse as CreateMountpointResponse;
		Console.WriteLine($"是否成功? {resp2.CreateSuccess}");
	}

}

[TestMethod]
[Description("透過關鍵字尋找 Email 資訊")]
public void AutoCompleteRequestTest()
{
	var searchString = "rai";
	ZmailRequest.ApiRequest = new AutoCompleteRequest(searchString);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as AutoCompleteResponse;
	var matchList = resp.MatchList;
	foreach (var match in matchList)
	{
		Console.WriteLine($"name:{match.DisplayName}, email:{match.Email} ");
	}
	 
}

[TestMethod]
[Description("新增行事曆，訂會議室 及 公務車, 還有其他人")]
public void CreateAppointmentRequestBookTestWithResource()
{
	var app = new AppointmentRequestParams();
	app.Subject = " 到小琉球玩 ";
	app.StartDate = new DateTime(2017, 6, 4, 14, 0, 0);
	app.EndDate = new DateTime(2017, 6, 4, 15, 00, 0);
	app.Organizer = new Attendee { DisplayName = "RM", Email = "rainmaker_ho@gss.com.tw" };
	app.Locations = new List<Attendee>{
		new Attendee{ DisplayName = "舞蝶館",Email = "room_xz_01@gss.com.tw"}
	};
	app.Attendees = new List<Attendee>{

		new Attendee { Email = "jr_yang@gss.com.tw", DisplayName = "丸子姐" } 
	};

	app.Resources = new List<Attendee>{
		new Attendee { Email = "car_toyota_altis_g2@gss.com.tw", DisplayName = "公務車-玩命關頭" }
	};

	ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
	var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
	var resp = zResquest.ApiResponse as CreateAppointmentResponse;
	var appResp = resp?.AppointmentResponse;
	Console.WriteLine($"{appResp?.InviteMessageId}");
}

```

* 測試版本為 7.2.4

參考資訊
[ZimbraTM SOAP API Reference 8.0.0_GA_5424](https://files.zimbra.com/docs/soap_api/8.0/soapapi-zimbra-doc/api-reference/overview-summary.html)
[Zimbra Toaster](https://sourceforge.net/projects/zimbratoaster/files/Windows/)



