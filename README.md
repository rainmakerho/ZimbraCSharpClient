# ZimbraCSharpClient

從 [Zimbra](https://sourceforge.net/p/zimbra/code/HEAD/tree/)  的 ZimbraCSharpClient 加入其他的功能 Call Zimbra SOAP

- Zimbra.Client 為 Zimbra 的 .NET 專案
目前加入功能如下，

| 功能 | 說明 | Services |
| ------ | ------ | ------ |
| SearchCalendarResourcesRequest | 找資源，例如會議室、車子 ... | Account |
| GetWorkingHoursRequest | GetWorkingHoursRequest | Mail |
| GetFreeBusy | 取得每個人的Free or Busy 時間 | Mail |
| CreateAppointment | Book 會議室 | Mail |
 
- zimbraClientTest 為測試 Demo 的專案

```C#
class Program
{
	private static Dispatcher _ZmailDispatcher;
	private static RequestEnvelope _ZmailRequest;
	static void Main(string[] args)
	{
		//設定連接的Server
		InitDispatcher();

		//透過使用者帳密，取回Token
		var userId = "rm@rmtech.com.tw";
		var pwd = "your zimbra password";
		GetToken(userId, pwd);

		//_ZmailRequest.ApiRequest = new GetFolderRequest();
		//var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);

		//取得某些人的行事歷時間
		//var start = new DateTime(2017, 3, 28);
		//var end = new DateTime(2017, 3, 29);
		//var searchNames = "p1@rmtech.com.tw,p2@rmtech.com.tw";
		//_ZmailRequest.ApiRequest = new GetWorkingHoursRequest(start, end, searchNames);
		//var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);
		//資料會在 zResquest.ApiResponse 之中

		//get Free or Busy
		//var start = new DateTime(2017, 3, 28, 8, 0, 0);
		//var end = new DateTime(2017, 3, 29, 20, 0, 0);
		//var searchNames = "rm@rmtech.com.tw";
		//_ZmailRequest.ApiRequest = new GetFreeBusyRequest(start, end, searchNames);
		//var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);
		//資料會在 zResquest.ApiResponse 之中
		
		//Search 會議室
		//var searchMeetingRooms = new SearchCalendarResourcesRequest("email,fullName");
		//searchMeetingRooms.ProcToXmlDocument = (conditionFilters) =>
		//{
		//    var condE = conditionFilters.OwnerDocument.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
		//    condE.SetAttribute(AccountService.A_ATTR, AccountService.V_ATTR_TYPE);
		//    condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE);
		//    condE.SetAttribute(AccountService.A_VALUE, AccountService.V_LOCATION);
		//    conditionFilters.AppendChild(condE);
		//};
		//_ZmailRequest.ApiRequest = searchMeetingRooms;
		//var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);
		//資料會在 zResquest.ApiResponse 之中

		//訂會議
		var app = new Appointment();
		app.Subject = "RM 測試主旨";
		app.Body = @"這是Body
行一
行2
行3
		";
		app.StartDate = new DateTime(2017, 4, 3, 15, 0, 0);
		app.EndDate = new DateTime(2017, 4, 3, 15, 30, 0);
		app.Organizer = new Attendee {Email = "rm@rmtech.com.tw", DisplayName = "Rainmaker Ho"};
		app.Location = new Attendee { Email = "meetingroom@rmtech.com.tw", DisplayName = "舞蝶館" };
		app.Attendees = new List<Attendee>()
		{
			new Attendee {Email = "alice_lai@rmtech.com.tw", DisplayName = "Alice Lai"}
		};

		_ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
		var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);
		//資料會在 zResquest.ApiResponse 之中
		
	}

	static void InitDispatcher()
	{
		_ZmailDispatcher = new Dispatcher("zmail.rmtech.com.tw", 443, true, true);
	}

	static void GetToken(string userId, string pwd)
	{
		var zRequestContext = new RequestContext();
		var zAuthRequest = new AuthRequest(userId, pwd);
		_ZmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
		 
		var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);
		//var zAuthToken = (zResquest.ApiResponse as AuthResponse)?.AuthToken;
		_ZmailRequest.Context.Update(zResquest.Context, (zResquest.ApiResponse as AuthResponse));
	}
}
```

* 測試版本為 7.2.4

參考資訊
[ZimbraTM SOAP API Reference 8.0.0_GA_5424](https://files.zimbra.com/docs/soap_api/8.0/soapapi-zimbra-doc/api-reference/overview-summary.html)
[Zimbra Toaster](https://sourceforge.net/projects/zimbratoaster/files/Windows/)
