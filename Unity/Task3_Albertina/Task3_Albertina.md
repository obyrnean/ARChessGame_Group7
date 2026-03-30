# Task 3 - Albertina

## Task 3.1: Headset / Phone Platform Research

### Recommended platform
The preferred platform is an AR/MR headset because it provides a more immersive experience and better supports natural hand interaction. Mobile AR should be kept as a fallback if headset setup or hardware access becomes difficult.

### Unity headset pipeline
1. Open the Unity project.
2. Switch the build target to Android.
3. Install OpenXR, XR Hands, and XR Interaction Toolkit.
4. Install Meta XR Core SDK.
5. Enable OpenXR in Project Settings.
6. Enable hand tracking features.
7. Enable passthrough / MR support.
8. Connect headset in developer mode.
9. Build and run a simple test scene.
10. Replace the test object with the chessboard and pieces.

### Phone fallback
If headset deployment is not possible in time, the same AR chess concept can still be demonstrated on a mobile phone using Unity AR Foundation.

## Task 3.2: Bot Opponent Research

### Recommended approach
The easiest approach is to use an existing chess engine such as Stockfish instead of building a bot from scratch.

### How it would work
1. The user moves a chess piece in Unity.
2. Unity updates the board state.
3. Unity sends the move to Stockfish.
4. Stockfish calculates the best response move.
5. Unity receives the move and updates the opponent piece.

### Complexity
This is technically feasible, but it adds complexity through move validation, board-state synchronisation, and communication with the engine.

### Fallback
If full bot integration is too complex, the project can instead use a guided tutorial mode or a scripted opponent.
