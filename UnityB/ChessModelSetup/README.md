# AR Chess Project - Ane O'Byrne

This repository contains the scripts, used models, and materials to visualize a chessboard and pieces in **Augmented Reality (AR)** using Unity.  

## Overview

The main focus of this project is the **`ChessSetup` script**, which:

- Arranges all chess pieces on the board correctly.
- Scales and rotates the pieces appropriately for both white and black sides.
- Ensures pieces are children of the **ChessModel**, so the whole set can be scaled or moved easily.

In AR mode, the chessboard aligns with an **Image Target** and adapts to its size while keeping the piece setup intact (similar to what we did for Lab4).

## Included Assets

- **Scripts:**
  - `ChessSetup.cs` – Places and configures chess pieces on the board.

- **3D Models:**
  - Chessboard model
  - Chess pieces (pawn, rook, knight, bishop, queen, king)

- **Materials:**
  - `WhitePiece.mat`
  - `BlackPiece.mat`
  (if it doesn't work simply create a new white and blac material)

## How to Use

1. Open the project in **Unity**.
2. Create an empty GameObject for the **ChessModel** (also to be the child of the AR Image Target) of size 0.02x0.02x0.02 and assign the `ChessSetup` script to it.
3. Assign the **chessBoardModel** (child mesh of the ChessModel) in the Inspector (do not add the chess pieces, just the board model).
4. Assign all piece prefabs and materials to the `ChessSetup` script in the Inspector.
5. Configure scaling and offsets as needed (the values here are specifically for the AR Image Target).
   - `boardInset` – Adjust if your board has extra borders (0.015).
   - `yOffset` – Lifts pieces above the board surface (0.0005).
   - Individual piece scales (0.4 best for all X Y Z).
6. Run the scene in **AR mode** to see the chessboard and pieces arranged and scaled correctly.


## Demonstration

A short video demonstrating the correct setup and AR placement is provided in the repository. Make sure all models and materials are in place to replicate the results. Can also be accessed at:
[Group 7 - Demo Videos](https://drive.google.com/drive/folders/17kt-myof2gih7YZU_Fex1HrL3-zN2w4d?usp=sharing)
