using System;

namespace RobloxSharp.Compiler.Nodes {
	class NodeBase {
		private NodeBase[] _childNodes = {};

		public NodeBase[] Children { get => _childNodes; }

		virtual public string ToLua() {
			return "";
		}
	}
}
