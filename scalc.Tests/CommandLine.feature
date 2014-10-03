Feature: CommandLine
	In order to calculate numbers from the command line
	As a user
	I want to calculate the sum of two numbers

Scenario: Add two numbers
	Given I have launched scalc with the command line "1,2"
	When I wait for output
	Then the screen should display "The result is 3"

Scenario: Add more than one set of numbers
	Given I have launched scalc with the command line "1,2"
	When I wait for output
	Then the screen should display "The result is 3"
	When I enter "2,3"
	And I wait for output
	Then the screen should display "The result is 5"

Scenario: Terminate the application by entering an empty line
	Given I have launched scalc with the command line "1,2"
	When I wait for output
	Then the screen should display "The result is 3"
	When I enter ""
	And I wait for output
	Then the application should end.

Scenario: Entering negative numbers should display an error
	Given I have launched scalc with the command line "1,-2"
	When I wait for output
	Then the screen should display "negatives not allowed. Invalid numbers:"
	And the screen should display "-2"

Scenario: Users can supply alternate delimiters
	Given I have launched scalc with the command line "4,5"
	When I wait for output
	Then the screen should display "The result is 9"
	When I enter "//[;]"
	And I enter "2;3"
	And I wait for output
	Then the screen should display "The result is 5"

Scenario: Users can supply multiple alternate delimiters
	Given I have launched scalc with the command line "4,5"
	When I wait for output
	Then the screen should display "The result is 9"
	When I enter "//[;][:]"
	And I enter "2;3:5"
	And I wait for output
	Then the screen should display "The result is 10"
