using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace EmlToMht
{
	internal class Program
	{
		private static void Main()
		{
			var folder = ConfigurationManager.AppSettings["Folder"];
			if (string.IsNullOrEmpty(folder)) return;
			if (!Directory.Exists(folder)) return;

			foreach (var file in EnumerateFileWithExtension(new DirectoryInfo(folder)).ToList())
			{
				var name = Path.ChangeExtension(file.FullName, ".mht");
				File.Move(file.FullName, name);
			}
		}

		private static IEnumerable<FileInfo> EnumerateFileWithExtension(DirectoryInfo directory)
		{
			foreach (var file in directory.GetFiles("*.eml"))
			{
				yield return file;
			}

			foreach (var file in directory.GetDirectories().SelectMany(EnumerateFileWithExtension))
			{
				yield return file;
			}
		}
	}
}