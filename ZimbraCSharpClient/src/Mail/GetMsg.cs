/*
 * ***** BEGIN LICENSE BLOCK *****
 * Version: MPL 1.1
 * 
 * The contents of this file are subject to the Mozilla Public License
 * Version 1.1 ("License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.zimbra.com/license
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See
 * the License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Original Code is: Zimbra CSharp Client
 * 
 * The Initial Developer of the Original Code is Zimbra, Inc.
 * Portions created by Zimbra are Copyright (C) 2006 Zimbra, Inc.
 * All Rights Reserved.
 * 
 * Contributor(s):
 * 
 * ***** END LICENSE BLOCK *****
 */
using System;
using System.Collections.Generic;
using System.Xml;
using Zimbra.Client.src.Mail;
using Zimbra.Client.Util;

namespace Zimbra.Client.Mail
{

	public class GetMsgRequest : MailServiceRequest
	{
		private String id;

		public GetMsgRequest(String id)
		{
			this.id = id;
		}

		public override String Name()
		{
			return MailService.NS_PREFIX + ":" + MailService.GET_MSG_REQUEST;
		}

		public override XmlDocument ToXmlDocument()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement reqElem =doc.CreateElement( MailService.GET_MSG_REQUEST, MailService.NAMESPACE_URI );

			XmlElement mElem = doc.CreateElement( MailService.E_MESSAGE, MailService.NAMESPACE_URI );
			mElem.SetAttribute(  MailService.A_ID, id );

			reqElem.AppendChild( mElem );
			doc.AppendChild( reqElem );
			return doc;
		}
	}


    public class GetMsgResponse : Response
    {
        public List<Attendee> Attendees;

        public GetMsgResponse()
        {}

        public GetMsgResponse(List<Attendee> attendees)
        {
            Attendees = attendees;
        }


        public override String Name
        {
            get { return MailService.NS_PREFIX + ":" + MailService.GET_MSG_RESPONSE; }
        }

        public override Response NewResponse(XmlNode responseNode)
        {
            var attendees = new List<Attendee>();
            //取得 Msg 的參與人員
            var attendeeNodes = responseNode.SelectNodes($"//{MailService.NS_PREFIX}:at", XmlUtil.NamespaceManager);
            if (attendeeNodes != null)
            {
                foreach (XmlNode attendeeNode in attendeeNodes)
                {
                     
                    string displayName = XmlUtil.AttributeValue(attendeeNode.Attributes, MailService.A_DISPLAY_NAME);
                    string email = XmlUtil.AttributeValue(attendeeNode.Attributes, MailService.A_EMAIL);
                    string userType = XmlUtil.AttributeValue(attendeeNode.Attributes, MailService.A_CUTYPE);
                    attendees.Add(new Attendee
                    {
                        DisplayName = displayName,
                        Email = email,
                        UserType = $"{userType}"
                    });
                }
            }
            
            var res = new GetMsgResponse(attendees);
            res.ResponseNode = responseNode;
            return res;
        }
    }
}
