﻿using System;
using MyNUnit;

namespace MyNUnitTest
{    
    public class AfterException
    {
        [After]
        public void SetUp()
        {
            throw new ArgumentException();
        }

        [Test]
        public void Test()
        {
        }
    }
}
