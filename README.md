Mythos Unleashed

Mythos Unleashed is a Unity-based gameplay systems project exploring dynamic world-state design, AI-driven behavior, and diegetic player interaction.

The project focuses on building cohesive gameplay loops where player actions directly influence the environment, NPC behavior, and overall game state, rather than relying on scripted or isolated mechanics.

⸻

Core Systems Overview

Dynamic World-State System (Global Harmony Index)

A centralized system that tracks player interaction and propagates changes across the game world.

* Influences NPC behavior and environmental response
* Modifies gameplay feedback based on player actions
* Acts as a shared state layer connecting gameplay systems

This system is designed to simulate a living world where the player’s presence has continuous and systemic impact.

⸻

AI-Driven Behavior Systems

Gameplay behaviors are built to respond dynamically to both player input and world-state conditions.

* NPC logic adapts based on global state and local interaction
* Systems prioritize reactivity over scripted sequences
* Designed to support scalable behavior expansion

⸻

Procedural Environment Generation

A modular 3D maze generation system built using prefab-driven architecture.

* Room-based generation with controlled randomness
* Modular wall, doorway, and traversal logic
* Designed for extensibility (new room types, layouts, and systems)

⸻

Diegetic UI (Cognitive State Interface)

A UI system embedded directly into gameplay, eliminating traditional menus.

* Player actions trigger in-world UI states
* “Living pause” system maintains environmental continuity
* Interaction feedback is integrated into animation and world response

This approach prioritizes immersion and reduces separation between player and system.

⸻

Design Philosophy

This project is built around a systems-first approach to gameplay design:

* Player interaction drives world behavior
* Systems are interconnected, not isolated
* Feedback is embedded into the world, not layered on top

Rather than guiding the player through explicit instruction, the goal is to create systems that feel intuitive, reactive, and discoverable through interaction.

⸻

Technical Stack

* Unity (C#)
* Universal Render Pipeline (URP)
* Object-Oriented Architecture
* Modular System Design
* Git (Version Control)

⸻

Development Approach

* Systems are designed for scalability and modular expansion
* Architecture supports future AI, narrative, and multiplayer integration
* Iterative development with focus on gameplay feel and responsiveness

⸻

Repository Focus

This repository highlights:

* Gameplay system architecture
* Core interaction loops
* Experimental system design approaches

⸻

Status

Active development. Systems are continuously being expanded and refined.