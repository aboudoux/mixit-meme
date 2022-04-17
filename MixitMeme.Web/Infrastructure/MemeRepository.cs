using Newtonsoft.Json;

namespace MixitMeme.Web.Infrastructure;

public class MemeRepository : IMemeRepository
{
	private readonly DatabaseDirectory _databaseDirectory;

	private List<MemeData?> _data = new();

	private int _lastId = 0;
	private object _locker = new();

	private int GetId()
	{
		lock (_locker)
		{
			return ++_lastId;
		}
	}

	public MemeRepository(DatabaseDirectory databaseDirectory)
	{
		_databaseDirectory = databaseDirectory ?? throw new ArgumentNullException(nameof(databaseDirectory));

		var entries = Directory.GetFiles(_databaseDirectory.Value, "*.json");
		if (!entries.Any()) return;

		_data = Directory.GetFiles(_databaseDirectory.Value, "*.json")
			.Select(path => JsonConvert.DeserializeObject<MemeData>(File.ReadAllText(path)))
			.ToList();

		_lastId = _data.Max(a => a.Id);
	}
	public int Add(MemeUrl url, MemeAuthor author)
	{
		var id = GetId();
		var data = new MemeData(id, url.Value, author.Value);
		File.WriteAllText(Path.Combine(_databaseDirectory.Value, id + ".json"), JsonConvert.SerializeObject(data));
		_data.Add(data);
		return id;
	}

	public MemeData[] All() => _data.ToArray();
}

public record DatabaseDirectory
{
	public string Value { get; }
	public DatabaseDirectory(string directory)
	{
		if (!Directory.Exists(directory))
			throw new DirectoryNotFoundException(directory);
		Value = directory;
	}
}