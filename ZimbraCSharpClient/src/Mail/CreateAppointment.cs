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
        private Appointment appointment;

        public static string DateTimeFormat = "yyyyMMddTHHmmss";

        public CreateAppointmentRequest()
        {}

        public CreateAppointmentRequest(Appointment app)
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
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.CREATE_APPT_REQUEST, MailService.NAMESPACE_URI);

             
            var m = doc.CreateElement(MailService.E_MESSAGE, MailService.NAMESPACE_URI);
            m.SetAttribute(MailService.A_PARENT_FOLDER_ID, MailService.V_PARENT_FOLDER_ID_10);
            var inv = doc.CreateElement(MailService.E_INV, MailService.NAMESPACE_URI);
            var comp = doc.CreateElement(MailService.E_COMP, MailService.NAMESPACE_URI);
            comp.SetAttribute(MailService.A_STATUS, MailService.V_STATUS_CONF);
            comp.SetAttribute(MailService.A_FREE_BUSY_STATUS, MailService.V_FB_BUSY);
            comp.SetAttribute(MailService.A_CLASS, MailService.V_APPT_CLASS_PUB);
            comp.SetAttribute(MailService.A_TRANSP, MailService.V_TRANSP_OPAQUE);
            comp.SetAttribute(MailService.A_DRAFT, MailService.V_ZERO);
            comp.SetAttribute(MailService.A_APPT_ALLDAY, MailService.V_ZERO);
            comp.SetAttribute(MailService.A_NAME, appointment.Subject);
            comp.SetAttribute(MailService.A_LOCATION, appointment.Location.Email);

            //會議室 Location
            var loc = doc.CreateElement(MailService.E_ATTENDEES, MailService.NAMESPACE_URI);
            loc.SetAttribute(MailService.A_ROLE, MailService.V_ROLE_NON);
            loc.SetAttribute(MailService.A_PARTICIPATION_STATUS, MailService.V_PARTICIPATION_STATUS_NE);
            loc.SetAttribute(MailService.A_RSVP, MailService.V_TRUE);
            loc.SetAttribute(MailService.A_EMAIL, appointment.Location.Email);
            loc.SetAttribute(MailService.A_DISPLAY_NAME, appointment.Location.DisplayName);
            loc.SetAttribute(MailService.A_CUTYPE, MailService.V_CUTYPE_RES);
            comp.AppendChild(loc);

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
                comp.AppendChild(at);
            }




            var start = appointment.StartDate;
            var s = doc.CreateElement(MailService.E_START, MailService.NAMESPACE_URI);
            s.SetAttribute(MailService.A_TIMEZONE, MailService.V_TIMEZONE_TAIPEI);
            s.SetAttribute(MailService.A_DATE, start.ToString(DateTimeFormat));
            comp.AppendChild(s);


            var end = appointment.EndDate;
            var e = doc.CreateElement(MailService.E_END, MailService.NAMESPACE_URI);
            e.SetAttribute(MailService.A_TIMEZONE, MailService.V_TIMEZONE_TAIPEI);
            e.SetAttribute(MailService.A_DATE, end.ToString(DateTimeFormat));
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

            or = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
            or.SetAttribute(MailService.A_EMAIL, appointment.Organizer.Email);
            or.SetAttribute(MailService.A_DISPLAY_NAME, appointment.Organizer.DisplayName);
            or.SetAttribute(MailService.A_TYPE, MailService.V_TO);
            m.AppendChild(or);

            loc = doc.CreateElement(MailService.E_EMAIL, MailService.NAMESPACE_URI);
            loc.SetAttribute(MailService.A_EMAIL, appointment.Location.Email);
            loc.SetAttribute(MailService.A_NAME_PART, appointment.Location.DisplayName);
            loc.SetAttribute(MailService.A_TYPE, MailService.V_TO);
            m.AppendChild(loc);

            


            

            var su = doc.CreateElement(MailService.E_SUBJECT, MailService.NAMESPACE_URI);
            su.InnerText = appointment.Subject;
            m.AppendChild(su);

            var mp = doc.CreateElement(MailService.E_MIME_PART, MailService.NAMESPACE_URI);
            mp.SetAttribute(MailService.A_CONTENT_TYPE, MailService.V_CT);

            var mpPlain = doc.CreateElement(MailService.E_MIME_PART, MailService.NAMESPACE_URI);
            mpPlain.SetAttribute(MailService.A_CONTENT_TYPE, MailService.V_TEXT_PLAIN);
            var contentPlain = doc.CreateElement(MailService.E_CONTENT, MailService.NAMESPACE_URI);
            var contentString = "以下為新會議請求：" + Environment.NewLine +
                                $"主題﹕ {appointment.Subject}" +
                                $"組織者: '{appointment.Organizer.DisplayName}' <{appointment.Organizer.Email}>" +
                                Environment.NewLine +
                                Environment.NewLine +
                                $"地點﹕ '{appointment.Location.DisplayName}' <{appointment.Location.Email}>" +
                                Environment.NewLine +
                                $"資源﹕ '{appointment.Location.DisplayName}' <{appointment.Location.Email}>" +
                                Environment.NewLine +
                                $"時間: {appointment.StartDate} To {appointment.EndDate}" + Environment.NewLine +
                                Environment.NewLine +
                                $"受邀人: {appointment.Attendees.Aggregate<Attendee, string>(string.Empty, (x, y) => x.Length > 0 ? x + ";" : x)}" +
                                Environment.NewLine + Environment.NewLine +
                                "*~*~*~*~*~*~*~*~*~*" + Environment.NewLine + Environment.NewLine +
                                $"{appointment.Body}";

            contentPlain.InnerText = contentString;
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
        private String id;
        public CreateAppointmentResponse()
        {}

        public CreateAppointmentResponse(string id)
        {
            this.id = id;
        }
        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.CREATE_APPT_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var calItemId = XmlUtil.AttributeValue(responseNode.Attributes, MailService.A_CAL_ITEM_ID);
            return new CreateAppointmentResponse(calItemId);
        }
    }

    public class AppointmentResponse
    {
        public string AppointmentId { get; set; }
       
        public string InviteMessageId { get; set; }
    }

    public class Attendee
    {
        public string Email { get; set; }
        
        public string DisplayName { get; set; }
    }

    public class Appointment
    {

        public Appointment()
        {
            this.Timezone = MailService.V_TIMEZONE_TAIPEI;
            this.AlarmMinutes = 5;
        }

        public Attendee Organizer { get; set; }

        public Attendee Location { get; set; }

        public List<Attendee> Attendees { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Timezone { get; set; }

        public int AlarmMinutes { get; set; }
    }
}
