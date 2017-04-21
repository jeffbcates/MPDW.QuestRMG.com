﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Util.Status
{
    public class Constants
    {
        // Error Definition fields
        public const uint questStatusSeverityIndicator = 0xC0000000;
        public const uint questStatusModuleIndicator = 0x4FFF0000;
        public const uint questStatusMessageIndicator = 0x0000FFFF;


        // Severity indicators.
        public const string keyUndefined = "?";
        public const string keyDebug = "D";
        public const string keyInformational = "I";
        public const string keyWarning = "W";
        public const string keyError = "E";
        public const string keyFatal = "F";


        // Severity values.
        public const uint SeverityDebug = 0x00000000;
        public const uint SeverityInformational = 0x00000000;
        public const uint SeverityWarning = 0x40000000;
        public const uint SeverityError = 0x80000000;
        public const uint SeverityFatal = 0xC0000000;


        // questStatus status codes.
        public const uint Success = 0x00000001;
    }
}
