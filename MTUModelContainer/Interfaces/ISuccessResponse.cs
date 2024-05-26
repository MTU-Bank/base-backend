using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTUModelContainer.Interfaces
{
    internal interface ISuccessResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
