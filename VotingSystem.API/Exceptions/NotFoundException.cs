namespace VotingSystem.API.Exceptions
{
    public class NotFoundException:CustomException
    {
        public NotFoundException(string message) : base(message) { }
    }
}
