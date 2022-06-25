namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        string GetUsername();
		string GetEmail();
		string GetRole();
		Guid GetID();
    }
}