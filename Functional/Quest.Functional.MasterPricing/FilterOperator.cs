using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Quest.Functional.MasterPricing
{
    public class FilterOperator
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public const int NoOperation = -1;

        public const int IsEqualTo = 1;
        public const int Contains = 2;
        public const int StartsWith = 3;
        public const int EndsWith = 4;
        public const int IsBlank = 5;
        public const int IsNull = 6;
        public const int IsNOTEqualTo = 7;
        public const int DoesNOTContain = 8;
        public const int DoesNOTStartWith = 9;
        public const int DoesNOTEndWith = 10;
        public const int IsNOTBlank = 11;
        public const int IsNOTNull = 12;
        public const int LessThanOrEqualTo = 13;
        public const int LessThan = 14;
        public const int GreaterThan = 15;
        public const int GreaterThanOrEqualTo = 16;
        public const int MatchesAdvanced = 17;
    }
}
