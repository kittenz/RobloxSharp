using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxSharp.Compiler.Nodes {
	class ClassNode : NodeBase {
		private ClassDeclarationSyntax _classSyntax;

		public ClassNode(ClassDeclarationSyntax classSyntax) => _classSyntax = classSyntax;

		override public string ToLua() {
			string output = "";

			output += $"local {_classSyntax.Identifier} = ";
			output += "{}\n";
			output += $"{_classSyntax.Identifier}.__index = {_classSyntax.Identifier}\n\n";
			output += $"function {_classSyntax.Identifier}.new()\n";
			output += "\tlocal self = {}\n";
			output += $"\tsetmetatable(self, {_classSyntax.Identifier})\n\n";
			output += "\treturn self\n";
			output += "end";

			return output;
		}
	}
}
