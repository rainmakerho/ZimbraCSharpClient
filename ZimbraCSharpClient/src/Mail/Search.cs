using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class SearchRequest : MailServiceRequest
    {
        private SearchRequestParams _searchParams = null;

        public SearchRequest()
        {}

        public SearchRequest(SearchRequestParams  searchParams)
        {
            _searchParams = searchParams;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.SEARCH_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();

            XmlElement reqElem = doc.CreateElement(MailService.SEARCH_REQUEST, MailService.NAMESPACE_URI);

            Int64 gmtStartMillis = DateUtil.DateTimeToGmtMillis(_searchParams.LocalStart);
            Int64 gmtEndMillis = DateUtil.DateTimeToGmtMillis(_searchParams.LocalEnd);

            reqElem.SetAttribute(MailService.A_CAL_EXPAND_INST_START, gmtStartMillis.ToString());
            reqElem.SetAttribute(MailService.A_CAL_EXPAND_INST_END, gmtEndMillis.ToString());
            //conversation|message|contact|appointment|task|wiki|document 
            reqElem.SetAttribute(MailService.A_TYPES, _searchParams.Type.ToString(MailService.EnumToStringFormat));
            reqElem.SetAttribute(MailService.A_SORT_BY, _searchParams.SortBy.ToString(MailService.EnumToStringFormat));

            var qryElem = doc.CreateElement(MailService.E_QUERY, MailService.NAMESPACE_URI);
            qryElem.InnerText = $"inid:{_searchParams.FolderId}";
            reqElem.AppendChild(qryElem);

            doc.AppendChild(reqElem);
            return doc;
        }
    }


    public class SearchResponse : Response
    {
        public List<SearchResult> Appointments = null;
        public SearchResponse()
        {}

        public SearchResponse(List<SearchResult> appts)
        {
            Appointments = appts;
        }

        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.SEARCH_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var appts = new List<SearchResult>();
            var apptNodes = responseNode.SelectNodes(MailService.NS_PREFIX + ":" + MailService.E_APPOINTMENT, XmlUtil.NamespaceManager);
            foreach (XmlNode apptNode in apptNodes)
            {
                var appt = new SearchResult();
                appt.Name = XmlUtil.AttributeValue(apptNode.Attributes, MailService.A_NAME);
                appt.Location = XmlUtil.AttributeValue(apptNode.Attributes, MailService.A_LOCATION);
                var instNode = apptNode.SelectSingleNode(MailService.NS_PREFIX + ":" + MailService.E_INSTANCE, XmlUtil.NamespaceManager);
                if (instNode != null)
                {
                    String s = XmlUtil.AttributeValue(instNode.Attributes, MailService.A_START);
                    Int64 seconds = Int64.Parse(s);
                    appt.StartTime = DateUtil.GmtSecondsToLocalTime(seconds);
                }
                appt.ModifySequence = XmlUtil.AttributeValue(apptNode.Attributes, MailService.A_MODIFY_SEQUENCE);
                appt.InviteMessageId = XmlUtil.AttributeValue(apptNode.Attributes, MailService.A_INV_ID);

                var orNode = apptNode.SelectSingleNode(MailService.NS_PREFIX + ":" + MailService.E_ORGANIZER, XmlUtil.NamespaceManager);
                if (orNode != null)
                {
                    appt.Organizer = new Attendee
                    {
                        DisplayName = XmlUtil.AttributeValue(orNode.Attributes, MailService.A_DISPLAY_NAME),
                        Email = XmlUtil.AttributeValue(orNode.Attributes, MailService.A_EMAIL)
                    };
                }
                appts.Add(appt);
            }
            return new SearchResponse(appts);
        }
    }

    public class SearchResult
    {
         

        public string InviteMessageId { get; set; }

        public string ModifySequence { get; set; }

        public string Name { get; set; }

        public string Revision { get; set; }

        public string Location { get; set; }

        public DateTime StartTime { get; set; }

        public Attendee Organizer { get; set; }

        

    }

    public class SearchRequestParams
    {
        public enum EmuSortBy
        {
            none = -1,
            dateAsc,
            dateDesc,
            subjAsc,
            subjDesc,
            nameAsc,
            nameDesc,
            rcptAsc,
            rcptDesc,
            attachAsc,
            attachDesc,
            flagAsc,
            flagDesc,
            priorityAsc,
            priorityDesc
        }

        public enum EmuTypes
        {
            conversation,
            message,
            contact,
            appointment,
            task,
            wiki,
            document
        }
        
        public SearchRequestParams()
        {
            this.FolderId = MailService.V_METTING_PARENT_FOLDER_ID;
            this.SortBy = EmuSortBy.dateAsc;
            this.Type = EmuTypes.appointment;
        }
        public DateTime LocalStart { get; set; }

        public DateTime LocalEnd { get; set; }

        public string FolderId { get; set; }

        public EmuSortBy SortBy { get; set; }

        public EmuTypes Type { get; set; }
    }
}
