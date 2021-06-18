using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.Core
{
    public class Data<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public ApiException Error { get; set; }
        
    }
}
