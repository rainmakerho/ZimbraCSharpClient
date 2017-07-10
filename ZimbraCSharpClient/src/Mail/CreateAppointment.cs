using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Zimbra.Client.Account;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;


namespace Zimbra.Client.src.Mail
{
    public class CreateAppointmentRequest : MailServiceRequest
    {
        private AppointmentRequestParams appointment;

        

        public CreateAppointmentRequest()
        {}

        public CreateAppointmentRequest(AppointmentRequestParams app)
        {
            appointment = app;
            if (appointment.Attendees == null) appointment.Attendees = new List<Attendee>();
        }


        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.CREATE_APPT_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            if (appointment.Locations == null) appointment.Locations = new List<Attendee>();

            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.CREATE_APPT_REQUEST, MailService.NAMESPACE_URI);
            
             
            var m = doc.CreateElement(MailService.E_MESSAGE, MailService.NAMESPACE_URI);
            m.SetAttribute(MailService.A_PARENT_FOLDER_ID, MailService.V_METTING_PARENT_FOLDER_ID);
            var inv = doc.CreateElement(MailService.E_INV, MailService.NAMESPACE_URI);
            var comp = doc.CreateElement(MailService.E_COMP, MailService.NAMESPACE_URI);
            comp.SetAttribute(MailService.A_STATUS, MailService.V_STATUS_CONF);
            comp.SetAttribute(MailService.A_FREE_BUSY_STATUS, MailService.V_FB_BUSY);
            comp.SetAttribute(MailService.A_CLASS, MailService.V_APPT_CLASS_PUB);
            comp.SetAttribute(MailService.A_TRANSP, MailService.V_TRANSP_OPAQUE);
            comp.SetAttribute(MailService.A_DRAFT, MailService.V_ZERO);
            comp.SetAttribute(MailService.A_APPT_ALLDAY, MailService.V_ZERO);
            comp.SetAttribute(MailService.A_NAME, appointment.Subject);
           
                var locations = appointment.Locations.Aggregate<Attendee, string>(string.Empty,
                (x, y) =>
                {
                    var init = x.Length > 0 ? x + MailService.SEMICOLON : x;
                    init += $"\"{y.DisplayName}\" <{y.Email}>";
                    return init;
                });
            
            comp.SetAttribute(MailService.A_LOCATION, $"{locations}" );

             
                //會議室 Location
                foreach (var attendee in appointment.Locations)
                {
                    var loc = doc.CreateElement(MailService.E_ATTENDEES, MailService.NAMESPACE_URI);
                    loc.SetAttribute(MailService.A_ROLE, MailService.V_ROLE_NON);
                    loc.SetAttribute(MailService.A_PARTICIPATION_STATUS, MailService.V_PARTICIPATION_STATUS_NE);
                    loc.SetAttribute(MailService.A_RSVP, MailService.V_TRUE);
                    loc.SetAttribute(MailService.A_EMAIL, attendee.Email);
                    loc.SetAttribute(MailService.A_DISPLAY_NAME, attendee.DisplayName);
                    loc.SetAttribute(MailService.A_CUTYPE, MailService.V_CUTYPE_RES);
                    comp.AppendChild(loc);
                }  
            
            

            //OR
            var or = doc.CreateElement(MailService.E_ATTENDEES, MailService.NAMESPACE_URI);
            or.SetAttribute(MailService.A_ROLE, MailService.V_ROLE_REQ);
            or.SetAttribute(MailService.A_PARTICIPATION_STATUS, MailService.V_PARTICIPATION_STATUS_NE);
            or.SetAttribute(MailService.A_RSVP, MailService.V_TRUE);
            or.SetAttribute(MailService.A_EMAIL, appointment.Organizer.Email);
            or.SetAttribute(MailService.A_DISPLAY_NAME, appointment.Organizer.DisplayName);
            comp.AppendChild(or);

            //參與人員 
            foreach (var attendee in appointment.Attendees)
            {
                var at = doc.CreateElement(MailService.E_ATTENDEES, MailService.NAMESPACE_URI);
                at.SetAttribute(MailService.A_ROLE, MailService.V_ROLE_REQ);
                at.SetAttribute(MailService.A_PARTICIPATION_STATUS, MailService.V_PARTICIPATION_STATUS_NE);
                at.SetAttribute(MailService.A_RSVP, MailService.V_TRUE);
                at.SetAttribute(MailService.A_EMAIL, attendee.Email);
                at.SetAttribute(MailService.A_DISPLAY_NAME, attendee.DisplayName);
                at.SetAttribute(MailService.A_CUTYPE, MailService.V_CUTYPE_RES);
                comp.AppendChild(at);
            }

            //Resource 車、投影機 ...
            foreach (var resource in appointment.Resources)
            {
                var at = doc.CreateElement(MailService.E_ATTENDEES, MailService.NAMESPACE_URI);
                at.SetAttribute(MailService.A_ROLE, MailService.V_ROLE_NON);
                at.SetAttribute(MailService.A_PARTICIPATION_STATUS, MailService.V_PARTICIPATION_STATUS_NE);
                at.SetAttribute(MailService.A_RSVP, MailService.V_TRUE);
                at.SetAttribute(MailService.A_EMAIL, resource.Email);
                at.SetAttribute(MailService.A_DISPLAY_NAME, resource.DisplayName);
                comp.AppendChild(at);
            }


            var start = appointment.StartDate;
            var s = doc.CreateElement(MailService.E_START, MailService.NAMESPACE_URI);
            s.SetAttribute(MailService.A_TIMEZONE, MailService.V_TIMEZONE_TAIPEI);
            s.SetAttribute(MailService.A_DATE, start.ToString(MailService.DateTimeFormat));
            comp.AppendChild(s);


            var end = appointment.EndDate;
            var e = doc.CreateElement(MailService.E_END, MailService.NAMESPACE_URI);
            e.SetAttribute(MailService.A_TIMEZONE, MailService.V_TIMEZONE_TAIPEI);
            e.SetAttribute(MailService.A_DATE, end.ToString(MailService.DateTimeFormat));
            comp.AppendChild(e);

            or = doc.CreateElement(MailService.E_ORGANIZER, MailService.NAMESPACE_URI);
            or.SetAttribute(MailService.A_EMAIL, appointment.Organizer.Email);
            or.SetAttribute(MailService.A_DISPLAY_NAME, appointment.Organizer.DisplayName);
            comp.AppendChild(or);


            var alarm = doc.CreateElement(MailService.E_ALARM, MailService.NAMESPACE_URI);
            alarm.SetAttribute(MailService.A_ACTION, MailService.V_ACTION_DISPLAY);

            var trigger = doc.CreateElement(MailService.E_TRIGGER, MailService.NAMESPACE_URI);
            var rel = doc.CreateElement(MailService.E_RELATIVE, MailService.NAMESPACE_URI);
            rel.SetAttribute(MailService.A_MINUTES, appointment.AlarmMinutes.ToString());
            rel.SetAttribute(MailService.A_RELATED,MailService.V_RELATED_START);
            rel.SetAttribute(MailService.A_NEGATIVE, MailService.V_TRUE);
            trigger.AppendChild(rel);
            alarm.AppendChild(trigger);
            comp.AppendChild(alarm);

            inv.AppendChild(comp);

            m.AppendChild(inv);

            //參與人員 
            foreach (var attendee in appointment?.Attendees)
            {
                var at = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
                at.SetAttribute(MailService.A_EMAIL, attendee.Email);
                at.SetAttribute(MailService.A_NAME_PART, attendee.DisplayName);
                at.SetAttribute(MailService.A_TYPE, MailService.V_TO);
                m.AppendChild(at);
            }

            //Resource 車、投影機 ...
            foreach (var resource in appointment?.Resources)
            {
                var at = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
                at.SetAttribute(MailService.A_EMAIL, resource.Email);
                at.SetAttribute(MailService.A_NAME_PART, resource.DisplayName);
                at.SetAttribute(MailService.A_TYPE, MailService.V_TO);
                m.AppendChild(at);
            }

            or = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
            or.SetAttribute(MailService.A_EMAIL, appointment.Organizer.Email);
            or.SetAttribute(MailService.A_DISPLAY_NAME, appointment.Organizer.DisplayName);
            or.SetAttribute(MailService.A_TYPE, MailService.V_TO);
            m.AppendChild(or);

             
            foreach (var attendee in appointment.Locations)
            {
                var loc = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
                loc.SetAttribute(MailService.A_EMAIL, attendee.Email);
                loc.SetAttribute(MailService.A_NAME_PART, attendee.DisplayName);
                loc.SetAttribute(MailService.A_TYPE, MailService.V_TO);
                m.AppendChild(loc);
            }
            

            var su = doc.CreateElement(MailService.E_SUBJECT, MailService.NAMESPACE_URI);
            su.InnerText = appointment.Subject;
            m.AppendChild(su);

            var mp = doc.CreateElement(MailService.E_MIME_PART, MailService.NAMESPACE_URI);
            mp.SetAttribute(MailService.A_CONTENT_TYPE, MailService.V_CT);

            var mpPlain = doc.CreateElement(MailService.E_MIME_PART, MailService.NAMESPACE_URI);
            mpPlain.SetAttribute(MailService.A_CONTENT_TYPE, MailService.V_TEXT_PLAIN);
            var contentPlain = doc.CreateElement(MailService.E_CONTENT, MailService.NAMESPACE_URI);
            var contentString = new StringBuilder();
            contentString.AppendLine("以下為新會議請求：");
            contentString.AppendLine($"主題﹕ {appointment.Subject}");
            contentString.AppendLine($"組織者: {appointment.Organizer.DisplayName} <{appointment.Organizer.Email}>");
            contentString.AppendLine("");
            if (!string.IsNullOrWhiteSpace(locations))
            {
                contentString.AppendLine($"地點﹕{locations} ");
            }

            if (appointment.Resources.Any())
            {
                var resources = appointment.Resources.Aggregate<Attendee, string>(string.Empty,
                    (x, y) => x + (x.Length > 0 ? MailService.SEMICOLON + y.DisplayName + " <" + y.Email + "> " : y.DisplayName + " <" + y.Email + "> "));
                contentString.AppendLine($"資源: {resources}");
            }
            
            contentString.AppendLine($"時間: {appointment.StartDate} To {appointment.EndDate}");

            if (appointment.Attendees.Any())
            {
                var attendees = appointment.Attendees.Aggregate<Attendee, string>(string.Empty,
                    (x, y) => x + (x.Length > 0 ? MailService.SEMICOLON + y.DisplayName + " <" + y.Email + "> " : y.DisplayName + " <" + y.Email + "> "));

                contentString.AppendLine($"受邀人: {attendees}");
            }
            
            contentString.AppendLine("*~*~*~*~*~*~*~*~*~*" + Environment.NewLine + Environment.NewLine);
            contentString.AppendLine($"{appointment.Body}");

            contentPlain.InnerText = contentString.ToString();
            mpPlain.AppendChild(contentPlain);
            mp.AppendChild(mpPlain);
            m.AppendChild(mp);

            reqElem.AppendChild(m);

            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class CreateAppointmentResponse : Response
    {
        public  AppointmentResponse AppointmentResponse;
        public CreateAppointmentResponse()
        {}

        public CreateAppointmentResponse(AppointmentResponse appRes)
        {
            AppointmentResponse = appRes;
        }
        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.CREATE_APPT_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var calItemId = XmlUtil.AttributeValue(responseNode.Attributes, MailService.A_CAL_ITEM_ID);
            var inviteMessageId = XmlUtil.AttributeValue(responseNode.Attributes, MailService.A_INV_ID);
            var appResponse = new AppointmentResponse {Id = calItemId, InviteMessageId = inviteMessageId};
            
            var res = new CreateAppointmentResponse(appResponse);
            res.ResponseNode = responseNode;
            return res;
        }
    }

    public class AppointmentResponse
    {
        //對應到 calItemId
        public string Id { get; set; }
       
        public string InviteMessageId { get; set; }
    }

    public class Attendee
    {
        public string Email { get; set; }
        
        public string DisplayName { get; set; }

        //Calendar user type
        public string UserType { get; set; }
    }

    public class AppointmentRequestParams
    {

        public AppointmentRequestParams()
        {
            this.Timezone = MailService.V_TIMEZONE_TAIPEI;
            this.AlarmMinutes = 5;
        }

        public Attendee Organizer { get; set; }

        public List<Attendee> Locations { get; set; }

        public List<Attendee> Attendees { get; set; }


        public List<Attendee> Resources { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Timezone { get; set; }

        public int AlarmMinutes { get; set; }
    }
}
