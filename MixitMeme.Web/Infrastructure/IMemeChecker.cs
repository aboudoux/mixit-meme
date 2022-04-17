namespace MixitMeme.Web.Infrastructure;

public interface IMemeChecker
{
	public Task<bool> Exists(MemeUrl url);
}