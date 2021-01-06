# Battleships
Simple console version of Battleships game. It allow a single human player to play a one-sided game of Battleships against ships placed by the computer.
The game creates a 10x10 grid and generates ships on the grid at random - 1x Battleship (5 squares), 2x Destroyers (4 squares).

The player enters coordinates of the form <b>A5</b> (where <b>A</b> is the column and <b>%</b> is the row) to specify a square to target. 
Shots result in hits, misses or sinks. The game ends when all ships are sunk.

## Technologies

The program was developed using C# and .NET Core 3.1. Unit tests were written with the help of xUnit and Fluent Assertions.

## How to build and run

The straightforward way is to open the solution in Visual Studio and build and run it there. 
If you do not have .NET Core 3.1 runtime installed, Visual Studio should prompt you to download and install it.
If it has not, head to https://dotnet.microsoft.com/download/dotnet-core/3.1 to download and install the runtime on your machine.

### Docker

You can also run the app as a Docker image using attached Dockerfile.
Just go to the directory where the Dockerfile resides and build the image using the following command:

<code>docker build . -t [docker_image_name]</code>

Then you run the image using:

<code>docker run -it [docker_image_name]</code>
