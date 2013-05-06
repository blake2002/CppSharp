﻿using System;
using System.IO;
using CppSharp.Types;

namespace CppSharp.Generators.CSharp
{
    public class CSharpGenerator : Generator
    {
        private readonly CSharpTypePrinter typePrinter;

        public CSharpGenerator(Driver driver) : base(driver)
        {
            typePrinter = new CSharpTypePrinter(driver.TypeDatabase, driver.Library);
            Type.TypePrinterDelegate += type => type.Visit(typePrinter).Type;
        }

        void WriteTemplate(TextTemplate template)
        {
            var file = Path.GetFileNameWithoutExtension(template.TranslationUnit.FileName)
                + Driver.Options.WrapperSuffix + "."
                + template.FileExtension;

            var path = Path.Combine(Driver.Options.OutputDir, file);

            template.Generate();

            Console.WriteLine("  Generated '" + file + "'.");
            File.WriteAllText(Path.GetFullPath(path), template.ToString());
        }

        public override bool Generate(TranslationUnit unit)
        {
            var template = new CSharpTextTemplate(Driver, unit);
            WriteTemplate(template);

            return true;
        }
    }
}
