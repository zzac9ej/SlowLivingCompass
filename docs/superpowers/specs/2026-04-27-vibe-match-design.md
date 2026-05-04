# AI Vibe-Match Specification

## Overview
A core feature in the SlowLivingCompass client app to provide highly personalized, AI-driven sanctuary recommendations based on unstructured user mood/vibe descriptions.

## Approach
- **LLM as an Omniscient Guide (Option 2)**
- Unstructured free text input from the user (e.g. "Feeling burnt out from my boss, I need to see the ocean").
- Instead of just returning basic metadata tags, the LLM will receive a candidate list of nearby `Place` entries (max 15 places inside a 5km radius).
- The LLM acts as a sympathetic "Sanctuary Guide", analyzing the user's mood, and picking 3 places from the candidates.
- The LLM then generates a customized, soothing reason (`VibeReason`) explaining exactly why this spot is perfect for their current state of mind.

## Components
1. **LlmService.cs**: A new service wrapped around `HttpClient` to communicate with standard OpenAI-compatible endpoints (GroqCloud / NVIDIA NIM). Handles serializing the candidate places + user prompt, and parsing the JSON response.
2. **VibeMatchResult.cs**: The underlying POCO to robustly deserialize the LLM's response (`[ { "PlaceId": "...", "VibeReason": "...", "MatchScore": 95 } ]`).
3. **VibeMatch.razor**: New page. Contains a large, aesthetic text area for user input, a dynamic loading state ("AI 正在為您沉澱心情..."), and a glassmorphism gallery of customized result cards.

## Trade-offs & Rules
- **Token overhead**: Sending candiate places consumes more prompt tokens compared to simple keyword extraction. We mitigate this by restricting the list to nearest 15 places.
- **Resilience**: If the LLM throws an error or times out, the system must gracefully degrade and fall back to the standard random/nearby recommendation with canned responses.

## Success Criteria
- The LLM must output valid JSON.
- The recommended places must actually exist in the local DB.
- The UI must look premium, feeling like a dialogue rather than a database search.
