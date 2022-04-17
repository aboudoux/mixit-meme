namespace MixitMeme.Web.Infrastructure;

public class MemeChecker : IMemeChecker
{
	public async Task<bool> Exists(MemeUrl url)
	{
		try
		{
			using HttpClient client = new HttpClient();
			var result = await client.GetAsync(url.Value);
			return result.IsSuccessStatusCode;
		}
		catch (Exception e)
		{
			return false;
		}
		
	}
}