using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace App2
{
    public class Machine
    {
        public int machineID { get; set; }
        public string machineName { get; set; }
        public string machineLocation { get; set; }
    }
}
