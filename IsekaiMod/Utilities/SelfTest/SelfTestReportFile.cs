using System;
using System.IO;
using System.Text;

namespace IsekaiMod.Utilities.SelfTest;

public sealed class SelfTestReportFile : IDisposable
{
	public const string FileName = "IsekaiSelfTest.log";

	private readonly object gate = new object();

	private FileStream stream;

	private SelfTestReportFile(FileStream stream)
	{
		this.stream = stream;
	}

	public static string PathBeside(string playerLogPath)
	{
		if (string.IsNullOrEmpty(playerLogPath))
		{
			return null;
		}
		string directoryName = Path.GetDirectoryName(playerLogPath);
		if (!string.IsNullOrEmpty(directoryName))
		{
			return Path.Combine(directoryName, "IsekaiSelfTest.log");
		}
		return null;
	}

	public static SelfTestReportFile Create(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return null;
		}
		try
		{
			return new SelfTestReportFile(new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete));
		}
		catch
		{
			return null;
		}
	}

	public bool Append(string line)
	{
		lock (gate)
		{
			if (stream == null)
			{
				return false;
			}
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes((line ?? "").Replace("\r", " ").Replace("\n", " ") + "\n");
				stream.Write(bytes, 0, bytes.Length);
				stream.Flush(flushToDisk: true);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	public void Dispose()
	{
		lock (gate)
		{
			try
			{
				stream?.Dispose();
			}
			catch
			{
			}
			stream = null;
		}
	}
}
