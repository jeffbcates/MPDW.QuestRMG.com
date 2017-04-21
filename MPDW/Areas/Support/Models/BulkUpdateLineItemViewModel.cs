using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Quest.MPDW.Support.Models
{
    public class BulkUpdateLineItemViewModel
    {
        public int Id { get; set; }
        public int UserSessionId { get; set; }
        public string Username { get; set; }
        public string Database { get; set; }
        public string Tableset { get; set; }
        public string Filter { get; set; }
        public string Event { get; set; }
        public int NumRows { get; set; }
        public string Parameters { get; set; }
        public string BulkInsertColumn { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string Batch { get; set; }
        public string Created { get; set; }
    }
}