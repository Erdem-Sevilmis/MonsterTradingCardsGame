﻿namespace SWE1.MessageServer.Core.Response
{
    public class Response
    {
        public StatusCode StatusCode { get; set; }
        public string? Payload { get; set; }
    }
}
