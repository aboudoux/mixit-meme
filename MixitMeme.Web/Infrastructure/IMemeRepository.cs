using MixitMeme.Infra;

namespace MixitMeme.Web.Infrastructure {
	public interface IMemeRepository
	{
		public int Add(string url, string author);
		public MemeData[] All();
	}
}