# The Game of Life

## Introduction

The Game of Life is a cellular automaton simulation that explores the patterns that can arise from a few rules. The game and its rules were created by mathematician John Conway in 1970. The simulation consists of a grid of cells that can be in either a dead or alive state. The next state of each cell is determined by the number of alive and dead neighbors it has. While it is not typically considered a conventional game, this project aims to implement the cellular generation of Conway’s Game of Life and introduce a puzzle exploration aspect to it. In this iteration the player is encouraged to explore the evolution of each initial pattern they create.

## Game Design

### Game Manager

The game uses the singleton pattern by creating a game manager instance that has a game state property. The manager is responsible for managing scene transitions and retaining data that needs to be persistent from scene to scene. The game has four scenes, a main menu, a rules scene, a game settings menu, and a game board scene.

### Game States

- Main Menu: The initial state of the game on launch. It provides players with a choice to either start a game, go to the game settings menu, the rules page, or exit the game.
- Rules: Shows a description of how the game is played and what the controls of the game are.
- Game Settings: A menu where players can change the dimensions of the game board.
- Setup: The first state of a play session where players set up the pattern of live cells they want to see evolve. Players can left click on any cell to turn it into a live cell or right click to turn it to a dead cell. They are given a max of 20 cells to place and must place at least 3. They can press R to reset the board and Spacebar to enter the play state.
- Play: The simulation evolves over time and the player can press E to increase the simulation speed or Q to decrease the simulation speed. The player can press Spacebar to enter the pause state.
- Paused: The evolution of the simulation is stopped and the player can press Spacebar again to enter the play state again.
- Victory: When the player reaches the 50th generation or achieves a population of 50 live cells on the board the player wins and is prompted to either continue the simulation or reset the board and go back to the setup state.
- Game Over: When the population is reduced to zero, the game enters the game over state and players are prompted to return to the setup state.

### Cell Management

- The game board consists of a two dimensional grid
- Each cell has two possible state: dead or alive
- The state of a cell is determined by states of its eight neighbors
- If a cell is alive and has two or three live neighbors it lives in the next generation otherwise it dies
- If a cell is dead and has exactly three live neighbors it becomes a live cell in the next generation

### Credits

- Music: Airplane Mode by Yokonap

Thank you for exploring the Game of Life Project. We hope you enjoy experimenting with cellular automata and observing the fascinating patterns that emerge from simple rules!