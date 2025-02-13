# Spacewar

**Spacewar** is a space shooter game developed using **Raylib** and **C#**. The player controls a spaceship and must survive by defeating incoming enemies while collecting gems to gain experience and score.

## 🛠️ Gameplay Mechanics
- The player pilots a **spaceship** and must destroy incoming **enemies**.
- There are **four types of enemies**:
  - **Basic:** Standard enemy with no special abilities.
  - **Fast:** Moves quickly but has low health.
  - **Strong:** Moves slower but has high health.
  - **Boss:** Appears at **1-minute mark**, has high health, and can shoot projectiles at the player.
- **Enemies drop gems** upon death:
  - **Gems grant EXP** to the player.
  - **Gems increase the player's score.**
  - **Defeating a Boss grants 1000 score instantly.**
- The player levels up by collecting EXP from gems and can choose to upgrade one of the following stats:
  - **Movement Speed**
  - **Attack Speed**
  - **Bullet Damage**
- The goal is to **eliminate as many enemies as possible within 5 minutes** while collecting gems to increase both EXP and score.

## 🛠️ Technical Details
- **Engine:** Raylib
- **Programming Language:** C#
- **Graphics Rendering:** Uses Raylib’s 2D rendering capabilities for sprites and animations.
- **Physics & Movement:** 
  - The spaceship movement is implemented with vector-based calculations to ensure smooth transitions.  
- **Collision Detection:** 
  - Implemented using circles for fast and efficient collision. 
- **Scoring System:** 
  - Gems increase the player’s EXP and score based on enemy type.
  - Boss enemies provide a significant score boost upon defeat.

## 💪 Strategy
- Prioritize collecting gems to level up faster and become stronger.
- Avoid enemy collisions and incoming **Boss projectiles**.
- Balance upgrades based on playstyle: **Speed for mobility, Attack Speed for rapid fire, Bullet Damage for stronger hits.**
- Survive until the end of the 5-minute duration while maximizing the score.



