namespace VotingSystem.API.Exceptions
{
    public class UnauthorizedException:CustomException
    {
        public UnauthorizedException(string message) : base(message) { }
    }
}
