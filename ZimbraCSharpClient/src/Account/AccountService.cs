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


namespace Zimbra.Client.Account
{
	public class AccountService : IZimbraService
	{
		public static String SERVICE_PATH	= "/service/soap";

		//this services namespace uri
		public static String NS_PREFIX		= "account";
		public static String NAMESPACE_URI	= "urn:zimbraAccount";
		
		//requests
		public static String AUTH_REQUEST	= "AuthRequest";
        public static String SEARCH_GAL_REQUEST = "SearchGalRequest";

        //add by rainmaker
        public static String SEARCH_CALENDAR_RESOURCES_REQUEST = "SearchCalendarResourcesRequest";

        

        //responses
        public static String AUTH_RESPONSE	= "AuthResponse";
        public static String SEARCH_GAL_RESPONSE = "SearchGalResponse";

        //add by rainmaker
        public static String SEARCH_CALENDAR_RESOURCES_RESPONSE = "SearchCalendarResourcesResponse";


        //element names
        public static String E_ACCOUNT		= "account";
		public static String E_PASSWORD		= "password";
		public static String E_LIFETIME		= "lifetime";
		public static String E_SESSIONID	= "sessionId";
		public static String E_AUTHTOKEN	= "authToken";
        public static String E_NAME = "name";

        //add by rainmaker
        public static string E_SEARCH_FILTER = "searchFilter";
        public static string E_CONDS = "conds";
        public static string E_COND = "cond";

        
      


        //attribute names
        public static String A_NAME			= "name";
		public static String A_BY			= "by";
        public static String A_ATTRNAME     = "n";


        //add by rainmaker
        public static string A_ID = "id";
        public static String A_ATTRS = "attrs";
        public static string A_ATTR = "attr";
        
        public static string A_OP_TYPE = "eq";
        public static string A_VALUE = "value";
        
        public static String A_OP = "op";

        //attribute Values
        public static string V_ATTR_TYPE = "zimbraCalResType";
        public static string V_LOCATION = "location";
        public static string V_DEFAULT_CALENDAR_ATTRS = "email,fullName";

        //qualified names
        public static String Q_AUTHTOKEN	= NS_PREFIX + ":" + E_AUTHTOKEN;
		public static String Q_LIFETIME		= NS_PREFIX + ":" + E_LIFETIME;
		public static String Q_SESSIONID	= NS_PREFIX + ":" + E_SESSIONID;

        public static Response[] responses = { new AuthResponse() };
		public String NamespacePrefix{ get{ return NS_PREFIX; }}
		public String NamepsaceUri{ get{ return NAMESPACE_URI; }}
		public Response[] Responses{get{ return responses;}}

	}


	public abstract class AccountServiceRequest : Request
	{
		public override String ServicePath{ get{ return AccountService.SERVICE_PATH; } }
		public override String HttpMethod{ get { return "POST"; } }
	}


}
