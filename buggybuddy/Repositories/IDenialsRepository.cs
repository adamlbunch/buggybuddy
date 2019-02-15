namespace buggybuddy.Repositories
{
    public interface IDenialsRepository
    {
		void AddDenial(string user, string prospect);
	}
}
