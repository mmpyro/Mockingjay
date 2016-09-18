Feature: MockingJayFeature
	MockingJay is an application which allows to register http requests and send it back.
	Some kind of mock, but for REST services.

Background: 
	Given I have http request for register mock response
	When I send it into mockingjay

@acceptance
Scenario: Register request
	Then the register response Status code is Created and contains headers
	| Name     | Value                        |
	| Location | http://localhost:51111/users |

@acceptance
Scenario: Register the same request again
	Given I have http request for register mock response
	When I send it into mockingjay
	Then the response Status code is InternalServerError

@acceptance
Scenario: Get all requests
	Given I have http request for get all mock requests
	When I send it into mockingjay
	Then the response Status code is OK

@acceptance
Scenario: Remove single registration
	Given I have http request for remove signle registered configuration
	When I send it into mockingjay
	Then the response Status code is NoContent

@acceptance
Scenario: Get all registered configurations
	Given I have http request for get all registered configuration
	When I send it into mockingjay
	Then the response Status code is OK

@acceptance
Scenario: Remove all request
	Given I have http request for remove all mock response
	When I send it into mockingjay
	Then the response Status code is NoContent

@acceptance
Scenario: Verify registered request
	Given I have http request for ask about registered message
	When I send it into mockingjay
	Then the response Status code is OK