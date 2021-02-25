using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxSharp.Compiler {
	class SyntaxNodeConverter {
		public static Nodes.NodeBase ConvertSyntaxNode(CSharpSyntaxNode syntaxNode) {
			switch (syntaxNode.Kind()) {
				case SyntaxKind.Block: {
					var syntax = (BlockSyntax)syntaxNode;
					Nodes.BlockNode blockNode = new Nodes.BlockNode(syntax);

					return blockNode;
				}
				case SyntaxKind.VariableDeclaration: {
					var syntax = (VariableDeclarationSyntax)syntaxNode;	
					Nodes.VariableNode variableNode = new Nodes.VariableNode(syntax);

					return variableNode;
				}
				default: {
					return null;
				}
			}
		}
	}
}