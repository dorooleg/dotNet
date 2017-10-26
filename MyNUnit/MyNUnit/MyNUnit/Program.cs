﻿using System;
using System.IO;

namespace MyNUnit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Invalid count arguments");
                return;
            }

            foreach (var dir in Directory.GetFiles(args[0], "*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    var engine = new TestEngine(dir);
                    Console.WriteLine(engine.Report());
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}
