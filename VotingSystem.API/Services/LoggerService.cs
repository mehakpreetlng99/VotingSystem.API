using VotingSystem.API.Services.Interface;
namespace VotingSystem.API.Services
{
    public class LoggerService:ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }
        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
        public void LogError(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }
    }
}
