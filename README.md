# ArenaDodgeGame

## Project Description
Arena Dodge is a competitive 2-player multiplayer arena game built in Unity using Netcode for GameObjects. Players compete in a small arena by dodging incoming projectiles while attempting to eliminate their opponent.

The game focuses on fast-paced movement, reaction time, and real-time multiplayer interaction. Players must balance offense and defense while reacting quickly to survive.

This project demonstrates multiplayer networking, event-driven programming, design patterns, and database integration.

## How to play

### Objective
Be the last player alive by avoiding enemy projectiles and hitting your opponent.

### Controls
Move Left: Left arrow
Move Right: Right arrow
Jump: Space
Shoot: Up arrow
Pause Menu: Escape

## Setup Instructions

### 1. Clone the Repository
git clone https://github.com/VictoriaGKnight/ArenaDodgeGame.git

### 2. Open in Unity
- Open Unity Hub
- Click "Open Project"
- Select the cloned folder
- Use Unity version: (your version, e.g., Unity 6 or 2022.x)

## How to Test Multiplayer

### Step 1 — Build the Game
1. Open Unity
2. Go to File → Build Settings
3. Make sure scenes are added in this order:
   - MainMenu
   - GameScene
   - GameOver
4. Click "Build"
5. Save the executable

### Step 2 — Run Multiplayer

#### Host (Unity Editor)
1. Press Play
2. Click "Host"

#### Client (Built Game)
1. Open the built .exe file
2. Enter IP address:
   127.0.0.1
3. Click "Join"

### Expected Behavior
- Two players spawn in the arena
- Players can move, jump, and shoot
- Projectiles damage opponents
- Game ends when one player is eliminated

## Project Structure
### Assets
Scripts
- GameManager.cs
- PlayerNetworkController.cs
- NetworkMatchManager.cs
- AudioManager.cs
- DatabaseManager.cs
- PauseMenuManager.cs
Scenes
- MainMenu
- GameScene
- GameScene2
- GameOver
Prefabs
UI
Audio

## Technical Requirements Implemented

### Singleton Pattern
- GameManager.cs → manages game state (health, score, winner)
- AudioManager.cs → handles music and sound effects
- DatabaseManager.cs → manages SQLite database

### Delegates / Event System
- GameManager.cs:
  - onHealthChanged
  - onScoreChanged
- UIManager.cs and AudioManager.cs subscribe to these events

### Additional Design Pattern
Object pool pattern used for projectiles to reuse objects instead of instantiating and destroying them repeatedly.

### Database Integration (SQLite)
Implemented in DatabaseManager.cs Handles creating a HighScores table, saving scores, and retrieving top scores.

### Save / Load System
Implemented using PlayerPrefs in AudioManager.cs. Saves and loads master, music, and sound effect volume settings between play sessions. 

### Audio System
- AudioManager.cs:
  - background music
  - shoot sound
  - damage sound

## 📂 Key Scripts Location
Assets/Scripts/

Important files:
- GameManager.cs
- PlayerNetworkController.cs
- Projectile.cs
- NetworkMatchManager.cs
- MenuManager.cs
- AudioManager.cs
- DatabaseManager.cs
- GameOverManager.cs

## Known Issues
- Multiplayer testing requires running both Unity Editor and a built executable.
- Music controls sometimes cause music and sound effects to have bugs.

## Technologies Used
- Unity Engine
- C#
- Unity Netcode for GameObjects
- SQLite (Mono.Data.Sqlite)
- PlayerPrefs

## Author
Victoria Knight

## Notes
This project was created for a Unity/C# game development course and demonstrates multiplayer networking, design patterns, and system architecture.
