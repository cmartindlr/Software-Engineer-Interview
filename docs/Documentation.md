# Technical Interview Solution - Tyler Carson

## Architecture
![Architecture of software components. Software is divided into four major sections: JSON Parsing, Answer Provision, Data Storage, and User 
Interface/Control.](Architecture.png "Architecture")

This is a fairly simple problem involving reading in a JSON and gathering information from the contents. 

It could be done in a much less modular way given its simplicity, but more care has been given in the design to demonstrate SOLID principles.

This was solved with a .NET 7 solution in C# written in Visual Studio 2022.

### JSON Parsing

First, the problem of JSON parsing must be solved. The Newtonsoft.Json package is a well-known package for this problem. It provides attributes 
to decorate C# classes with in order to deserialize JSON into an object properly.

Inspecting the JSON revealed a uniform pattern to the objects inside, so classes were made to represent those and only those, following the single 
responsibility principle. This includes the Registered Person and their Name. In addition, these data classes also have derived properties for the 
balance as a number and the formatted name given these values will be needed by users of the class.

The data classes for the JSON objects account for the fact that JSON does not enforce structured data and simply produce a null field for missing 
fields and logs the missing field, and ignores extra fields. This allows for compatability with future data structures derived from this one, both 
if the class gets extra fields later or the JSON sends extra fields and prevents data from being omitted.

Appropriate tests ensure proper integration with Newtonsoft.Json.


### Answer Provision

Next, the problem of getting answers to the questions was required.

Each question was answered by a fairly simple LINQ query on the objects provided from the JSON parsing.

To be modular, with single-responsibility principles applied, the questions were divided into subclasses that, through the Liskov substitution principle, 
can be placed into a list to get all answers that are desired. This allows for extending the set of questions by providing another IAnswerProvider in the 
list of questions to answer. Additionally, this uses a simple interface for the answer providers, following the Interface Segregation principle, preventing
the classes from including needless functions.

To implement the interface, the object must provide the ProvideAnswer(data) function that takes a data set and provides an answer to its question using that 
data. Any objects with relevant fields null are skipped.

In order to prevent coupling, the answer providers return both the question they answer as well as their answer so that the user can have all in one place 
without worrying about how to present the questions and answers together.

Each of these are tested with MSTest under a number of cases to ensure adhereance to expected behavior and prevent regressions.

In order to streamline the process of getting the answers, there is the interface and class for answer aggregation. This is injected with a list of answer 
providers, demonstrating the Dependency Inversion Principle of SOLID that allows for extending and testing, in order to provide answers to all of the questions
in an ordered manner. The implementation recognizes that each question is independent and runs threads for each, using the Task library from .NET to 
intelligently decide when to use a new thread. Even then, there is still some overhead on the usage of tasks, so a data set size threshold is also provided.

This is also unit tested using the Moq library to create generic answer providers.

### Data Storage

Then, persistent storage was considered.

SQLite is a simple database that runs alongside software as opposed to the traditional server model. This made sense to use in this case as the deployment 
environment was not known and there is no need for a full-scale database for this problem.

Entity Framework Core was used as an object-relational model to write to the SQLite database.

The database schema was provided using a code-first approach and data migrations, which are set up to automatically run and keep the database to the most 
current standard, as this seemed to be in spirit with the SQLite approach of having a database that just works for what it needs to do for this problem.

![Database schema used. Fields are AnswerRecordID (int), AnswerDate (DateTime), FileName (string), Question (string), and Answer (string).
](Schema.png "Database Schema")

The schema consists of an integer ID, date the information was retrieved, name for the file the data originated from, question, and answer. All of these 
fields would be relevant in storing data like this in a real-world senario and are included for that purpose.

The schema exists in the AnswerRecord class. Answer records can be retrieved and stored through the AnswerContext class. Answer contexts can be obtained 
through the AnswerContextFactory class, which is a type of DbContextFactory.

The responsibility of saving the answers, along with any other answer record data operations, falls on the AnswerRecordDataManager, which follows the same
SOLID principles as previously mentioned in its design as well as dependency inversion since it relies of the abstract IDbContextFactory to get the DbContext.
 This is specifically utilized in testing to create an in-memory database for testing.

### User Interface/Control

The control of the program is provided in two different projects depending on the flavor or interface desired. 

A console program is provided with a singular main method that runs the operations in sequence, instantiating the needed objects and displaying output to 
the console.

A Blazor server program is also provided that sets up dependencies through the service provider and injects them into the main page of Index. On load, it runs
the operations in order to get the data and store it and then notifies the client of the updated state to redraw the view.

## Project Structure

![Structure of projects within solution with descriptions of the purpose of each. Automated tests are for running automated tests on software components in the Backend project. Backend is for all of the reusable logical components that must be referenced in the frontend. There are both Console and Blazor solutions for presenting the data and running the program.](SolutionStructure.png "Solution Structure")

The solution is divided into four projects: Automated Tests, Backend, Blazor Solution, and Console Solution.

The automated tests are part of an MSTest project to ensure the correct operation of the backend components.

The core of the logic described in the previous section is located in the Backend project. This project contains all reusable functionality. The rationale for 
placing it in its own project was so that the console and Blazor solutions could co-exist. Additionally, the backend project could also be placed into a 
microservice with a little bit of adjustment and deployed to a cloud such as Azure by providing an API and setting up a service. Ultimately, the logistics of
setting up a microservice were too much to tackle in a reasonable timeframe without expert knowledge of Azure deployment, so this benefit was never fully 
realized. Despite this, the potential still exists.

The Blazor solution contains all of the code strictly related to running a Blazor app. This also has docker containers set up to support deployment to the cloud 
even though that was never implemented. Docker containers also provide a better indication of the production environment, so that benefit is also present. This 
can be run either in a container through the Docker run option or with IIS express as is typical.

Finally, the console solution contains the console program and can also be run.
