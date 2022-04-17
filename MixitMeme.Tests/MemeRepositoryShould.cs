using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MixitMeme.Web.Infrastructure;
using MS.Tests.Assets;
using Newtonsoft.Json;
using TypeSupport.Extensions;
using Xunit;

namespace MixitMeme.Tests;

[Category("database")]
public class MemeRepositoryShould 
{
	[Fact(DisplayName = "Add new meme and retrive it")]
	public void Test8()
	{
		using TestDirectory directory = TestDirectory.Create("database");
		MemeRepository repository = new(new DatabaseDirectory(directory.DirectoryPath));
		var id = repository.Add(new MemeUrl("https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg"), new MemeAuthor("test"));

		id.Should().Be(1);
		File.Exists(Path.Combine(directory.DirectoryPath, "1.json")).Should().BeTrue();

		repository.All().Should().HaveCount(1);
		repository.All().First().Should().Be(new MemeData(1,
			"https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg",
			"test"));
	}

	[Fact(DisplayName = "Add multiples meme and retrive it")]
	public void Test9() 
	{
		using TestDirectory directory = TestDirectory.Create("database");
		MemeRepository repository = new(new DatabaseDirectory(directory.DirectoryPath));
		repository.Add(new MemeUrl("https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg"), new MemeAuthor("test"));
		repository.Add(new MemeUrl("https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg"), new MemeAuthor("test"));
		repository.Add(new MemeUrl("https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg"), new MemeAuthor("test"));
		var id = repository.Add(new MemeUrl("https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg"), new MemeAuthor("test"));

		id.Should().Be(4);
		Directory.GetFiles(directory.DirectoryPath, "*.json").Should().HaveCount(4);

		File.Exists(Path.Combine(directory.DirectoryPath, "1.json")).Should().BeTrue();
		File.Exists(Path.Combine(directory.DirectoryPath, "2.json")).Should().BeTrue();
		File.Exists(Path.Combine(directory.DirectoryPath, "3.json")).Should().BeTrue();
		File.Exists(Path.Combine(directory.DirectoryPath, "4.json")).Should().BeTrue();

		repository.All().Should().HaveCount(4);
		var all = repository.All();
		all[0].Should().Be(new MemeData(1, "https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg", "test"));
		all[1].Should().Be(new MemeData(2, "https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg", "test"));
		all[2].Should().Be(new MemeData(3, "https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg", "test"));
		all[3].Should().Be(new MemeData(4, "https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg", "test"));
	}

	[Fact(DisplayName = "Load database")]
	public async Task Test55()
	{
		using TestDirectory directory = TestDirectory.Create("database") ;
		await Enumerable.Range(1, 10).ForEachAsync(id => {
			var data = new MemeData(id, "https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg", "test");
			File.WriteAllText(Path.Combine(directory.DirectoryPath, id + ".json"), JsonConvert.SerializeObject(data));
			return Task.CompletedTask;
		});

		MemeRepository repository = new(new DatabaseDirectory(directory.DirectoryPath));
		repository.All().Should().HaveCount(10);
		repository.All().Max(a => a.Id).Should().Be(10);

		repository.Add(new MemeUrl("https://community.hackyourjob.org/uploads/default/original/1X/eff9694537c0fc1d2333551d2b29131ec3b91b32.jpeg"), new MemeAuthor("test"));
		repository.All().Should().HaveCount(11);
		repository.All().Max(a => a.Id).Should().Be(11);
	}
}