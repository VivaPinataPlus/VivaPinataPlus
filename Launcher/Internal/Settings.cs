using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Launcher.Internal;

internal record Settings
{
	[JsonIgnore]
	private const string ConfigName = "config.json";


	public bool Windowed { get; set; } = false;

	private static Settings _instance = null;
	public static Settings Current
	{
		get
		{
			if (_instance == null)
				_instance = LoadOrCreate();

			return _instance;
		}
	}

	private static Settings LoadOrCreate()
	{
		Settings settings;
		if (File.Exists(ConfigName))
		{
			var jsonString = File.ReadAllText(ConfigName);
			var deserialiezd = JsonSerializer.Deserialize<Settings>(jsonString);
			if (deserialiezd == null)
				throw new SerializationException($"Failed to deserialize settings file!");
			settings = deserialiezd;
		}
		else
		{
			settings = new Settings
			{
				Windowed = false
			};
			var jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions
			{
				WriteIndented = true
			});
			File.WriteAllText(ConfigName, jsonString);
		}
		return settings;
	}
}
