﻿using System;

namespace MyNUnit
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class BeforeClass : Attribute
    {
    }
}