using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxSharp.Compiler.Nodes {
	class VariableNode : NodeBase {
		private VariableDeclarationSyntax _variableDeclarationSyntax;
		private List<string> _variableNames = new List<string>();
		private List<dynamic> _variableValues = new List<dynamic>();

		public VariableDeclarationSyntax VariableDeclSyntax { get => _variableDeclarationSyntax; }

		public VariableNode(VariableDeclarationSyntax syntax) => _variableDeclarationSyntax = syntax;

		private void _ParseVariable(VariableDeclaratorSyntax variable) {
			_variableNames.Add(variable.Identifier.ToString());

			foreach (var childNode in variable.ChildNodes()) {
				if (childNode.Kind() == SyntaxKind.EqualsValueClause) {
					foreach (var equalsValueKindChild in childNode.ChildNodes()) {
						switch (equalsValueKindChild.Kind()) {
							case SyntaxKind.NullLiteralExpression: {
								_variableValues.Add("nil");

								break;
							}
							case SyntaxKind.NumericLiteralExpression: {
								double number;

								if (!double.TryParse(equalsValueKindChild.ToString(), out number)) {
									Console.WriteLine("[COMPILATION ERROR]: Cannot parse double");
									Environment.Exit(-1);
								}

								_variableValues.Add(number.ToString());

								break;
							}
							default: {
								_variableValues.Add(equalsValueKindChild.ToString());

								break;
							}
						}
					}
				}
			}
		}

		override public string ToLua() {
			string output = "local ";

			_variableNames = new List<string>();
			_variableValues = new List<dynamic>();

			foreach (var variable in _variableDeclarationSyntax.Variables) {
				_ParseVariable(variable);
			}

			if (_variableNames.Count > _variableValues.Count) {
				int substraction = _variableNames.Count - _variableValues.Count;

				for (int i = 0; i < substraction; i++) {
					_variableValues.Add("nil");
				}
			}

			if (_variableValues.Count > 0) {
				string names = String.Join(", ", _variableNames.ToArray());
				string values = String.Join(", ", _variableValues.ToArray());

				output += names;
				output += " = ";
				output += values;
			} else {
				string names = String.Join(", ", _variableNames.ToArray());

				output += names;
			}

			return output;
		}
	}
}
