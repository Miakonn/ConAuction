using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConAuction3.Utilities {
    public static class StringHelpers {
        public static string SetValueWithLimit(string value, int maxLength) {
            if (value == null) {
                return null;
            }
            return value.Length > maxLength ? value.Substring(0, maxLength) : value;
        }
    }
}
