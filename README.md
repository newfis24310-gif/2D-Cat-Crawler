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

## 🎨 Assets & Credits

### 🖋 Font
**Medodica** by Roberto Mocci  
Copyright (c) 2024 Roberto Mocci  
Licensed under the **SIL Open Font License 1.1**  
Reserved Font Name: **Medodica**

- Author / contact: postocarattere@gmail.com
- Author page: https://patreon.com/rmocci
- OFL reference: http://scripts.sil.org/OFL

> The full font license text is included in the project files.

---

### 🔊 Sound Effects
Sound effects sourced from **Pixabay**:

- Cardboard box close  
- Cardboard box handling / movement  
- Cardboard box drop hit  
- Box opening  
- Dropping cardboard box  
- Cardboard box open / close  
- Opening package / parcel box  
- Robotic movement  
- Dark horror ambience  
- Gas decompression  
- Notification / alert  
- Footsteps on stone floor  
- Lab bottles bubbling ambience  
- Cat meow effects  
- Cat purring  
- Computer lab ambience  
- Eating sound  
- Additional cat vocalizations  
- Keyboard typing sound  

Source links:
- https://pixabay.com/sound-effects/film-special-effects-cardboard-box-close-182562/
- https://pixabay.com/sound-effects/film-special-effects-cardboard-box-73023/
- https://pixabay.com/sound-effects/film-special-effects-cardboard-box-drop-hit-handling-32135/
- https://pixabay.com/sound-effects/film-special-effects-box-open-90674/
- https://pixabay.com/sound-effects/dropping-cardboard-box-453026/
- https://pixabay.com/sound-effects/film-special-effects-cardboard-box-open-close-86303/
- https://pixabay.com/sound-effects/film-special-effects-open-package-box-parcel-100334/
- https://pixabay.com/sound-effects/technology-28-movimientosroboticos-29880/
- https://pixabay.com/sound-effects/horror-dark-horror-ambient-05-425468/
- https://pixabay.com/sound-effects/film-special-effects-gas-decompression-329819/
- https://pixabay.com/sound-effects/film-special-effects-notification-alert-8-331718/
- https://pixabay.com/sound-effects/film-special-effects-2-persons-walking-on-stonefloor-28579/
- https://pixabay.com/sound-effects/film-special-effects-lab-bottles-bubbling-ambience-211721/
- https://pixabay.com/sound-effects/cat-meow-fx-461188/
- https://pixabay.com/sound-effects/nature-real-cat-purring-sound-354510/
- https://pixabay.com/sound-effects/technology-computer-lab-33482/
- https://pixabay.com/sound-effects/people-eat-353533/
- https://pixabay.com/sound-effects/nature-cute-cat-meow-472372/
- https://pixabay.com/sound-effects/nature-cat-meow-321642/
- https://pixabay.com/sound-effects/nature-262312-steffcaffrey-cat-meow1-80256/
- https://pixabay.com/sound-effects/film-special-effects-keyboard-typing-one-short-1-292590/

---

### 🛠 Tools
- Unity
- Yarn Spinner
- FMOD Studio
- TextMeshPro

---

### 🛠 Tools
- Unity Engine
- Yarn Spinner
- FMOD Studio

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
