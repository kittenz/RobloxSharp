using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxSharp.Compiler;

namespace RobloxSharp.Compiler.Nodes {
	class UsingNode : NodeBase {
		private UsingDirectiveSyntax _syntax;

		public UsingNode(UsingDirectiveSyntax syntax) => _syntax = syntax;

		private List<string> _GetNamespaces() {
			List<string> nameList = new List<string>();
			List<IdentifierNameSyntax> identifiers = new List<IdentifierNameSyntax>();

			foreach (var child in _syntax.ChildNodes()) {
				if (child.Kind() == SyntaxKind.IdentifierName) {
					identifiers.Add((IdentifierNameSyntax)child);
				}
			}

			foreach (var indentifier in identifiers) {
				nameList.Add(indentifier.Identifier.ToString());
			}

			if (nameList.Count == 0) {
				Console.WriteLine("Compilation error: no namespaces in using");
				Environment.Exit(-1);
			}

			return nameList;
		}

		public override string ToLua() {
			StringBuilder output = new StringBuilder("local ");
			List<string> namespaces = _GetNamespaces();
			Random rng = new Random();

			string variableName = namespaces[namespaces.Count - 1] + $"_{rng.Next(100).ToString()}"; // Add a random number to prevent name collisions

			output.Append(variableName);
			output.Append(" = require(RBLXSHARP_RUNTIME");

			foreach (var namespc in namespaces) {
				output.Append($":WaitForChild(\"{namespc}\")");
			}

			output.Append(")\n");
			
			return output.ToString();
		}
	}
}
