using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core
{
    public class AppValidationResponse: AppResponse
    {
        public AppValidationResponse() : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }        
    }
}
