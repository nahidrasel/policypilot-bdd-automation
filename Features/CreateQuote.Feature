Feature: Quote API

Scenario: Create a valid quote
	Given I have a valid quote request
	When I create the quote
	Then the API should return 201
	And the quote should have an identifier
	And the quote premium should be positive
	And the quote status should be populated

Scenario: MockAPI accepts an invalid registration without validation
	Given I have a quote request with an invalid registration
	When I create the quote
	Then the API should return 201
	And the response should include the invalid registration