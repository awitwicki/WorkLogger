namespace WorkLogger.Domain.ConfigModels;

public class ConfigModel
{
    private readonly List<string> _errorMessages = new();
    public IReadOnlyList<string> ErrorMessages => _errorMessages.AsReadOnly();
    public bool HasErrors => ErrorMessages.Any();
    
    private void AddEnvNotDeclaredMessage(string envName)
    {
        _errorMessages.Add($"Environment '{envName}' is not declared!");
    }
    
    private void AddEnvHasWrongValueMessage(string envName, string value)
    {
        _errorMessages.Add($"Environment '{envName}' have wrong value '{value}'!");
    }
    
    public ConfigModel()
    {
        // SmtpHost = Environment.GetEnvironmentVariable("SmtpHost")!;
        // var smtpPortStr = Environment.GetEnvironmentVariable("SmtpPort");
        // SmtpUsername = Environment.GetEnvironmentVariable("SmtpUsername")!;
        // SmtpPassword = Environment.GetEnvironmentVariable("SmtpPassword")!;
        // EmailFrom = Environment.GetEnvironmentVariable("EmailFrom")!;
        //
        // if (string.IsNullOrWhiteSpace(SmtpHost))
        //     AddEnvNotDeclaredMessage(nameof(SmtpHost));
        //
        // if (string.IsNullOrWhiteSpace(smtpPortStr))
        //     AddEnvNotDeclaredMessage(nameof(SmtpPort));
        // else if (int.TryParse(smtpPortStr, out var port))
        //     SmtpPort = port;
        // else
        //     AddEnvHasWrongValueMessage(nameof(SmtpPort), smtpPortStr);
        //
        // if (string.IsNullOrWhiteSpace(SmtpUsername))
        //     AddEnvNotDeclaredMessage(nameof(SmtpUsername));
        //
        // if (string.IsNullOrWhiteSpace(SmtpPassword))
        //     AddEnvNotDeclaredMessage(nameof(SmtpPassword));
        //
        // if (string.IsNullOrWhiteSpace(EmailFrom))
        //     AddEnvNotDeclaredMessage(nameof(EmailFrom));
    }
}
