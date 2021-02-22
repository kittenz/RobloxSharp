using System;
using System.IO;

namespace RobloxSharp.Compiler {
	class Program {
		void ParseSourceFile(string source) {
			Console.WriteLine(source);
		}

		static void Main(string[] args) {
			var program = new Program();
			string filename = args[0];

			if (!File.Exists(filename)) {
				Console.WriteLine($"[ERROR]: The file {filename} cannot be found");
				Environment.Exit(1);
			} else if (Path.GetExtension(filename) != ".cs") {
				Console.WriteLine($"[ERROR]: The file {filename} isn't a C# source file");
				Environment.Exit(2);
			}

			try {
				var reader = new StreamReader(filename);

				program.ParseSourceFile(reader.ReadToEnd());
			} catch (IOException e) {
				Console.WriteLine($"[ERROR]: The file {filename} cannot be read: {e.Message}");
			}
		}
	}
}
