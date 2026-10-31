# SYRAX — Immersive Fraud-Awareness Experience

> An interactive Unity experience that trains people to recognize and resist financial scams through realistic, AI-personalized scenarios: a phone call, a website, and a text message.

![Unity](https://img.shields.io/badge/Unity-6000.3-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-.NET-512BD4?logo=csharp)
![URP](https://img.shields.io/badge/Render%20Pipeline-URP-blue)
![Status](https://img.shields.io/badge/status-prototype-orange)

---

## Overview

SYRAX places the user inside three common fraud situations and asks them to make decisions in real time. A backend server builds a **session** for each user, including a risk profile, known weaknesses and a tailored stage order. Unity then renders each stage, collects the user's decisions and reports them back for scoring.

| Stage | What the user faces |
|-------|---------------------|
| 📞 **Phone Call** | A caller impersonating a trusted institution |
| 🌐 **Website** | A convincing fake page with a suspicious URL |
| 💬 **Message** | A phishing SMS / chat message with a call to action |

At the end, the user receives a final score (out of 15) and a completion message.

## Features

- **Server-driven sessions:** the user profile, risk profile and stage content are fetched from a REST API (`/api/unity/session`).
- **Personalized scenarios:** each stage carries an actor name, opening text, body, call-to-action and a fake URL.
- **Decision scoring:** 5 points per correct decision, 15 points maximum.
- **Smooth stage transitions:** fade-in/out with a CanvasGroup and animated stage entrance.
- **Scene routing:** `SyraxServerConnector` can load a dedicated scene per stage type.

## Tech Stack

- **Engine:** Unity 6 (6000.3.8f1), Universal Render Pipeline
- **Language:** C#
- **Networking:** `UnityWebRequest` + `JsonUtility`
- **UI:** uGUI + TextMeshPro
- **Input:** Unity Input System
- **Backend (separate repo):** Node.js REST API with AI-generated scenarios

## Project Structure

```
Assets/
├── SyraxExperienceManager.cs   # Stage flow, decisions, scoring, transitions
├── SyraxServerConnector.cs     # Fetches session JSON and routes to stage scenes
└── Scenes/SampleScene.unity
```

## Getting Started

1. Clone the repo and open the folder with **Unity 6000.3.8f1** (or a newer 6000.3.x).
2. Start the SYRAX backend server on port `3000`.
3. In the Inspector, set **Server Base URL** on `SyraxExperienceManager`:
   - Editor on the same PC: `http://localhost:3000`
   - Headset / another device: `http://<your-PC-IP>:3000`
4. Press **Play**.

> 🔐 **Security:** this Unity client contains **no API keys**. All AI calls happen on the backend, which reads its keys from a local `.env` file that is git-ignored.

## My Role

<!-- ✏️ Edit this section to match your exact contribution -->
- Designed the experience concept and the three-stage fraud scenario flow
- Built the Unity client: stage manager, scoring, transitions and server integration
- Defined the JSON contract between Unity and the AI backend

## Demo

<!-- ✏️ Replace the placeholders with your real links -->
- 🎥 Video demo: _coming soon_
- 🖼️ Screenshots: _coming soon_

## Author

**Leena** · [github.com/leenasync](https://github.com/leenasync)
