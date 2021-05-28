using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapr_aspnetcore.Models
{
    public class Account
    {
        /// <summary>
        /// Gets or sets account id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets account balance.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
