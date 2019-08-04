﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Statiq.CodeAnalysis.Scripting;
using Statiq.Common;

namespace Statiq.CodeAnalysis
{
    /// <summary>
    /// Compiles a C# based script contained in document content.
    /// </summary>
    /// <category>Extensibility</category>
    public class CompileScript : IModule
    {
        public const string CompiledKey = "_CompiledScript";

        public async Task<IEnumerable<IDocument>> ExecuteAsync(IExecutionContext context) =>
            await context.ParallelQueryInputs().SelectAsync(async input =>
            {
                byte[] assembly = ScriptHelper.Compile(await input.GetStringAsync(), input, context);
                MemoryStream stream = context.MemoryStreamFactory.GetStream(assembly);
                return input.Clone(
                    new MetadataItems
                    {
                        { CompiledKey, true }
                    },
                    context.GetContentProvider(stream));
            });
    }
}
