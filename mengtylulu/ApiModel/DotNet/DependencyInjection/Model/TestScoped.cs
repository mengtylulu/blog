﻿using mengtylulu.ApiModel.DotNet.DependencyInjection.Interface;

namespace mengtylulu.ApiModel.DotNet.DependencyInjection.Model
{
    public class TestScoped : ITestScoped
    {
        public string name { get; set; } = string.Empty;
    }
}
