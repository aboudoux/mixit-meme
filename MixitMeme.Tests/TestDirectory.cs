using System;
using System.IO;
using System.Reflection;

namespace MS.Tests.Assets;

public class TestDirectory : IDisposable {
	private static readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

	public string DirectoryPath;

	private TestDirectory(string directoryName) {
		if (string.IsNullOrWhiteSpace(directoryName)) throw new ArgumentException(nameof(directoryName));
		DirectoryPath = Path.Combine(CurrentDirectory, directoryName);
		if (!Directory.Exists(DirectoryPath))
			Directory.CreateDirectory(DirectoryPath);
	}

	public static TestDirectory Create(string directoryName) => new(directoryName);

	public void Dispose() {
		try {
			Directory.Delete(DirectoryPath, true);
		}
		catch (Exception) {

		}
	}
}