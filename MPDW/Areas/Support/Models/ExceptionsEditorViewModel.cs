using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quest.Util.Data;
using Quest.MPDW.Models;
using Quest.Functional.ASM;
using Quest.Functional.FMS;
using Quest.Functional.Logging;


namespace Quest.MPDW.Support.Models
{
    public class ExceptionsEditorViewModel : SupportBaseListViewModel
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Module { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime Created { get; set; }


        public ExceptionsEditorViewModel()
            : base()
        {
        }
        public ExceptionsEditorViewModel(UserSession userSession)
            : base(userSession)
        {
        }
        public ExceptionsEditorViewModel(UserSession userSession, BaseUserSessionViewModel baseUserSessionViewModel)
            : base(userSession, baseUserSessionViewModel)
        {
        }
    }
}