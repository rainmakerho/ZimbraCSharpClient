using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Zimbra.Client.Account;
using Zimbra.Client.Util;

namespace Zimbra.Client.src.Account
{
    public class GetShareInfoRequest : AccountServiceRequest
    {
        public string OwnerEmail { get; set; }

        public GetShareInfoRequest()
        {}

        public GetShareInfoRequest(string ownerEmail)
        {
            OwnerEmail = ownerEmail;
        }

        public override String Name()
        {
            return AccountService.NS_PREFIX + ":" + AccountService.GET_SHARE_INFO_REQUEST;
        }

        public override XmlDocument ToXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement reqElem = doc.CreateElement(AccountService.GET_SHARE_INFO_REQUEST, AccountService.NAMESPACE_URI);
            XmlElement ownerElem = doc.CreateElement(AccountService.E_OWNER, AccountService.NAMESPACE_URI);
            ownerElem.SetAttribute(AccountService.A_BY, AccountService.V_ATTR_NAME);
            ownerElem.InnerText = OwnerEmail;
            reqElem.AppendChild(ownerElem);
            doc.AppendChild(reqElem);
            return doc;
        }
    }

    public class GetShareInfoResponse:Response
    {
        public string MountpointId { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }

        public GetShareInfoResponse()
        {}

       


        public override String Name
        {
            get { return AccountService.NS_PREFIX + ":" + AccountService.GET_SHARE_INFO_RESPONSE; }
        }


        public override Response NewResponse(XmlNode responseNode)
        {
            var res = new GetShareInfoResponse();
            if (responseNode.ChildNodes.Count > 0)
            {
                var mId = XmlUtil.AttributeValue(responseNode.ChildNodes[0].Attributes, AccountService.A_M_ID);
                var ownerId = XmlUtil.AttributeValue(responseNode.ChildNodes[0].Attributes, AccountService.A_OWNER_ID);
                var ownerName = XmlUtil.AttributeValue(responseNode.ChildNodes[0].Attributes, AccountService.A_OWNER_NAME);
                res.MountpointId = mId;
                res.OwnerId = ownerId;
                res.OwnerName = ownerName;
            }
            res.ResponseNode = responseNode;
            return res;
        }
    }
}
