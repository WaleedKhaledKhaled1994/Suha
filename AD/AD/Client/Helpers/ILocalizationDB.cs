using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AD.Client.Helpers
{
    public interface ILocalizationDB
    {
        string GetName(Object Entity);
        string GetDescription(Object Entity);
    }
}
