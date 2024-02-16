using NLog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.Transactions;

namespace BANKING_APPLICATION
{
    public class baratis_mflobelis_monacemebi
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public baratis_monacmebi cardDetails { get; set; }
        public string pinCode { get; set; }
        public Transaction[] transactionHistory { get; set; }
    }
}