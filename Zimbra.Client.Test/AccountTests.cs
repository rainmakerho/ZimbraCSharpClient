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
        public void AuthRequesTest()
        {
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            var zAuthRequest = new AuthRequest(UserId, Pwd);
            var zmailRequest = new RequestEnvelope(zRequestContext, zAuthRequest);
            var zResquest = zmailDispatcher.SendRequest(zmailRequest);
            var zAuthToken = (zResquest.ApiResponse as AuthResponse)?.AuthToken;
            Console.WriteLine(zAuthToken);

            Assert.IsTrue($"{zAuthToken}" != string.Empty);
        }


        [TestMethod]
        [Description("依Token去Request")]
        public void AuthRequesFromTokenTest()
        {
            var zmailDispatcher = new Dispatcher(ZmailServer, ZmailServerPort, true, true);
            var zRequestContext = new RequestContext();
            var token =
                "0_4a49f1723bbe362456559c61f44025358b4fe1a3_69643d33363a61666464306130302d663739332d343737652d626662632d3136353938373536633038623b6578703d31333a313439323036333138373336373b747970653d363a7a696d6272613b";
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
    }
}
