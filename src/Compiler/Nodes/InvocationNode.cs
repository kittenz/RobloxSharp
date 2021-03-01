using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxSharp.Compiler.Nodes {
	class InvocationNode : NodeBase {
		private InvocationExpressionSyntax _invocationSyntax;

		public InvocationNode(InvocationExpressionSyntax syntax) => _invocationSyntax = syntax;

		private string _BuildArgumentList() {
			List<string> arguments = new List<string>();

			foreach (var child in _invocationSyntax.ChildNodes()) {
				if (child.Kind() == SyntaxKind.ArgumentList) {
					var argListSyntax = (ArgumentListSyntax)child;

					foreach (var argument in argListSyntax.Arguments) {
						var expression = argument.Expression;
						arguments.Add(expression.ToString());
					}
				}
			}

			return String.Join(", ", arguments);
		}

		override public string ToLua() {
			string output = "";
			string identifier = "";

			foreach (var child in _invocationSyntax.ChildNodes()) {
				if (child.Kind() == SyntaxKind.IdentifierName) {
					var identifierName = (IdentifierNameSyntax)child;

					identifier = identifierName.Identifier.ToString();
				}
			}

			output = $"{identifier}({_BuildArgumentList()})";

			return output;
		}
	}
}
