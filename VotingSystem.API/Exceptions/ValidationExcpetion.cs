namespace VotingSystem.API.Exceptions
{
    public class ValidationExcpetion: CustomException
    {
        public ValidationExcpetion(string message) : base(message) { }
    }
    
    
}
