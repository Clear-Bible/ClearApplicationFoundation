using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearApplicationFoundation.Services;

namespace ClearApplicationFoundation.Demo.Services
{
    internal class LocalizationService : ILocalizationService
    {
        public string Get(string key)
        {
            throw new NotImplementedException();
        }

        public string this[string key] => throw new NotImplementedException();
    }
}
