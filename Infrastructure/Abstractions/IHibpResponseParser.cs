using Infrastructure.Breach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordAnalysisService.Infrastructure.Abstractions
{
    public interface IHibpResponseParser
    {
        int? FindBreachCount(string response, string suffix);
    }
}
