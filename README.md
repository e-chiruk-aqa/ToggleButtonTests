**Test Automation - Job Assignment**

Welcome to the Test Automation assignment for the Test Automation Lead role. This assignment assesses your ability to develop a simple yet robust test framework and related tests to validate the functionality of similar executable files.

**Assignment Overview**

In this assignment, you are provided with three folders, each containing an executable file named `Toggle button.exe`. These files are nearly identical, with minor behavior differences:

- **ToggleButton** - This file is expected to pass all test cases.
- **ToggleButton_NoStart** - This file is expected to fail by not launching correctly.
- **ToggleButton_WrongBehavior** - This file is expected to fail due to incorrect functional behavior.

**The Goal** is to create 
- a test framework that enables testing of the given executables. Hint: Test framework should contain reusable pieces that enables implementation of future tests as well.
- create set of tests which actually verify the behavior of our input executables. 

**Instructions**
1. Build a test framework to validate the behavior of each `Toggle button.exe` file. You may choose any technologies or frameworks you find best suited for the task, with the exception of Capture and Replay tools. Our focus is on assessing your approach to creating lightweight, reusable, and reliable test code, so please prioritize clear, maintainable solutions over automation shortcuts. 
2. Test cases:
    - Click on the toggle button and verify the changes of the state as expected.
3. Error Handling: Ensure that the framework provides clear failure messages for non-passing cases, indicating the cause of failure (e.g., “Application did not launch” or “Toggle behavior is incorrect”).
4. Execution & Logging
    - Run the tests on all three executables.
    - Record outputs for each test case.
    - Include error logs and failure descriptions for non-passing executables.
5. Deliverables
    - Test Code: Submit the code for the test framework and the tests, structured and documented, making it easy to understand and execute.
    - Execution Instructions: Provide a guide on how to run the test framework, including any dependencies or specific setup required.
    - Test Report: Include a report detailing the results of running the test framework on each executable, with logs for both passing and failing cases.
    - Failure Messages: Ensure all failure cases include an informative message explaining the issue.
    - Environment Setup
        - Prerequisites.
        - Any required frameworks.
        - Environment configurations.
    - Instructions for running the tests
        - Clone the repository if applicable.
        - Install dependencies (commands for dependency installation).
        - Run the tests (command to run tests).
        - Evaluation Criteria

**Notes**

When executing `Toggle button.exe` files, you may encounter a "Windows protected your PC" warning due to the files not being signed. This may introduce test flakiness, as different test environments or host setups might display this prompt inconsistently.

To manually bypass this prompt:

Click on “More info.”
Select “Run anyway” to proceed with the application.
For reliable test outcomes, please ensure that your test framework or environment can handle such prompts consistently to prevent flakiness across different setups.


**You will be evaluated based on:**

**Correctness:** The test framework and the tests meets the assignment’s objectives.
**Code Quality:** The test framework and the tests should be well-structured, readable, and maintainable.
**Error Reporting:** Failure cases should include clear, informative messages.
**Documentation:** Instructions for running the tests should be clear and concise.