using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zimbra.Client;
using Zimbra.Client.Account;
using Zimbra.Client.Mail;
using Zimbra.Client.src.Account;
using Zimbra.Client.src.Mail;
using Zimbra.Client.Util;

//https://files.zimbra.com/docs/soap_api/8.0/soapapi-zimbra-doc/api-reference/index.html
namespace zimbraClientTest
{
    class Program
    {
        private static Dispatcher _ZmailDispatcher;
        private static RequestEnvelope _ZmailRequest;
        static void Main(string[] args)
        {
            //設定連接的Server
            InitDispatcher();

            //透過使用者帳密，取回Token
            var userId = "rainmaker_ho@gss.com.tw";
            var pwd = "your pwd";
            GetToken(userId, pwd);

            //_ZmailRequest.ApiRequest = new GetFolderRequest();
            //var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);

            //get workinghours
            //var start = new DateTime(2017, 3, 28);
            //var end = new DateTime(2017, 3, 29);
            //var searchNames = "sk@gss.com.tw,ra@gss.com.tw";
            //_ZmailRequest.ApiRequest = new GetWorkingHoursRequest(start, end, searchNames);
            //var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);


            //get Free or Busy
            //var start = new DateTime(2017, 3, 28, 8, 0, 0);
            //var end = new DateTime(2017, 3, 29, 20, 0, 0);
            //var searchNames = "rainmaker_ho@gss.com.tw";
            //_ZmailRequest.ApiRequest = new GetFreeBusyRequest(start, end, searchNames);
            //var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);



            //取得會議室
            //var searchMeetingRooms = new SearchCalendarResourcesRequest("email,fullName");
            //searchMeetingRooms.ProcToXmlDocument = (conditionFilters) =>
            //{
            //    var condE = conditionFilters.OwnerDocument.CreateElement(AccountService.E_COND, AccountService.NAMESPACE_URI);
            //    condE.SetAttribute(AccountService.A_ATTR, AccountService.V_ATTR_TYPE);
            //    condE.SetAttribute(AccountService.A_OP, AccountService.A_OP_TYPE_EQ);
            //    condE.SetAttribute(AccountService.A_VALUE, AccountService.V_LOCATION);
            //    conditionFilters.AppendChild(condE);
            //};
            //_ZmailRequest.ApiRequest = searchMeetingRooms;
            //var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);


            //訂會議
            //var app = new AppointmentRequestParams();
            //app.Subject = "到 一銀  Support";
            ////app.Body = @"這是Body
            ////行一
            ////行2
            ////行3
            ////            ";
            //app.StartDate = new DateTime(2017, 4, 7, 15, 30, 0);
            //app.EndDate = new DateTime(2017, 4, 7, 16, 30, 0);
            //app.Organizer = new Attendee { Email = "rainmaker_ho@gss.com.tw", DisplayName = "Rainmaker Ho" };
            //app.Locations = new List<Attendee>()
            //{
            //    //new Attendee {Email ="room_xz_01@gss.com.tw", DisplayName =  "協志會議室-舞蝶館"},
            //    //new Attendee {Email ="room_xz_02@gss.com.tw", DisplayName =  "協志會議室-天空農場"},
            //};
            //app.Attendees = new List<Attendee>()
            //            {
            //               // new Attendee {Email = "alice_lai@gss.com.tw", DisplayName = "Alice Lai"}
            //            };

            //_ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
            //var zResquestx = _ZmailDispatcher.SendRequest(_ZmailRequest);


            //取得會議室的資訊
            var sdate = new DateTime(2017, 4, 7, 08, 0, 0);
            var edate = new DateTime(2017, 4, 7, 18, 0, 0);
            var searchReqParams = new SearchRequestParams();
            searchReqParams.LocalEnd = edate;
            searchReqParams.LocalStart = sdate;
            _ZmailRequest.ApiRequest = new SearchRequest(searchReqParams);
            var zResquest = _ZmailDispatcher.SendRequest(_ZmailRequest);

            //取出會議室的資訊
            var searchRes = zResquest.ApiResponse as SearchResponse;
            var appts = searchRes.Appointments;
            foreach (var appt in appts)
            {
                Console.WriteLine($"{appt.Name}, OR:{appt.Organizer.DisplayName}, {appt.Organizer.Email}");
                _ZmailRequest.ApiRequest = new GetMsgRequest(appt.InviteMessageId);
                var msgRequest = _ZmailDispatcher.SendRequest(_ZmailRequest);
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

        static void InitDispatcher()
        {
            _ZmailDispatcher = new Dispatcher("zmail.gss.com.tw", 443, true, true);
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
}
