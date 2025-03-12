namespace VotingSystem.API.Exceptions
{
    public class DuplicateEntryException:CustomException
    {
        public DuplicateEntryException(string message) : base(message) { }
    }
}
