﻿using System;
using System.Text;
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

				if (node.Parent.Kind() == SyntaxKind.Block && node.Parent != _blockSyntax) {
					continue;
				} else {
					NodeBase convertedNode = SyntaxNodeConverter.ConvertSyntaxNode(node);

					if (convertedNode != null) {
						_childNodes.Add(convertedNode);
					}

					_ConvertChildNodes(node);
				}
			}
		}

		private string _GetBlockStartKeyword() {
			if (_blockSyntax.Parent?.Kind() == SyntaxKind.LocalFunctionStatement) {
				return "";
			} else {
				return "do";
			}
		}

		override public string ToLua() {
			StringBuilder output = new StringBuilder();
			
			output.Append($"{_GetBlockStartKeyword()}\n");

			foreach (var node in _childNodes) {
				string lua = node.ToLua();
				output.Append($"\t{lua}");
			}

			output.Append("end\n");

			return output.ToString();
		}
	}
}
