using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxSharp.Compiler;

namespace RobloxSharp.Compiler.Nodes {
	class BlockNode : NodeBase {
		private BlockSyntax _blockSyntax;
		private List<NodeBase> _childNodes = new List<NodeBase>();
		private string _output = "";

		public BlockNode(BlockSyntax blockSyntax) {
			_blockSyntax = blockSyntax;

			CSharpSyntaxNode syntaxNode = (CSharpSyntaxNode)blockSyntax;
			_ConvertChildNodes(syntaxNode);
		}

		private void _ConvertChildNodes(CSharpSyntaxNode syntaxNode) {
			foreach (var childNode in syntaxNode.ChildNodes()) {
				CSharpSyntaxNode node = (CSharpSyntaxNode)childNode;
				NodeBase convertedNode = SyntaxNodeConverter.ConvertSyntaxNode(node);

				if (convertedNode != null) {
					_childNodes.Add(convertedNode);
				}

				_ConvertChildNodes(node);
			}
		}

		override public string ToLua() {
			string output = "";
			
			output += "do\n";

			foreach (var node in _childNodes) {
				string lua = node.ToLua();
				output += $"\t{lua}";
			}

			output += "end\n";

			return output;
		}
	}
}
