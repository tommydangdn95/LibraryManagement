using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Utils
{
    public static class StringExtension
    {
        public static string ToError(this IEnumerable<IdentityError> errors)
        {
            return string.Join(".", errors.Select(e => e.Description));
        }

        public static string ToError(this IList<string> errors)
        {
            return string.Join(".", errors);
        }
    }
}
