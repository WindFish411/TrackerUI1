# TrackerUI1

///////////////
Purpose in creating this program:
I created this program in order to develop my programming skills. Building this program from the ground up helped me learn the entire program design process
of creating an application in C#. An additional skill that I had to learn in order to complete the project is usuing SQL to create and store data properly in
a database. 

This program was created using Tim Corey's free Youtube course "Create a C# Application from Start to Finish - Complete Course".
///////////////

///////////////
Big Picture Design:
Structure: Windows Forms application and Class Library
Data: SQL and/or Text File
Users: One at a time on one application
///////////////

///////////////
Key Concepts:
-SQL (data tables, stored procedures)
-Custom events
-Error Handling
-Interfaces
-Random Ordering
///////////////

///////////////
To run the program: 
The program boots from Visual Studio, first opening the Tournament Dashboard. Here you can load an existing tournament or create a new one. You will need to
enter the players names, team names, as well as some other information to create a tournament. Once a tournament is created, you can score it, which will 
determine a winner or loser and will automatically populate the next round of the tournament. At any time you can quit out and load a tournament and upate it. 
Since this program was created for learning purposes, and not practical application, I have not eliminated every bug, so some of the rounds after a second round
may not load correctly. 

Note: To store data in SQl database you will need a database with the proper tables and stored procedures. If you prefer, you can just store to text file by
changing the following found in program.cs: 
TrackerLibrary.GlobalConfig.InitializeConnection(DatabaseType.Sql); 
to the following: 
TrackerLibrary.GlobalConfig.InitializeConnection(DatabaseType.Txt);
///////////////

///////////////
Mapping Data

Team
-TeamMembers(List<Person>)
	//Could be 1 person to multiple person teams
-TeamName(string)

Person
-FirstName(string)
-LastName(string)
-EmailAddress(string)
-CellphoneNumber(string)

Tournament
-TournamentName(string)
-EntryFee(decimal)
-EnteredTeams(List<Team>)
	//List of type team. 
-Prizes(List<Prize>)
-Rounds(List<List<Matchup>>)
	//We have a matchup and a set of matchups for each round. 

Price
-PlaceNumber(int)
	//1st, 2nd, 3rd place etc. 
-PlaceName(string)
-PriceAmount(decimal)
-PricePercentage(double)

Matchup
-Entries(List<MatchupEntry>)
-Winner(Team)
-MatchupRound(int)

MatchupEntry
-TeamCompeting(Team)
-Score(double)
-ParentMatchup(Matchup)
///////////////
