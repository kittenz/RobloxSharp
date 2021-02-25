using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxSharp.Compiler.Nodes {
	class FunctionNode : NodeBase {
		private LocalFunctionStatementSyntax _functionSyntax;

		public FunctionNode(LocalFunctionStatementSyntax functionSyntax) => _functionSyntax = functionSyntax;

		private string _BuildParameterList() {
			string output = "";
			List<string> parameters = new List<string>();

			foreach (var functionChild in _functionSyntax.ChildNodes()) {
				if (functionChild.Kind() == SyntaxKind.ParameterList) {
					foreach (var parameterListChild in functionChild.ChildNodes()) {
						ParameterSyntax parameter = (ParameterSyntax)parameterListChild;

						parameters.Add(parameter.Identifier.ToString());
					}
				}
			}

			output += String.Join(", ", parameters.ToArray());

			return output;
		}

		override public string ToLua() {
			string output = "";

			output += $"function {_functionSyntax.Identifier}(";
			output += _BuildParameterList();
			output += ")";

			return output;
		}
	}
}
