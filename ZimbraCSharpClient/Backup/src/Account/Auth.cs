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
using System.Xml;
using Zimbra.Client.Util;

namespace Zimbra.Client.Account
{
	



	public class AuthRequest : AccountServiceRequest
	{
		private String accountName;
		private String password;

		public AuthRequest(String account, String password)
		{
			accountName = account;
			this.password = password;
		}

		public override XmlDocument ToXmlDocument()
		{
			XmlDocument doc = new XmlDocument();

			//create the AuthRequest node
			XmlElement requestNode = doc.CreateElement( AccountService.AUTH_REQUEST, AccountService.NAMESPACE_URI);

			//create & config the account node
			XmlElement accountNode = doc.CreateElement( AccountService.E_ACCOUNT, AccountService.NAMESPACE_URI );
			accountNode.SetAttribute( AccountService.A_BY, AccountService.A_NAME );
			accountNode.InnerText = accountName;

			//create and config the password node
			XmlElement pwdNode = doc.CreateElement( AccountService.E_PASSWORD, AccountService.NAMESPACE_URI );
			pwdNode.InnerText = password;

			//add em together...
			requestNode.AppendChild( accountNode );
			requestNode.AppendChild( pwdNode );
			doc.AppendChild( requestNode );

			return doc;
		}

		public override String Name()
		{
			return AccountService.NS_PREFIX + ":" + AccountService.AUTH_REQUEST;
		}

	}


	
	
	
	public class AuthResponse : Response
	{
		private String authToken;
		private String lifetime;
		private String sessionId;

		public AuthResponse(){}

		public AuthResponse( String a, String l, String s )
		{
			authToken = a;
			lifetime = l;
			sessionId = s;
		}

		public String AuthToken
		{
			get{ return authToken; }
		}

		public String LifeTime
		{
			get{ return lifetime; }
		}

		public String SessionId
		{
			get{ return sessionId; }
		}

		public override String Name
		{
			get{ return AccountService.NS_PREFIX + ":" + AccountService.AUTH_RESPONSE;}
		}


		public override Response NewResponse(XmlNode responseNode)
		{
			String authToken = XmlUtil.GetNodeText( responseNode, AccountService.Q_AUTHTOKEN );
			String lifetime  = XmlUtil.GetNodeText( responseNode, AccountService.Q_LIFETIME );
			String sessionId = XmlUtil.GetNodeText( responseNode, AccountService.Q_SESSIONID);

			return new AuthResponse( authToken, lifetime, sessionId );
		}

		
	}
}
