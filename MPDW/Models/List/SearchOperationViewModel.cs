using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quest.MPDW.Models.List
{
    public class SearchOperationViewModel
    {
        public string Value { get; set; }

        public const string None = "!";
        public const string Equal = "=";
        public const string NotEquals = "!=";
        public const string LessThan = "<";
        public const string LessThanOrEqual = "<=";
        public const string GreaterThan = ">";
        public const string GreaterThanOrEqual = ">=";
        public const string BeginsWith = "^";
        public const string DoesNotBeginWith = "!^";
        public const string Contains = "//";
        public const string DoesNotContain = "!//";
        public const string EndsWith = "$";
        public const string DoesNotEndWith = "!$";
        public const string IsNull = "null";
        public const string IsNotNull = "!null";
        public const string DateOnly = "date";
        public const string DateAndTime = "datetime";

        public SearchOperationViewModel()
        {
            Value = SearchOperationViewModel.Contains;
        }
        public SearchOperationViewModel(string _value)
        {
            Value = _value;
        }
    }
}