# Enemy Wave Survival

A first-person wave survival prototype made for the SVR Robotics assignment.
Built in Unity 2022.3.62f3 with C#.

## How to Run

1. Open the project in Unity 2022.3.62f3.
2. Open the Game scene.
3. Press Play.

## Controls

- WASD — Move
- Mouse — Look
- Left Click — Fire

## Architecture

### InputReader

The new Input System is used for input. InputReader is the only class that
references the Input System — it reads the actions and exposes them as plain
variables (move direction, look delta, firing state). Any class that needs
input reads from InputReader, so swapping keyboard for touch controls would
touch a single file.

### Player Split

Player is divided into PlayerMover, PlayerLook, PlayerShooter, and a small
Player orchestrator. Each class has one responsibility:

- PlayerMover only cares about which direction to move.
- PlayerLook only cares about looking.
- PlayerShooter handles firing the projectile, without touching movement or look.
- Player reads input and passes it to mover and look.

Each part works independently, which keeps them simple to test and extend.

### Events & Channel

Plain C# events wire a component to its direct listeners — for example
Health.Damaged updates the UI and plays a sound. `EnemyDiedChannel` is a
ScriptableObject that holds an event, so any system (ScoreManager, WaveManager,
AudioManager) can react to an enemy death without holding a reference to the
Enemy itself. Raising side and listeners never touch each other, which keeps
the systems decoupled and easy to extend.

### ScriptableObjects

EnemyData and WaveData are ScriptableObjects used as data containers. Enemy
types (fast/weak, slow/tanky) and wave tuning can be balanced by tweaking
values in the inspector without touching any code. The difficulty scaling
parameters also live in WaveData.

### Pooling

Projectiles and enemies are pooled through a generic `Pool<T>`. The pool
prewarms instances and grows if no free object is available, which avoids
Instantiate/Destroy spikes — important with many enemies alive in higher
waves. Since instances are reused, `Init` fully resets their state — health,
timers, material, dead flag — which prevents stale-data bugs such as a
respawned enemy keeping its previous life's health bar.

### Waves & Difficulty

Spawn points are manually placed to cover the map; each spawn picks a random
point with a ring offset and NavMesh sampling, keeping a minimum distance from
the player. A maxAlive cap keeps performance within limits in crowded waves.
The agents' repath timer is randomised so all agents never calculate paths on
the same frame, avoiding performance spikes. Difficulty increases every wave:
speed, health and damage are multiplied per wave, with caps so the game stays
playable in later waves.

## Known Simplifications

- A formal state machine for enemy behaviour (currently guard-based chase/attack)
- Mobile touch controls (input is already abstracted behind InputReader)
- Basic animations
- 2D audio; positional audio would be the next step
