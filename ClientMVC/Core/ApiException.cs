﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMVC.Core
{
    public class ApiException
    {
        public ApiException(int statusCode, string message =null, string details= null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }        
        public IEnumerable<string> Errors { get; set; }

    }
}