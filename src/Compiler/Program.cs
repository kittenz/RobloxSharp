using System;

namespace RobloxSharp.Compiler {
	class Program {
		static void Main(string[] args) {
			var compiler = new Compiler(args[0]);

			compiler.Compile();
			Console.WriteLine(compiler.LuaOutput);
		}
	}
}
