# 🐱 2D Cat Dungeon Crawler  
Inspired by Schrödinger’s Cat Thought Experiment

<img width="500" height="500" alt="coverart" src="https://github.com/user-attachments/assets/809290cd-4efa-4c58-a5d9-f2d2d045dbd4" />



A 2D grid-based puzzle game built in Unity where the player controls a cat navigating a mysterious dungeon full of hidden dangers and uncertain outcomes.

---

## 🧠 Concept

This game is inspired by the famous Schrödinger’s Cat thought experiment.

The core idea is **uncertainty**:
- The player does not initially understand how the game works.
- Through experimentation and observation, the player gradually learns how to survive.

> The game is designed so that knowledge is *discovered*, not given.

---

## 🎮 Gameplay

- The player moves across a grid by clicking adjacent tiles.
- Each tile may hide:
  - Safe space
  - Dangerous elements
  - Interactive items

### Core Goal
Reach the **Exit Tile**, guided by a robotic mouse.

However:
- The outcome remains uncertain until the very end.
- The player may already be “dead” without knowing it.

---

## 🧩 Mechanics

- 🐟 **Fish**  
  Can either revive or kill the player depending on their current state.

- ☠️ **Gas**  
  Accumulates and becomes lethal after multiple exposures.

- 🚪 **Exit Tile**  
  Determines the final outcome (win or fail).

- 🐭 **Robot Mouse**  
  Leads the player toward the exit and reveals the final state.

---

## 💬 Narrative Elements

- Two scientists observe the experiment and communicate through dialogue.
- Their speech provides **subtle hints**, but never explicit instructions.

---

## 🧠 Design Philosophy

- No explicit tutorial
- Learning through trial and error
- Hidden state (alive/dead not always obvious)
- Player-driven discovery of rules

---

## 🧱 Architecture

The project is built around a modular gameplay architecture with clear separation between grid generation, player logic, item interactions, and overall game flow.

- `GameManager` handles attempts, win/fail conditions, and round progression.
- `GridManager` generates the grid, places special tiles, and assigns items.
- `Player` manages movement, state, and item usage.
- `BaseTile` defines shared tile behavior, while specialized tiles such as `EmptyTile`, `ActionTile`, and `ExitTile` implement different gameplay roles.
- `Item` serves as the base class for interactive objects such as `FishItem` and `GasItem`.
- Additional systems such as dialogue, UI, sound, and visual feedback are handled through dedicated managers and helper components.

This structure makes the project easier to extend with new tiles, items, and gameplay mechanics.

## 🛠 Technologies Used

- Unity (C#)
- Yarn Spinner (dialogue system)
- FMOD (audio system)
- TextMeshPro (UI)

---

## 🎥 Demo


https://github.com/user-attachments/assets/5c8e7150-48d2-470c-a3b7-d44d1c281281


---

## ▶️ How to Run

1. Clone the repository
2. Open the project in Unity
3. Load the main scene
4. Press Play

---

## 📄 License

This project is for educational and portfolio purposes.
