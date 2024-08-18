namespace Launcher.Internal;

public interface IPatch
{
	bool Apply(int processId);
}
