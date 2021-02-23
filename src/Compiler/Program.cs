using System;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxSharp.Compiler;

namespace RobloxSharp.Compiler {
	class Program {
		string luaOutput = "-- Compiled with RobloxSharp\n";

		void ParseSourceFile(string source) {
			SyntaxTree tree = CSharpSyntaxTree.ParseText(source);
			CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

			foreach (var usingDirective in root.Usings) {
				ParseUsing(usingDirective);
			}

			foreach (MemberDeclarationSyntax member in root.Members) {
				CSharpSyntaxNode node = (CSharpSyntaxNode)member;
				
				ParseNode(node);
			}
		}

		void ParseUsing(UsingDirectiveSyntax usingDirective) {
			
		}

		void ParseNode(CSharpSyntaxNode node) {
			switch (node.Kind()) {
				case SyntaxKind.VariableDeclaration: {
					var syntax = (VariableDeclarationSyntax)node;			
					Nodes.VariableNode varNode = new Nodes.VariableNode(syntax);

					AddToLuaOutput($"{varNode.ToLua()}\n");

					break;
				}
			}

			foreach (var childNode in node.ChildNodes()) {
				CSharpSyntaxNode csharpNode = (CSharpSyntaxNode)childNode;
				ParseNode(csharpNode);
			}
		}

		void AddToLuaOutput(string output) {
			luaOutput += output;
		}

		void OutputLua() {
			Console.WriteLine(luaOutput);
		}

		static void Main(string[] args) {
			var program = new Program();
			string filename = args[0];

			if (!File.Exists(filename)) {
				Console.WriteLine($"[ERROR]: The file {filename} cannot be found");
				Environment.Exit(1);
			} else if (Path.GetExtension(filename) != ".cs") {
				Console.WriteLine($"[ERROR]: The file {filename} isn't a C# source file");
				Environment.Exit(2);
			}

			try {
				var reader = new StreamReader(filename);

				program.ParseSourceFile(reader.ReadToEnd());

				program.OutputLua();
			} catch (IOException e) {
				Console.WriteLine($"[ERROR]: The file {filename} cannot be read: {e.Message}");
			}
		}
	}
}
