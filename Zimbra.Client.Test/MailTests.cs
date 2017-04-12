using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zimbra.Client.Account;
using Zimbra.Client.src.Mail;
using System.Linq;
using Zimbra.Client.Mail;

namespace Zimbra.Client.Test
{
    /// <summary>
    /// MailTests 的摘要說明
    /// </summary>
    [TestClass]
    public class MailTests
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
            Pwd = "yourpassword";
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
            app.StartDate = new DateTime(2017, 4, 11, 08, 30, 0);
            app.EndDate = new DateTime(2017, 4, 11, 12, 00, 0);
            app.Organizer = new Attendee { Email = "rainmaker_ho@gss.com.tw", DisplayName = "亂馬客" };
             
            ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
            var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            var resp = zResquest.ApiResponse as CreateAppointmentResponse;
            var appResp = resp?.AppointmentResponse;
            Console.WriteLine($"{appResp?.InviteMessageId}");
        }

        [TestMethod]
        [Description("取消 Book 的 行事曆，外出")]
        public void CancelAppointmentRequestSelfTest()
        {
            var appCancelParam = new CancelAppointmentRequestParam();
            appCancelParam.Subject = "到台灣銀行洽工 ";
            appCancelParam.Body = "取消:到台灣銀行洽工 ";
            appCancelParam.Id = "32326-32325";
            appCancelParam.Attendees = new List<Attendee>
            {
                new Attendee{Email="rainmaker_ho@gss.com.tw"}
            };
            ZmailRequest.ApiRequest = new CancelAppointmentRequest(appCancelParam);
            var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            var resp = zResquest.ApiResponse as CancelAppointmentResponse;
             
        }


        [TestMethod]
        [Description("建立後，立馬取消 Book 的 行事曆，外出")]
        public void CreateAndCancelAppointmentRequestSelfTest()
        {
            var app = new AppointmentRequestParams();
            app.Subject = "到台灣銀行洽工 ";
            app.StartDate = new DateTime(2017, 4, 11, 08, 30, 0);
            app.EndDate = new DateTime(2017, 4, 11, 12, 00, 0);
            app.Organizer = new Attendee { Email = "rainmaker_ho@gss.com.tw", DisplayName = "亂馬客" };

            ZmailRequest.ApiRequest = new CreateAppointmentRequest(app);
            var zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            var resp = zResquest.ApiResponse as CreateAppointmentResponse;
            var appResp = resp?.AppointmentResponse;
            Console.WriteLine($"{appResp?.InviteMessageId}");

            var appCancelParam = new CancelAppointmentRequestParam();
            appCancelParam.Subject = app.Subject;
            appCancelParam.Body = $"取消: {app.Subject}";
            appCancelParam.Id = appResp?.InviteMessageId;
            appCancelParam.Attendees = new List<Attendee>
            {
                app.Organizer
            };
            ZmailRequest.ApiRequest = new CancelAppointmentRequest(appCancelParam);
            zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            var resp2 = zResquest.ApiResponse as CancelAppointmentResponse;
            
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
        [Description("新增&取消行事曆，訂會議室, 自已")]
        public void CreateAndCancelAppointmentRequestSelfBookTest()
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


            var appCancelParam = new CancelAppointmentRequestParam();
            appCancelParam.Subject = app.Subject;
            appCancelParam.Body = $"取消: {app.Subject}";
            appCancelParam.Id = appResp?.InviteMessageId;
            appCancelParam.Attendees = new List<Attendee>
            {
                app.Organizer 
            };
            appCancelParam.Attendees.AddRange(app.Locations);

            ZmailRequest.ApiRequest = new CancelAppointmentRequest(appCancelParam);
            zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            var resp2 = zResquest.ApiResponse as CancelAppointmentResponse;

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
        [Description("新增 & 取消 行事曆，訂會議室, 還有其他人")]
        public void CreateAndCancelAppointmentRequestBookTest()
        {
            var app = new AppointmentRequestParams();
            app.Subject = "開會，新增行事曆，訂會議室  ";
            app.StartDate = new DateTime(2017, 4, 6, 13, 0, 0);
            app.EndDate = new DateTime(2017, 4, 6, 14, 00, 0);
            app.Organizer = new Attendee { DisplayName = "RM", Email = "rainmaker_ho@gss.com.tw" };
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

            var appCancelParam = new CancelAppointmentRequestParam();
            appCancelParam.Subject = app.Subject;
            appCancelParam.Body = $"取消: {app.Subject}";
            appCancelParam.Id = appResp?.InviteMessageId;
            appCancelParam.Attendees = new List<Attendee>
            {
                app.Organizer
            };
            appCancelParam.Attendees.AddRange(app.Locations);
            appCancelParam.Attendees.AddRange(app.Attendees);

            ZmailRequest.ApiRequest = new CancelAppointmentRequest(appCancelParam);
            zResquest = ZmailDispatcher.SendRequest(ZmailRequest);
            var resp2 = zResquest.ApiResponse as CancelAppointmentResponse;
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



    }
}
