// using System;
// using System.Linq;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UIButton = UnityEngine.UI.Button;
// using System.Text;

// public class PuzzleManagerScript : MonoBehaviour
// {
//     // Serializable class to store grid cell data
//     [System.Serializable]
//     public class GridCell
//     {
//         public UIButton buttonObject;
//         public string spriteName;
//         public int x, y; // Add position properties
//         public bool visited = false;
//         public bool mustHit = false; // New property to indicate must-hit cells
//         public UIButton Button => buttonObject.GetComponent<UIButton>();
//     }

//     [System.Serializable]
//     public class PuzzleConfiguration
//     {
//         public GridCell[] gridCells;
//         public int startXOn, startYOn, startXOff, startYOff, totalOnGrids, totalOffGrids;
//         public List<GridCell> mustHitOnCells; // List of cells that must be turned on
//         public List<GridCell> mustHitOffCells; // List of cells that must be turned off
//     }

//     // Sprites for dice images and grid states
//     public Sprite dice1OffSprite;
//     public Sprite dice1OnSprite;
//     public Sprite dice2OffSprite;
//     public Sprite dice2OnSprite;
//     public Sprite dice3OffSprite;
//     public Sprite dice3OnSprite;
//     public Sprite dice4OffSprite;
//     public Sprite dice4OnSprite;
//     public Sprite dice5OffSprite;
//     public Sprite dice5OnSprite;
//     public Sprite dice6OffSprite;
//     public Sprite dice6OnSprite;
//     public Sprite offGridSprite;
//     public Sprite onGridSprite;
//     public Sprite Negative_1_Off;
//     public Sprite Negative_1_On;
//     public Sprite Negative_2_Off;
//     public Sprite Negative_2_On;
//     public Sprite Negative_3_Off;
//     public Sprite Negative_3_On;
//     public Sprite Negative_4_Off;
//     public Sprite Negative_4_On;
//     public Sprite Negative_5_Off;
//     public Sprite Negative_5_On;
//     public Sprite Negative_6_Off;
//     public Sprite Negative_6_On;

//     public PuzzleConfiguration[] puzzleConfigurations;
//     private int[] puzzleIndices = new int[3];
//     private int currentPuzzleIndex = 0;
//     private List<int> solvedPuzzles = new List<int>();
//     public UIButton Submit;
//     private Dictionary<string, Sprite> sprites;

//     private string playerSerialID = "678912"; // Assign the serial ID here for testing

//     void Start()
//     {
//         InitializeSprites();
//         InitializePuzzles();
//         AssignImagesToGrid();
//         Submit.onClick.AddListener(OnSubmit);
//     }

//     void InitializeSprites()
//     {
//         sprites = new Dictionary<string, Sprite>
//         {
//             { "Dice_1_Off", dice1OffSprite },
//             { "Dice_1_On", dice1OnSprite },
//             { "Dice_2_Off", dice2OffSprite },
//             { "Dice_2_On", dice2OnSprite },
//             { "Dice_3_Off", dice3OffSprite },
//             { "Dice_3_On", dice3OnSprite },
//             { "Dice_4_Off", dice4OffSprite },
//             { "Dice_4_On", dice4OnSprite },
//             { "Dice_5_Off", dice5OffSprite },
//             { "Dice_5_On", dice5OnSprite },
//             { "Dice_6_Off", dice6OffSprite },
//             { "Dice_6_On", dice6OnSprite },
//             { "OffGrid", offGridSprite },
//             { "OnGrid", onGridSprite },
//             { "Negative_1_Off", Negative_1_Off },
//             { "Negative_1_On", Negative_1_On },
//             { "Negative_2_Off", Negative_2_Off },
//             { "Negative_2_On", Negative_2_On },
//             { "Negative_3_Off", Negative_3_Off },
//             { "Negative_3_On", Negative_3_On },
//             { "Negative_4_Off", Negative_4_Off },
//             { "Negative_4_On", Negative_4_On },
//             { "Negative_5_Off", Negative_5_Off },
//             { "Negative_5_On", Negative_5_On },
//             { "Negative_6_Off", Negative_6_Off },
//             { "Negative_6_On", Negative_6_On }
//         };
//     }

//     void InitializePuzzles()
//     {
//         // Get the first three characters of the serial ID
//         string serialIDPart = playerSerialID.Substring(0, 3);

//         // Hash each character to determine the puzzle index
//         for (int i = 0; i < 3; i++)
//         {
//             puzzleIndices[i] = HashSerialID(serialIDPart[i].ToString());
//         }

//         currentPuzzleIndex = 0;
//     }

//     int HashSerialID(string serialIDPart)
//     {
//         byte[] asciiBytes = Encoding.ASCII.GetBytes(serialIDPart);
//         int sum = asciiBytes.Sum(b => b);
//         int hashValue = sum % puzzleConfigurations.Length;
//         return hashValue;
//     }

//     void AssignImagesToGrid()
//     {
//         PuzzleConfiguration currentConfiguration = puzzleConfigurations[puzzleIndices[currentPuzzleIndex]];

//         foreach (var gridCell in currentConfiguration.gridCells)
//         {
//             Image imgComponent = gridCell.buttonObject.GetComponent<Image>();
//             UIButton buttonComponent = gridCell.Button;

//             if (imgComponent != null && sprites.ContainsKey(gridCell.spriteName))
//             {
//                 imgComponent.sprite = sprites[gridCell.spriteName];
//             }

//             if (buttonComponent != null)
//             {
//                 buttonComponent.onClick.RemoveAllListeners();
//                 buttonComponent.onClick.AddListener(() => OnTileClick(gridCell.buttonObject.gameObject));
//             }
//         }
//     }

//     void OnTileClick(GameObject tile)
//     {
//         Image imgComponent = tile.GetComponent<Image>();
//         if (imgComponent != null)
//         {
//             string currentSpriteName = imgComponent.sprite.name;
//             string newSpriteName = "";

//             // Check if the current sprite is one of the dice sprites
//             if (currentSpriteName.StartsWith("Dice"))
//             {
//                 return; // Do nothing
//             }

//             if (currentSpriteName.Contains("_Off"))
//             {
//                 newSpriteName = currentSpriteName.Replace("_Off", "_On");
//                 imgComponent.sprite = sprites[newSpriteName];
//             }
//             else if (currentSpriteName.Contains("_On"))
//             {
//                 newSpriteName = currentSpriteName.Replace("_On", "_Off");
//                 imgComponent.sprite = sprites[newSpriteName];
//             }
//             else if (currentSpriteName == "OffGrid")
//             {
//                 newSpriteName = "OnGrid";
//                 imgComponent.sprite = sprites[newSpriteName];
//             }
//             else if (currentSpriteName == "OnGrid")
//             {
//                 newSpriteName = "OffGrid";
//                 imgComponent.sprite = sprites[newSpriteName];
//             }

//             // Update the spriteName 
//             PuzzleConfiguration currentConfiguration = puzzleConfigurations[puzzleIndices[currentPuzzleIndex]];
//             foreach (var gridCell in currentConfiguration.gridCells)
//             {
//                 if (gridCell.buttonObject.gameObject == tile)
//                 {
//                     gridCell.spriteName = newSpriteName;
//                     break; // Exit the loop once the matching Cell is found 
//                 }
//             }
//         }
//     }

//     public void OnSubmit()
//     {
//         PuzzleConfiguration currentConfiguration = puzzleConfigurations[puzzleIndices[currentPuzzleIndex]];

//         // Reset visited state 
//         foreach (var gridCell in currentConfiguration.gridCells)
//         {
//             gridCell.visited = false;
//         }

//         // Find the starting positions
//         int startXOn = currentConfiguration.startXOn;
//         int startYOn = currentConfiguration.startYOn;
//         int startXOff = currentConfiguration.startXOff;
//         int startYOff = currentConfiguration.startYOff;
//         int onGridCount = 0;
//         int offGridCount = 0;
//         if (startXOn != -1 && startYOn != -1)
//         {
//             onGridCount = DFSOnGrids(startXOn, startYOn);
//             Debug.Log("Starting coordinates for On Grids: " + startXOn + ", " + startYOn);
//             Debug.Log($"Total On Grids: {onGridCount}");
//         }

//         if (startXOff != -1 && startYOff != -1)
//         {
//             offGridCount = DFSOffGrids(startXOff, startYOff);
//             Debug.Log("Starting coordinates for Off Grids: " + startXOff + ", " + startYOff);
//             Debug.Log($"Total Off Grids: {offGridCount}");
//         }

//         if(currentConfiguration.totalOnGrids == onGridCount && currentConfiguration.totalOffGrids == offGridCount
//             && AreMustHitCellsCorrect(currentConfiguration))
//         {
//             Debug.Log("Puzzle Solved!");

//             solvedPuzzles.Add(puzzleIndices[currentPuzzleIndex]);

//             // Check if there are more puzzles to solve
//             if (currentPuzzleIndex < 2)
//             {
//                 do
//                 {
//                     currentPuzzleIndex++;
//                 } while (solvedPuzzles.Contains(puzzleIndices[currentPuzzleIndex]) && currentPuzzleIndex < puzzleIndices.Length - 1);

//                 AssignImagesToGrid();
//             }
//             else
//             {
//                 Debug.Log("All puzzles solved!");
//             }
//         }
//         else
//         {
//             Debug.Log("Puzzle Not Solved!");
//         }
//     }

//     bool AreMustHitCellsCorrect(PuzzleConfiguration currentConfiguration)
//     {
//         // Check must-hit on cells
//         foreach (var cell in currentConfiguration.mustHitOnCells)
//         {
//             if (!cell.spriteName.Contains("On"))
//             {
//                 return false;
//             }
//         }

//         // Check must-hit off cells
//         foreach (var cell in currentConfiguration.mustHitOffCells)
//         {
//             if (!cell.spriteName.Contains("Off"))
//             {
//                 return false;
//             }
//         }

//         return true;
//     }

//     private int DFSOnGrids(int x, int y)
//     {
//         if (x < 0 || x >= 3 || y < 0 || y >= 3) return 0; // Assuming a 3x3 grid for simplicity
//         PuzzleConfiguration currentConfiguration = puzzleConfigurations[puzzleIndices[currentPuzzleIndex]];
//         GridCell cell = currentConfiguration.gridCells.FirstOrDefault(c => c.x == x && c.y == y);
//         if (cell == null || !cell.spriteName.Contains("On") || cell.visited) return 0;

//         cell.visited = true; // Mark as visited
//         return 1 + DFSOnGrids(x + 1, y) + DFSOnGrids(x - 1, y) + DFSOnGrids(x, y + 1) + DFSOnGrids(x, y - 1);
//     }

//     private int DFSOffGrids(int x, int y)
//     {
//         if (x < 0 || x >= 3 || y < 0 || y >= 3) return 0; // Assuming a 3x3 grid for simplicity
//         PuzzleConfiguration currentConfiguration = puzzleConfigurations[puzzleIndices[currentPuzzleIndex]];
//         GridCell cell = currentConfiguration.gridCells.FirstOrDefault(c => c.x == x && c.y == y);
//         if (cell == null || !cell.spriteName.Contains("Off") || cell.visited) return 0;

//         cell.visited = true; // Mark as visited
//         return 1 + DFSOffGrids(x + 1, y) + DFSOffGrids(x - 1, y) + DFSOffGrids(x, y + 1) + DFSOffGrids(x, y - 1);
//     }
// }
