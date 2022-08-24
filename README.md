**Annotation:** Game with characters from the animated movie "Tom & Jerry". The user plays as a mouse that jumps between compartments and simultaneously shoots cats.

# Documentation
## Starting the game
Windows Forms is included by default in Mono (Linux) and .Net (Windows).
**Windows:** Create a Windows Forms project. You can start the game with the Play button or the F5 key.
**Linux:** Type `csc game.cs -r:System.Windows.Forms.dll` and then `mono game.exe` in the terminal.

## Game modes
The game supports 2 modes "Easy" and "Hard", which differ in game speed.

## Instruction
The main condition of the game is not to touch the compartments and the cat. With the help of the SPACE key, the user can shoot cats and get points for it. With the UP key, the user can jump between compartments. If he loses, he can start a new game using the ENTER key. When the score of the game is increased, the speed of the bins and the cat also increases.

## Program structure
At the beginning, the user sees a main menu with two buttons "EASY" and "HARD" and instructions. The ``class StartScreen`' is responsible for the main menu. If the user presses one of those buttons, the game will start. The difference between the buttons is the speed of the cat and the tray and the image of the mouse during the game. The ``class MyForm`', which has 8 basic methods, is responsible for the game. `MainEvents` is responsible for the main changes during the game (for example, the movement of the mouse, bins and cat, shooting and collisions between objects). `KeyIsDown` and `KeyIsUp` are responsible for key presses and the resulting changes. `New Game` and `EndOfGame` are responsible for the start and end of the game. `BulletTrue` and `BulletFalse` are responsible for displaying bullets. `ChooseCat` will respond to the display of a cat.
