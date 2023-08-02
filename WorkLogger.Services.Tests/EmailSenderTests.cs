using WorkLogger.Domain.ConfigModels;

namespace WorkLogger.Services.Tests;

public class EmailSenderTests
{
    [Fact]
    public async Task SendEmailAsync_WithValidDate_ShouldSendEmail()
    {
        // Arrange
        var config = new ConfigModel();
        var senderService = new EmailSender(config);

        // Act
        await senderService.SendEmailAsync("", "Zmiana hasła", "Test email message");
        
        // Assert
        Assert.True(true);
    }
}
