using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zimbra.Client.Account;
using Zimbra.Client.src.Account;

namespace Zimbra.Client.Test
{
    [TestClass]
    public class AccountTests
    {
        public static string ZmailServer;
        public static int ZmailServerPort;
        public static string UserId;
        public static string Pwd;

        public static Dispatcher ZmailDispatcher;
        public static RequestEnvelope ZmailRequest;

        [TestInitialize()]
        public void Initialize()
        {
            ZmailServer = "zmail.gss.com.tw";
            ZmailServerPort = 443;
            UserId = "rainmaker_ho@gss.com.tw";
            Pwd = "your pwd";
            AssignUserToken();
        }

        public void AssignUserToken()
        {
            ZmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            //設定 Debug 輸出到 Console 
            ZmailDispatcher.SetDebugStream(Console.Out);
            var zRequestContext = new RequestContext();
            var zAuthRequest = new AuthRequest(UserId, Pwd);
            ZmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
            var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            ZmailRequest.Context.Update(zResquest.Context, (zResquest.ApiResponse as AuthResponse));

        }

        [TestMethod]
        [ExpectedException(typeof(ZimbraException), "使用者/密碼錯誤")]
        public void AuthRequesWithFailUserOrPwdtTest()
        {
            Pwd = "@@@@@@@";            
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            var zAuthRequest = new AuthRequest(UserId, Pwd);
            var zmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
            var zResquest = zmailDispatcher.SendRequest(zmailRequest);
        }

        [TestMethod]
        [Description("取得 Token ")]
        public string AuthRequesTest()
        {
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            var zAuthRequest = new AuthRequest(UserId, Pwd);
            var zmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
            var zResquest = zmailDispatcher.SendRequest(zmailRequest);
            var zAuthToken = (zResquest.ApiResponse as AuthResponse)?.AuthToken;
            Console.WriteLine(zAuthToken);
            Console.WriteLine((zResquest.ApiResponse as AuthResponse)?.LifeTime);
            Assert.IsTrue($"{zAuthToken}" != string.Empty);
            return zAuthToken;
        }

        [TestMethod]
        [Description("取得 Token ")]
        public void AuthRequesTest2()
        {
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            var zAuthRequest = new AuthRequest(UserId, Pwd);
            var zmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
            var zResquest = zmailDispatcher.SendRequest(zmailRequest);
            var zAuthToken = (zResquest.ApiResponse as AuthResponse)?.AuthToken;
            Console.WriteLine(zAuthToken);
            Console.WriteLine((zResquest.ApiResponse as AuthResponse)?.LifeTime);
            Assert.IsTrue($"{zAuthToken}" != string.Empty);
            
        }

        [TestMethod]
        [Description("傳入 token 驗證 token 的有效性")]
        [ExpectedException(typeof(ZimbraException), "auth credentials have expired")]
        public void AuthRequesTestTokenValidate()
        {
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            string token = "0_83bbf557b587c222bb534fe06e3b70644809d652_69643d33363a61666464306130302d663739332d343737652d626662632d3136353938373536633038623b6578703d31333a313439313132353035353033363b747970653d363a7a696d6272613b";
            var zAuthRequest = new AuthRequest(token);
            var zmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
            var zResquest = zmailDispatcher.SendRequest(zmailRequest);
            var zAuthToken = (zResquest.ApiResponse as AuthResponse)?.AuthToken;
            Console.WriteLine(zAuthToken);
            Console.WriteLine((zResquest.ApiResponse as AuthResponse)?.LifeTime);
            Assert.IsTrue($"{zAuthToken}" != string.Empty);
        }

        [TestMethod]
        [Description("依Token去Request")]
        public void AuthRequesFromTokenTest()
        {
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            var token = AuthRequesTest();
            zRequestContext.AuthToken = token;
            //測試使用 Token
            var calendarAttributes ="fullName,email";
            var searchMeetingRooms = new SearchCalendarResourcesRequest(calendarAttributes);
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
        [Description("取得使用者分享的 Folder 資訊，在gss是 folderPath=/Calendar ")]
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
    }
}
