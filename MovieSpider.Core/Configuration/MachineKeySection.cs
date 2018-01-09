using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Configuration
{
    public class MachineKeySection : ConfigurationSection
    {
        [ConfigurationProperty("applicationName", IsRequired = false)]
        public string ApplicationName { get { return (string)this["applicationName"]; } }

        [ConfigurationProperty("decryption", IsRequired = false)]
        public string Decryption { get { return (string)this["decryption"]; } }

        [ConfigurationProperty("decryptionKey", IsRequired = false)]
        public string DecryptionKey { get { return (string)this["decryptionKey"]; } }

        [ConfigurationProperty("validation", IsRequired = false)]
        public string Validation { get { return (string)this["validation"]; } }

        [ConfigurationProperty("validationKey", IsRequired = false)]
        public string ValidationKey { get { return (string)this["validationKey"]; } }
    }
}
