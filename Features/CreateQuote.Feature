Feature: Quote API
Scenario: Create A Valid Quote
Given I have a valid quote request
When I create the quote
Then the API should return 201
And the quote status should be "Created"