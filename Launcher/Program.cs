using Launcher.Internal;
using Launcher.PathCollection;
using System.Diagnostics;

int processId = 0;
var processName = "Viva Pinata";

Logger.Info($"Waiting for '{processName}' to start.");
while (true)
{
	var processes = Process.GetProcessesByName(processName);
	if (processes.Length == 1)
	{
		var process = processes[0];
		processId = process.Id;

		var elapsedTime = DateTime.Now - process.StartTime;
		if (elapsedTime > TimeSpan.FromSeconds(1))
			Logger.Warn($"Please run the launcher, then start {processName} to ensure patches can be applied before starting!");

		Win32.SuspendProcess(processId);
		Logger.Success($"Found process '{processName}' with ID: {processId}");
		break;
	}
	Thread.Sleep(100);
}

if (new NoPlacementLimitPatch().Apply(processId))
	Logger.Success($"{nameof(NoPlacementLimitPatch)} applied!");

if (Settings.Current.Windowed && new WindowedModePatch().Apply(processId))
	Logger.Success($"{nameof(WindowedModePatch)} applied!");

bool resumed = Win32.ResumeProcess(processId);
if (resumed) { 
	Logger.Success($"Running...");
}
else
{
	Logger.Error($"Failed to resume process!");
	Win32.KillProcess(processId);
	return;
}

Console.WriteLine($"\n\nPress enter to stop the {processName}.");
Console.ReadLine();
Win32.KillProcess(processId);
