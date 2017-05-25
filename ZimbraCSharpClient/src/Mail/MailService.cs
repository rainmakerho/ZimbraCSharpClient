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
using Zimbra.Client.src.Mail;

namespace Zimbra.Client.Mail
{
	public class MailService : IZimbraService
	{
		public static String SERVICE_PATH					= "/service/soap";

		public static String NS_PREFIX						= "mail";
		public static String NAMESPACE_URI					= "urn:zimbraMail";
	    public static String SEMICOLON = ";";
        public static String DateTimeFormat = "yyyyMMddTHHmmss";
	    public static String EnumToStringFormat = "G";

        //requests
        public static String GET_FOLDER_REQUEST				= "GetFolderRequest";
		public static String GET_TAG_REQUEST				= "GetTagRequest";
		public static String SEARCH_REQUEST					= "SearchRequest";
		public static String GET_APPT_SUMMARIES_REQUEST		= "GetApptSummariesRequest";
		public static String GET_APPT_REQUEST				= "GetAppointmentRequest";
		public static String GET_MSG_REQUEST				= "GetMsgRequest";
		public static String SYNC_REQUEST					= "SyncRequest";
		public static String NO_OP_REQUEST					= "NoOpRequest";
		public static String MSG_ACTION_REQUEST				= "MsgActionRequest";

        //add by rainmaker_ho@gss.com.tw
        public static String GET_WORKINGHOURS_REQUEST = "GetWorkingHoursRequest";
        public static String GET_FREE_BUSY_REQUEST = "GetFreeBusyRequest";
        public static String CREATE_APPT_REQUEST = "CreateAppointmentRequest";
	    public static String CANCEL_APPT_REQUEST = "CancelAppointmentRequest";
	    public static String CHECK_RECURCONFLICTS_REQUEST = "CheckRecurConflictsRequest";
	    public static String SEND_INVITE_REPLY_REQUEST = "SendInviteReplyRequest";
	    public static String AUTO_COMPLETE_REQUEST = "AutoCompleteRequest";
	    public static String CREATE_MOUNTPOINT_REQUEST = "CreateMountpointRequest";

        //responses
        public static String GET_FOLDER_RESPONSE			= "GetFolderResponse";
		public static String GET_TAG_RESPONSE				= "GetTagResponse";
		public static String SEARCH_RESPONSE				= "SearchResponse";
		public static String GET_APPT_SUMMARIES_RESPONSE	= "GetApptSummariesResponse";
		public static String GET_APPT_RESPONSE				= "GetAppointmentResponse";
		public static String GET_MSG_RESPONSE				= "GetMsgResponse";
		public static String NO_OP_RESPONSE					= "NoOpResponse";
		public static String MSG_ACTION_RESPONSE			= "MsgActionResponse";


        //add by rainmaker_ho@gss.com.tw
        public static String GET_WORKINGHOURS_RESPONSE = "GetWorkingHoursResponse";
        public static String GET_FREE_BUSY_RESPONSE = "GetFreeBusyResponse";
	    public static String CREATE_APPT_RESPONSE = "CreateAppointmentResponse";
        public static String CANCEL_APPT_RESPONSE = "CancelAppointmentResponse";
        public static String CHECK_RECURCONFLICTS_RESPONSE = "CheckRecurConflictsResponse";
        public static String SEND_INVITE_REPLY_REPONSE = "SendInviteReplyResponse";
	    public static String AUTO_COMPLETE_REPONSE = "AutoCompleteResponse";
	    public static String CREATE_MOUNTPOINT_RESPONSE = "CreateMountpointResponse";

        //elements
        public static String E_FOLDER						= "folder";
		public static String E_TAG							= "tag";
		public static String E_QUERY						= "query";
		public static String E_FRAGMENT						= "fr";
		public static String E_INSTANCE						= "inst";
		public static String E_MESSAGE						= "m";
		public static String E_ACTION						= "action";

        //add by rainmaker
	    public static String E_USR = "usr";
        public static String E_F = "f";
        public static String E_U = "u";
        public static String E_B = "b";
	    public static String E_INV = "inv";
	    public static String E_COMP = "comp";
        public static String E_ATTENDEES = "at";
	    public static String E_START = "s";
	    public static String E_END = "e";
	    public static String E_ORGANIZER = "or";
	    public static String E_ALARM = "alarm";
	    public static String E_TRIGGER = "trigger";
        public static String E_RELATIVE = "rel";
	    public static String E_EMAIL = "e";
	    public static String E_SUBJECT = "su";
	    public static String E_MIME_PART = "mp";
	    public static String E_CONTENT = "content";
	    public static String E_APPOINTMENT = "appt";
	    public static String E_LINK = "link";


        //attributes
        public static String A_PARENT_FOLDER_ID				= "l";
		public static String A_ID							= "id";
		public static String A_NAME							= "name";
		public static String A_COLOR						= "color";
		public static String A_UNREAD_COUNT					= "u";
		public static String A_ITEM_COUNT					= "n";
		public static String A_VIEW							= "view";
		public static String A_QUERY						= "query";
		public static String A_TYPES						= "types";
		public static String A_SORT_BY						= "sortBy";
		public static String A_LIMIT						= "limit";
		public static String A_OFFSET						= "offset";
		public static String A_GROUP_BY						= "groupBy";
		public static String A_FETCH						= "fetch";
		public static String A_HTML							= "html";
		public static String A_READ							= "read";
		public static String A_RECIP						= "recip";
		public static String A_START						= "s";
		public static String A_END							= "e";
		public static String A_LOCATION						= "loc";
		public static String A_INV_ID						= "invId";
		public static String A_COMP_NUM						= "compNum";
		public static String A_OP							= "op";

        //add by rainmaker
	    public static String A_STATUS = "status";
	    public static String A_FREE_BUSY_STATUS = "fb";
	    public static String A_CLASS = "class";
	    public static String A_APPT_ALLDAY = "allDay";
	    public static String A_DRAFT = "draft";
	    public static String A_TRANSP = "transp";
	    
	    public static String A_ROLE = "role";
	    public static String A_PARTICIPATION_STATUS = "ptst";
        public static String A_RSVP = "rsvp";
	    public static String A_TIMEZONE = "tz";
	    public static String A_DATE = "d";
	    public static String A_EMAIL = "a";
	    public static String A_DISPLAY_NAME = "d";
	    public static String A_ACTION = "action";
	    public static String A_MINUTES = "m";
	    public static String A_RELATED = "related";
	    public static String A_NEGATIVE = "neg";
	    public static String A_TYPE = "t";
	    public static String A_NAME_PART = "p";
	    public static String A_CONTENT_TYPE = "ct";
	    public static String A_CUTYPE = "cutype";
	    public static String A_CAL_ITEM_ID = "calItemId";
	    public static String A_MODIFY_SEQUENCE = "ms";
        public static String A_CAL_EXPAND_INST_START = "calExpandInstStart";
        public static String A_CAL_EXPAND_INST_END = "calExpandInstEnd";
	    public static String A_VERB = "verb";
	    public static String A_UPDATE_OGGANIZER = "updateOrganizer";
	    public static String A_NEED_EXP = "needExp";
	    public static String A_ZIMBRA_ID = "zid";
	    public static String A_REMOTE_ID = "rid";

        //values ...
        public static String V_METTING_PARENT_FOLDER_ID = "10";
	    public static String V_STATUS_CONF = "CONF";
        public static String V_FB_BUSY = "B";
	    public static String V_APPT_CLASS_PUB = "PUB";
	    public static String V_TRANSP_OPAQUE  = "O";
	    public static String V_ZERO = "0";
	    public static String V_ROLE_NON = "NON";
	    public static String V_ROLE_REQ = "REQ";
	    public static String V_PARTICIPATION_STATUS_NE = "NE";
	    public static String V_TRUE = "1";
	    public static String V_TIMEZONE_TAIPEI = "Asia/Taipei";
	    public static String V_ACTION_DISPLAY = "DISPLAY";
	    public static String V_TO = "t";
        public static String V_FROM = "f";
        public static String V_CT = "multipart/alternative";
	    public static String V_TEXT_PLAIN = "text/plain";
	    public static String V_RELATED_START = "START";
	    public static String V_CUTYPE_RES = "RES";
        

        //qualified names

        //responses...
        public static Response[] responses = { 
			new GetFolderResponse(), 
			new GetTagResponse(),
			new GetApptSummariesResponse(),
			new GetAppointmentResponse(),
			new NoOpResponse(),
			new MsgActionResponse(), 
            new GetWorkingHoursResponse(), 
            new GetFreeBusyResponse(),
            new CreateAppointmentResponse(),
            new SearchResponse(),
            new GetMsgResponse(),
            new CheckRecurConflictsResponse(),
            new CancelAppointmentResponse(),
            new SendInviteReplyResponse(),
            new AutoCompleteResponse(), 
            new CreateMountpointResponse()
        };

		//IZimbraService
		public String NamespacePrefix{ get{ return NS_PREFIX;} } 
		public String NamepsaceUri{ get{ return NAMESPACE_URI; } }
		public Response[] Responses{get{ return responses;} }

	}

	public abstract class MailServiceRequest : Request
	{
		public override String ServicePath{ get{ return MailService.SERVICE_PATH; } }
		public override String HttpMethod{ get { return "POST"; } }
	}
}
