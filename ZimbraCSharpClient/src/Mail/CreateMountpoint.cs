using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Zimbra.Client.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Mail
{
    public class CreateMountpointRequest : MailServiceRequest
    {

        public string OwnerId { get; set; }
        public string MountDisplayName { get; set; }

        public CreateMountpointRequest()
        {}

        public CreateMountpointRequest(string ownerId, string mountDisplayName)
        {
            OwnerId = ownerId;
            MountDisplayName = mountDisplayName;
        }

        public override String Name()
        {
            return MailService.NS_PREFIX + ":" + MailService.CREATE_MOUNTPOINT_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(MailService.CREATE_MOUNTPOINT_REQUEST, MailService.NAMESPACE_URI);
            XmlElement linkElem = doc.CreateElement(MailService.E_LINK, MailService.NAMESPACE_URI);
            linkElem.SetAttribute(MailService.A_NAME, MountDisplayName);
            linkElem.SetAttribute(MailService.A_VIEW, SearchRequestParams.EmuTypes.appointment.ToString(MailService.EnumToStringFormat));
            linkElem.SetAttribute(MailService.A_ZIMBRA_ID, OwnerId);
            linkElem.SetAttribute(MailService.A_PARENT_FOLDER_ID, "1");
            linkElem.SetAttribute(MailService.A_REMOTE_ID, MailService.V_METTING_PARENT_FOLDER_ID);
            reqElem.AppendChild(linkElem);
            doc.AppendChild(reqElem);
            return doc;
        }
 
    }

    public class CreateMountpointResponse : Response
    {
        public bool CreateSuccess { get; set; }
        public CreateMountpointResponse()
        { }


        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.CREATE_MOUNTPOINT_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var res = new CreateMountpointResponse();
            res.CreateSuccess = (responseNode.ChildNodes.Count > 0);
            res.ResponseNode = responseNode;
            return res;
        }
    }
}
