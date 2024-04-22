# Instructions to Run:

You must have the .NET runtime installed.

Open a terminal to the `~/ConsoleApp` directory and run `dotnet bin/Release/net8.0/ConsoleApp.dll`


# Sample output:
![Sample Output](Sample%20Output.png)
<!-- Use %20 to escape space in the file name -->

# Technical note:
I implemented a separate algorithm for each question for the sake of readability and reusability. For example, rather than loop over the individuals once to sum up their combined balance and simultaneously track most common eye colors as well, I implemented 2 separate loops: one which sums up the combined balance and another which tracks most the most common eye colors.

# What would I have next with more time?
I would have developed a cross-platform front end using .NET MAUI (native) or ASP.NET Core and Blazor (web-based).
