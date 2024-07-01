@Checkout
Feature: Checkout on Sauce Labs
    Saucelabs Checkout

  Scenario Outline: Checkout products with different user details
    Given I am logged in with valid credentials
    When I add <product> to the cart from inventory page
    And I proceed to checkout
    And I enter user details:
      | First Name | Last Name | Zip Code |
      | <firstName> | <lastName> | <zipCode> |
    Then I should complete the checkout successfully

    Examples:
      | product               | firstName  | lastName   | zipCode   |
      | Sauce Labs Backpack   | John       | Doe        | 12345     |
      | Sauce Labs Bike Light | Jane       | Smith      | 54321     |
      | Sauce Labs Bolt T-Shirt| Michael    | Johnson    | 98765     |