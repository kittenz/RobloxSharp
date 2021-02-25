using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxSharp.Compiler {
	class Compiler {
		private string _luaOutput = "-- Compiled with RobloxSharp\n";
		private string _sourceCode = "";

		public string LuaOutput { get => _luaOutput; }

		public Compiler(string sourceFile) {
			if (!File.Exists(sourceFile)) {
				Console.WriteLine($"[ERROR]: The file {sourceFile} cannot be found");
				Environment.Exit(1);
			} else if (Path.GetExtension(sourceFile) != ".cs") {
				Console.WriteLine($"[ERROR]: The file {sourceFile} isn't a C# source file");
				Environment.Exit(2);
			}

			try {
				var reader = new StreamReader(sourceFile);

				_sourceCode = reader.ReadToEnd();
			} catch (Exception e) {
				Console.WriteLine($"[ERROR]: The file {sourceFile} cannot be read: {e.Message}");
			}
		}

		private void _ParseSyntaxNode(CSharpSyntaxNode syntaxNode, bool isRecursive) {
			if (syntaxNode.Parent?.Kind() != SyntaxKind.Block) {
				var convertedNode = SyntaxNodeConverter.ConvertSyntaxNode(syntaxNode);

				if (convertedNode != null) {
					_luaOutput += $"{convertedNode.ToLua()}\n";
				}

				if (isRecursive) {
					foreach (var childNode in syntaxNode.ChildNodes()) {
						CSharpSyntaxNode csharpNode = (CSharpSyntaxNode)childNode;

						_ParseSyntaxNode(csharpNode, true);
					}
				}
			}
		}

		public void Compile() {
			SyntaxTree tree = CSharpSyntaxTree.ParseText(_sourceCode);
			CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

			foreach (MemberDeclarationSyntax member in root.Members) {
				CSharpSyntaxNode syntaxNode = (CSharpSyntaxNode)member;

				_ParseSyntaxNode(syntaxNode, true);
			}
		}
	}
}
