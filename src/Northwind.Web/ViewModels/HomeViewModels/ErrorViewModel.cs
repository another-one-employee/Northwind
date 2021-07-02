﻿namespace Northwind.Web.ViewModels.HomeViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ExceptionMessage { get; set; }
    }
}