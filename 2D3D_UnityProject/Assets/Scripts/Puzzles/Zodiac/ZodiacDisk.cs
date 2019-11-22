using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodiacDisk : ZodiacPuzzlePiece
{
    /// <summary>
    /// How fast the disk should rotate
    /// </summary>
    [SerializeField]
    [Range(1,2)]
    private float rotationSpeed = 1.5f;

    /// <summary>
    /// List (ordered containing references to symbols on the disk
    /// </summary>
    public List<Sprite> symbols;

    /// <summary>
    /// Correct symbols for this ring
    /// </summary>
    [SerializeField]
    private List<Sprite> correctSymbols;

    [SerializeField]
    private SpriteRenderer spritePrefab;

    // TODO: do this programmatically?
    /// <summary>
    /// Hand-placed game object at top of disk, helps us indicate sprite placement (imagine it rotated around the disk)
    /// </summary>
    [SerializeField]
    private GameObject spritePivot;

    /// <summary>
    /// Index of current symbol
    /// </summary>
    private int selectedSymbolIndex;

    /// <summary>
    /// Holds angle (in degrees) between each symbol on the disk
    /// </summary>
    private float angleBetweenSymbols;

    /// <summary>
    /// Reference to zodiac puzzle
    /// </summary>
    private ZodiacPuzzle puzzle;

    /// <summary>
    /// Event fired when player rotates the disk to a selected symbol
    /// </summary>
    public SelectedSymbol selectedSymbol;
    public delegate void SelectedSymbol();

    private void Start()
    {
        // Print error if no correct symbol assigned for this disk
        if(correctSymbols == null) {
            Debug.LogErrorFormat("Zodiac Puzzle - {0}: please set the correct symbol for this disk", name);
        }
    }

    public void Init(ZodiacPuzzle puzzle)
    {
        // Break if no symbols found
        if (symbols.Count == 0) {
            Debug.LogErrorFormat("{0}: no symbols found", name);
            return;
        }

        // Break if incorrect number of correct symbols
        if (correctSymbols.Count != ZodiacPuzzle.numberOfRounds)
        {
            Debug.LogErrorFormat("{0}: incorrect number of elements in correct symbols list", name);
            return;
        }

        this.puzzle = puzzle;

        // Calculate angle between symbols
        angleBetweenSymbols = 360f / symbols.Count;

        // Generate each symbol around the ring
        PopulateSymbols();
    }

    private void PopulateSymbols()
    {
        // Instantiate each symbol around the ring
        for(int i = 0; i < symbols.Count; i++) {
            Sprite symbol = symbols[i];

            // Calculate sprite rotation
            Quaternion rotation = Quaternion.Euler(90, 0, i * angleBetweenSymbols);

            // Instantiate symbol at pivot point
            SpriteRenderer instance = Instantiate(spritePrefab, transform, false);
            instance.sprite = symbol;
            instance.name = symbol.name;
            instance.transform.localRotation = rotation;
            instance.transform.localPosition = spritePivot.transform.localPosition;

            // Rotate pivot around ring to position of next symbol
            spritePivot.transform.RotateAround(transform.position, transform.up, angleBetweenSymbols);
        }

        selectedSymbolIndex = 0;
    }

    /// <summary>
    /// Rotates disk left/right towards the next symbol along the ring
    /// </summary>
    /// <param name="direction"></param>
    public void Rotate(ZodiacPuzzle.Direction direction)
    {
        if(rotateCoroutineInstance == null) {
            rotateCoroutineInstance = RotateCoroutine(direction);
            StartCoroutine(rotateCoroutineInstance);
        }
    }

    private IEnumerator rotateCoroutineInstance;
    private IEnumerator RotateCoroutine(ZodiacPuzzle.Direction direction)
    {
        // Calculate degrees of rotation (in given distance)
        float change = (angleBetweenSymbols * (int)direction);

        // Create quaternion containing change in rotation (relative to current reference frame)
        Quaternion rotationChange = Quaternion.Euler(new Vector3(0, change));

        // Rotate towards that quaternion
        Quaternion targetRotation = transform.rotation * rotationChange;

        // Track starting rotation to maintain lerp lower-bound
        Quaternion startRotation = transform.rotation; 

        // Increment time each frame (scaled by rotationSpeed)
        for(float t=0; t<1; t += rotationSpeed * Time.fixedDeltaTime) {
            // Update rotation
            Quaternion rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            transform.rotation = rotation;

            yield return null;
        }

        // Ensure we land exactly on target rotation
        transform.rotation = targetRotation;

        // Check which symbol we landed on
        UpdateSelectedSymbol(direction);

        rotateCoroutineInstance = null;
        yield return null;
    }

    /// <summary>
    /// Tracks symbol selected after a disk rotation.
    /// Sends a message to ZodiacPuzzle if correct symbol selected
    /// </summary>
    /// <param name="direction">Rotation direction</param>
    private void UpdateSelectedSymbol(ZodiacPuzzle.Direction direction)
    {
        // Update selected symbol index
        if (direction.Equals(ZodiacPuzzle.Direction.CLOCKWISE)) {
            // Wrap around to first symbol
            if (selectedSymbolIndex >= symbols.Count - 1) {
                selectedSymbolIndex = 0;
            }
            // or increment normally
            else {
                selectedSymbolIndex++;
            }
        }
        else if (direction.Equals(ZodiacPuzzle.Direction.COUNTER_CLOCKWISE)) {
            // Wrap around to last symbol
            if (selectedSymbolIndex == 0) {
                selectedSymbolIndex = symbols.Count-1;
            }
            // or decrement normally
            else {
                selectedSymbolIndex--;
            }
        }

        // Fire symbol selected event
        selectedSymbol?.Invoke();
    }

    /// <summary>
    /// Returns true if disk rotated to correct symbol, false otherwise
    /// </summary>
    /// <returns></returns>
    public bool Correct()
    {
        return symbols[selectedSymbolIndex].Equals(correctSymbols[puzzle.currentRound - 1]);
    }
}
