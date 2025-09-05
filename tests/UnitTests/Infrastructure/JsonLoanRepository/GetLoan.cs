using Library.Infrastructure.Data;
using Library.ApplicationCore;
using Library.ApplicationCore.Entities; // Add this line or adjust to the correct namespace for Loan
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Library.UnitTests.Infrastructure.JsonLoanServiceTests;

public class GetLoanTest
{
    private readonly ILoanRepository _mockLoanRepository;
    private readonly JsonLoanRepository _jsonLoanRepository;
    private readonly IConfiguration _configuration;
    private readonly JsonData _jsonData;

    public GetLoanTest()
    {
        // Build configuration for JsonData
        _configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json", optional: true)
            .Build();

        _jsonData = new JsonData(_configuration);
        _jsonLoanRepository = new JsonLoanRepository(_jsonData);

        // _mockLoanRepository can be set up with a mocking framework if needed in future tests
        // For now, just assign null or a mock as appropriate for your test setup
        _mockLoanRepository = Substitute.For<ILoanRepository>();
    }

    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Returns loan when loan ID exists")]
    public async Task GetLoan_ReturnsLoan_WhenLoanIdExists()
    {
        // Arrange
        int existingLoanId = 1; // This ID exists in Loans.json
        var expectedLoan = new Loan
        {
            Id = existingLoanId,
            BookItemId = 17,
            PatronId = 22,
            LoanDate = DateTime.Parse("2023-12-08T00:40:43.1808862"),
            DueDate = DateTime.Parse("2023-12-22T00:40:43.1808862"),
            ReturnDate = null
        };

        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(existingLoanId);

        // Assert
        Assert.NotNull(actualLoan);
        Assert.Equal(expectedLoan!.Id, actualLoan!.Id);
    }
    [Fact(DisplayName = "JsonLoanRepository.GetLoan: Returns null when loan ID does not exist")]
    public async Task GetLoan_ReturnsNull_WhenLoanIdDoesNotExist()
    {
        // Arrange
        int nonExistingLoanId = 9999; // This ID does not exist in Loans.json

        // Act
        var actualLoan = await _jsonLoanRepository.GetLoan(nonExistingLoanId);

        // Assert
        Assert.Null(actualLoan);
    }
}
