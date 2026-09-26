public static class QuoteTestData
{
    public static QuoteRequest ValidQuote()
    {
        return new QuoteRequest
        {
            CustomerId = "C1001",
            RegistrationNumber = "ABC123",
            CoverType = "Comprehensive"
        };
    }

    public static QuoteRequest InvalidRegistrationQuote()
    {
        return new QuoteRequest
        {
            CustomerId = "C1001",
            RegistrationNumber = "INVALID",
            CoverType = "Comprehensive"
        };
    }
}