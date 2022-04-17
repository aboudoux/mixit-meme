namespace MixitMeme.Infra {
	public interface IMemeRepository
	{
		public void Add(string url, string author);
		public MemeData[] All();
	}
}