using System.Threading.Tasks;
using FluentAssertions;
using MixitMeme.Web.Infrastructure;
using Xunit;

namespace MixitMeme.Tests;

public class MemeCheckerShould
{
	[Theory(DisplayName = "Check if meme exists")]
	[InlineData("https://community.hackyourjob.org/uploads/default/original/1X/cbbc6b05709f843951979d663b291f11aeff7122.gif", true)]
	[InlineData("https://community.hackyourjob.org/uploads/default/original/1X/osef.gif", false)]
	public async Task Test8(string url, bool expectedResult)
	{
		MemeChecker checker = new MemeChecker();
		(await checker.Exists(new MemeUrl(url))).Should().Be(expectedResult);
	}
}