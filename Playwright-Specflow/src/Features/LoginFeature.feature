@UI
Feature: Login to Sauce Labs
    Saucelabs Login

    Scenario: TC001_Successful login
        Given I am on the Sauce Labs login page
        When I log in with username "standard_user" and password "secret_sauce"
        Then I should be logged in successfully

    Scenario Outline: TC002_Invalid login attempts
        Given I am on the Sauce Labs login page
        When I log in with username "<username>" and password "<password>"
        Then I should see an error message "<errorMessage>"

        Examples:
            | username        | password       | errorMessage                                                |
            | locked_out_user | secret_sauce   | Epic sadface: Sorry, this user has been locked out          |
            | standard_user   | wrong_password | Username and password do not match any user in this service |
            | invaliduser     | secret_sauce   | Username and password do not match any user in this service |