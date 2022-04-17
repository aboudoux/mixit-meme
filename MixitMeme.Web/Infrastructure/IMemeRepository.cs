namespace MixitMeme.Web.Infrastructure;

public interface IMemeRepository
{
	public int Add(MemeUrl url, MemeAuthor author);
	public MemeData[] All();
}