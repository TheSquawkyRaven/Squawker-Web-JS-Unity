using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MatrixEnvironment : Environment
{

    [System.Serializable]
    public class MatrixContainer : EnvironmentContainer
    {
        public SColor mainColor = null;
        public SColor headColor = null;
        public bool isRandomColor = false;
        public float randomSpeed = -1;
        public float matrixFlowLinePulse = -1;
        public float newTrailPulse = -1;
        public int lineTrailMaxLength = -1;
        public int lineTrailMinLength = -1;
    }

    public MatrixContainer Save => (MatrixContainer)container;

    public enum Usage
    {
        RowsColumns,
        CellSize
    }

    public RectTransform thisTransform;

    public Usage usage;
    public Vector2Int rowsColumns;
    public Vector2 cellSize;
    public GameObject prefab;

    public List<Dictionary<RectTransform, TextMeshProUGUI>> table = new List<Dictionary<RectTransform, TextMeshProUGUI>>();

    #region Particles
    public ParticleSystem matrixParticles;
    #endregion

    #region Runtime Variables
    [HideInInspector] public bool active;
    //[HideInInspector] public int lastPointingRow = 0;
    [HideInInspector] public int lastPointingColumn = 0;
    [HideInInspector] public Coroutine randomColorCoroutine;
    #endregion

    #region Settings
    public bool setAlpha;
    public Color mainColor;
    public Color color1;
    public Color color2;
    [Range(0f, 1f)]
    public float colorLowerRange;
    [Range(0f, 1f)]
    public float colorUpperRange;
    public float matrixFlowLineMinPulse;
    public float matrixFlowLineMaxPulse;
    public float matrixFlowLineInternalRangePulse;
    public Color matrixFlowHeadColor;
    public int lineTrailMinLength;
    public int lineTrailMaxLength;
    public float newTrailMinPulse;
    public float newTrailMaxPulse;
    public int lineSelectTolerance;
    #endregion

    protected override void AddCommands()
    {
        base.AddCommands();

        mainCommand.Add("clear", Clear);
        mainCommand.Add("clr", Clear);

        mainCommand.Add("maincolor", MainColor);
        mainCommand.Add("colormain", MainColor);
        mainCommand.Add("color", MainColor);

        mainCommand.Add("headcolor", HeadColor);
        mainCommand.Add("colorhead", HeadColor);
        mainCommand.Add("head", HeadColor);

        mainCommand.Add("randomcolors", RandomColors);
        mainCommand.Add("colorsrandom", RandomColors);
        mainCommand.Add("randcolors", RandomColors);
        mainCommand.Add("colorsrand", RandomColors);

        mainCommand.Add("stoprandom", StopRandom);
        mainCommand.Add("randomstop", StopRandom);
        mainCommand.Add("stoprand", StopRandom);
        mainCommand.Add("randstop", StopRandom);

        mainCommand.Add("flowspeed", FlowSpeed);
        mainCommand.Add("speed", FlowSpeed);
        mainCommand.Add("flow", FlowSpeed);

        mainCommand.Add("frequency", Frequency);
        mainCommand.Add("freq", Frequency);

        mainCommand.Add("trail", TrailLength);
        mainCommand.Add("traillength", TrailLength);

        mainCommand.Add("trailmin", TrailLengthMin);
        mainCommand.Add("traillengthmin", TrailLengthMin);

        mainCommand.Add("trailmax", TrailLengthMax);
        mainCommand.Add("traillengthmax", TrailLengthMax);
    }

    private void CheckTable()
    {
        if (table.Count == 0)
        {
            RefreshTableVariable();
            if (table.Count == 0)
            {
                GenerateTable();
            }
            else
            {
                if (table[0].Count == 0)
                {
                    GenerateTable();
                }
            }
        }
    }

    #region Commands
    protected void Clear(FileReader.Content content)
    {
        ClearAllText();
        lastPointingColumn = 0;
        SN.@this.popups.PutPopup($"Matrix cleared", SN.infoPopup);
        //lastPointingRow = table[0].Count - 1;
    }
    protected void MainColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            SetColor(color, false);
            Save.mainColor = color;
            SN.@this.popups.PutPopup($"Main color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "maincolor", "green");
    }
    protected void HeadColor(FileReader.Content content)
    {
        ColorHelper(content, (color) =>
        {
            matrixFlowHeadColor = color;
            Save.headColor = color;
            SN.@this.popups.PutPopup($"Head color set to {ShowColor(color)}", SN.BrushPopup(color));
            return;
        }, "headcolor", "lime");
    }
    protected void RandomColors(FileReader.Content content)
    {
        if (content.commands.Length >= 2)
        {
            bool isFloat = float.TryParse(content.commands[1], out float value);
            if (isFloat && value > 0)
            {
                Save.randomSpeed = Mathf.Max(value, 0.001f);

                SN.@this.popups.PutPopup($"Colors set to randomly change every {value} seconds", SN.optionsPopup);
                return;
            }
        }
        if (randomColorCoroutine != null)
        {
            StopCoroutine(randomColorCoroutine);
        }
        randomColorCoroutine = StartCoroutine(SetRandomColors(Save.randomSpeed));
        Save.isRandomColor = true;

        SN.@this.popups.PutPopup($"Colors set to randomly change every {Save.randomSpeed} seconds", SN.optionsPopup);
    }
    protected void StopRandom(FileReader.Content content)
    {
        if (randomColorCoroutine != null)
        {
            StopCoroutine(randomColorCoroutine);
            Save.isRandomColor = false;
        }
        SN.@this.popups.PutPopup($"Randomizing colors stopped", SN.optionsPopup);
    }

    private IEnumerator SetRandomColors(float pulse)
    {
        while (true)
        {
            SetColor(Random.ColorHSV(), false);
            yield return new WaitForSeconds(pulse);
        }
    }
    protected void FlowSpeed(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            value = Mathf.Max(value, 0.01f);
            matrixFlowLineMinPulse = matrixFlowLineMaxPulse = value;
            Save.matrixFlowLinePulse = value;
            SN.@this.popups.PutPopup($"Speed set to {value}", SN.optionsPopup);
            return;
        }, "speed", 0.05f);
    }
    protected void Frequency(FileReader.Content content)
    {
        FloatHelper(content, (value) =>
        {
            value = Mathf.Max(value, 0.01f);
            newTrailMinPulse = newTrailMaxPulse = value;
            Save.newTrailPulse = value;
            SN.@this.popups.PutPopup($"Frequency set to {value}", SN.optionsPopup);
            return;
        }, "frequency", 0.04f);
    }
    protected void TrailLength(FileReader.Content content)
    {
        IntHelper(content, (value) =>
        {
            value = Mathf.Max(1, value);
            lineTrailMinLength = lineTrailMaxLength = value;
            Save.lineTrailMinLength = Save.lineTrailMaxLength = value;
            SN.@this.popups.PutPopup($"Trail length set to {value}", SN.optionsPopup);
            return;
        }, "traillength", 8);
    }
    protected void TrailLengthMin(FileReader.Content content)
    {
        IntHelper(content, (value) =>
        {
            value = Mathf.Max(1, value);
            lineTrailMinLength = value;
            Save.lineTrailMinLength = value;
            SN.@this.popups.PutPopup($"Trail length minimum capped at {value}", SN.optionsPopup);
            return;
        }, "traillengthmin", 10);
    }
    protected void TrailLengthMax(FileReader.Content content)
    {
        IntHelper(content, (value) =>
        {
            value = Mathf.Max(1, value);
            lineTrailMaxLength = value;
            Save.lineTrailMaxLength = value;
            SN.@this.popups.PutPopup($"Trail length maximum capped at {value}", SN.optionsPopup);
            return;
        }, "traillengthmax", 16);
    }
    #endregion

    #region Setup

    [ContextMenu("Refresh Table Variable")]
    public void RefreshTableVariable()
    {
        table.Clear();
        for (int i = 0; i < rowsColumns.x; i++)
        {
            table.Add(new Dictionary<RectTransform, TextMeshProUGUI>());
            for (int j = 0; j < rowsColumns.y; j++)
            {
                string name = $"[{i}][{j}]";
                Transform child = thisTransform.Find(name);
                if (child != null)
                {
                    table[i].Add((RectTransform)child, child.GetComponent<TextMeshProUGUI>());
                }
                else
                {
                    break;
                }
            }
        }
    }

    [ContextMenu("Generate Table")]
    public void GenerateTable()
    {
        if (usage == Usage.CellSize)
        {
            rowsColumns.x = Mathf.FloorToInt(thisTransform.rect.size.x / cellSize.x);
            rowsColumns.y = Mathf.FloorToInt(thisTransform.rect.size.y / cellSize.y);
            Debug.Log(thisTransform.rect.size);
        }
        else
        {
            cellSize.x = thisTransform.rect.size.x / rowsColumns.x;
            cellSize.y = thisTransform.rect.size.y / rowsColumns.y;
        }
        table.Clear();
        for (int i = 0; i < rowsColumns.x; i++)
        {
            table.Add(new Dictionary<RectTransform, TextMeshProUGUI>());
            for (int j = 0; j < rowsColumns.y; j++)
            {
                Vector2 position = new Vector2(thisTransform.rect.size.x / rowsColumns.x * i, thisTransform.rect.size.y / rowsColumns.y * j);
                GameObject gameObject = Instantiate(prefab, thisTransform);
                gameObject.name = $"[{i}][{j}]";
                RectTransform transform = gameObject.GetComponent<RectTransform>();
                transform.sizeDelta = cellSize;
                transform.anchoredPosition = position;
                table[i].Add(transform, gameObject.GetComponent<TextMeshProUGUI>());
            }
        }
    }

    [ContextMenu("Destroy Table")]
    public void DestroyTable()
    {
        for (int i = 0; i < table.Count; i++)
        {
            table[i].Clear();
        }
        table.Clear();
        bool loopRan = true;
        while (loopRan)
        {
            loopRan = false;
            foreach (Transform child in thisTransform)
            {
                loopRan = true;
                DestroyImmediate(child.gameObject);
            }
        }
    }

    [ContextMenu("Set All Text 'A'")]
    public void SetAllTextA()
    {
        CheckTable();
        for (int i = 0; i < table.Count; i++)
        {
            foreach (KeyValuePair<RectTransform, TextMeshProUGUI> cell in table[i])
            {
                cell.Value.SetText("A");
            }
        }
    }
    [ContextMenu("Clear All Text")]
    public void ClearAllText()
    {
        CheckTable();
        for (int i = 0; i < table.Count; i++)
        {
            foreach (KeyValuePair<RectTransform, TextMeshProUGUI> cell in table[i])
            {
                if (cell.Value.text != string.Empty)
                {
                    cell.Value.SetText(string.Empty);
                }
            }
        }
    }
    #endregion

    #region Color
    private Color GetColor()
    {
        return Color.Lerp(color1, color2, Random.value);
    }

    [ContextMenu("Set All Text Invisible")]
    public void SetAllTextInvisible()
    {
        CheckTable();
        for (int i = 0; i < table.Count; i++)
        {
            foreach (KeyValuePair<RectTransform, TextMeshProUGUI> cell in table[i])
            {
                cell.Value.color = new Color(0, 0, 0, 0);
            }
        }
    }
    [ContextMenu("Set Color")]
    public void SetColor()
    {
        SetColor(mainColor, setAlpha);
        SetColor(true);
    }
    public void SetColor(bool hasSetColor)
    {
        if (!hasSetColor)
        {
            SetColor(mainColor, setAlpha);
        }
        CheckTable();
        for (int i = 0; i < table.Count; i++)
        {
            foreach (KeyValuePair<RectTransform, TextMeshProUGUI> cell in table[i])
            {
                cell.Value.color = Color.Lerp(color1, color2, Random.value);
            }
        }
    }
    public void SetColor(Color color, bool setAlpha)
    {
        this.setAlpha = setAlpha;
        color1 = new Color(color.r - colorLowerRange, color.g - colorLowerRange, color.b - colorLowerRange, setAlpha ? color.a - colorLowerRange : 1f);
        color2 = new Color(color.r + colorUpperRange, color.g + colorUpperRange, color.b + colorUpperRange, setAlpha ? color.a + colorUpperRange : 1f);
        mainColor = color;
        if (matrixParticles != null)
        {
            ParticleSystem.MainModule mainSystem = matrixParticles.main;
            mainSystem.startColor = new ParticleSystem.MinMaxGradient(color1, color2);
        }
        //SetColor(true);
    }

    private void SetCellColor(int column, int row, Color color)
    {
        if (column >= 0 && column < table.Count && row >= 0 && row < table[0].Count)
        {
            table[column].ElementAt(row).Value.color = color;
        }
    }
    #endregion

    protected override void ManageSave()
    {
        base.ManageSave();
        if (Save != null)
        {
            if (Save.backgroundColor != null)
            {
                backgroundImage.color = Save.backgroundColor;
            }
            if (Save.mainColor != null)
            {
                SetColor(Save.mainColor, false);
            }
            if (Save.headColor != null)
            {
                matrixFlowHeadColor = Save.headColor;
            }
            if (Save.isRandomColor)
            {
                if (randomColorCoroutine != null)
                {
                    StopCoroutine(randomColorCoroutine);
                }
                randomColorCoroutine = StartCoroutine(SetRandomColors(Save.randomSpeed > 0 ? Save.randomSpeed : 0.01f));
            }
            if (Save.matrixFlowLinePulse > 0)
            {
                matrixFlowLineMinPulse = matrixFlowLineMaxPulse = Save.matrixFlowLinePulse;
            }
            if (Save.newTrailPulse > 0)
            {
                newTrailMinPulse = newTrailMaxPulse = Save.newTrailPulse;
            }
            if (Save.lineTrailMinLength > 0)
            {
                lineTrailMinLength = Save.lineTrailMinLength;
            }
            if (Save.lineTrailMaxLength > 0)
            {
                lineTrailMaxLength = Save.lineTrailMaxLength;
            }
        }
    }

    public override void StartEnvironment()
    {
        environmentName = "matrix";
        base.StartEnvironment();
        ManageSave(SN.@this.save.matrixContainer);
        DestroyTable();
        GenerateTable();
        SetAllTextInvisible();
        ClearAllText();
        CheckTable();
        //if (table.Count > 0 && table[0].Count > 0)
        //{
        //    lastPointingRow = table[0].Count - 1;
        //}
        lastPointingColumn = 0;
        SN.@this.FILEReader.onContentChange += Write;
        active = true;
        StartFlowing();
    }

    public override void StopEnvironment()
    {
        base.StopEnvironment();
        SN.@this.FILEReader.onContentChange -= Write;
        active = false;
    }

    #region Matrix Flow
    public class MatrixFlowLine
    {
        public bool completed = false;
        public Color randomizedColor;
        public int column;
        public int headRow;
        public int tailRow;
    }
    public List<MatrixFlowLine> randomUpdateLines = new List<MatrixFlowLine>();
    public List<MatrixFlowLine> constantUpdateLines = new List<MatrixFlowLine>();
    private void StartFlowing()
    {
        StartCoroutine(CreateFlowLine());
    }
    private IEnumerator CreateFlowLine()
    {
        List<int> availableIndexes = new List<int>();
        while (active)
        {
            yield return new WaitForSeconds(Random.Range(newTrailMinPulse, newTrailMaxPulse));
            if (!active)
            {
                break;
            }
            availableIndexes.Clear();
            for (int i = 0; i < table.Count; i++)
            {
                availableIndexes.Add(i);
            }
            for (int i = 0; i < randomUpdateLines.Count; i++)
            {
                if (randomUpdateLines[i].tailRow > (table[0].Count - lineSelectTolerance))
                {
                    availableIndexes.Remove(randomUpdateLines[i].column);
                }
            }
            if (availableIndexes.Count > 0)
            {
                int selectedColumn = availableIndexes[Random.Range(0, availableIndexes.Count)];
                SetFlowLine(selectedColumn, Random.Range(lineTrailMinLength, lineTrailMaxLength), true);
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="trailLength">Excludes head</param>
    private void SetFlowLine(int column, int trailLength, bool selfUpdate)
    {
        if (table.Count > 0 && table[0].Count > 0)
        {
            if (trailLength < 0)
            {
                trailLength = 0;
            }
            MatrixFlowLine line = new MatrixFlowLine()
            {
                randomizedColor = GetColor(),
                column = column,
                headRow = table[0].Count - 1,
                tailRow = table[0].Count - 1 + trailLength
            };
            if (selfUpdate)
            {
                randomUpdateLines.Add(line);
                StartCoroutine(UpdateLine(line, Random.Range(matrixFlowLineMinPulse, matrixFlowLineMaxPulse)));
            }
            else
            {
                constantUpdateLines.Add(line);
            }
        }

    }
    private IEnumerator UpdateLine(MatrixFlowLine line, float pulse)
    {
        while (!line.completed && active)
        {
            UpdateFlowLine(line);

            yield return new WaitForSeconds(Random.Range(pulse - matrixFlowLineInternalRangePulse, pulse + matrixFlowLineInternalRangePulse));
        }
        randomUpdateLines.Remove(line);
    }
    private void UpdateFlowLine(MatrixFlowLine line)
    {
        SetCellColor(line.column, line.headRow, matrixFlowHeadColor);
        for (int i = line.headRow + 1; i <= line.tailRow; i++)
        {
            if (i >= 0 && i < table[0].Count)
            {
                Color color = line.randomizedColor;
                color.a = (float)(line.tailRow - i) / (line.tailRow - line.headRow);
                //Debug.Log($"({line.tailRow} - {i} = {line.tailRow - i}) / \n({line.tailRow} - {line.headRow} = {line.tailRow - line.headRow}) = {color.a}");
                SetCellColor(line.column, i, color);
            }
        }
        line.tailRow--;
        line.headRow--;
        if (line.tailRow < 0)
        {
            line.completed = true;
        }
    }
    #endregion

    #region Write

    private void Write(FileReader.Content content)
    {
        if (!content.isCommand)
        {
            FullWriteText(content.displayName);
            FullWriteText(content.text);
        }
    }

    /// <summary>
    /// Full Write
    /// </summary>
    private void FullWriteText(string text)
    {
        if (table.Count > 0 && table[0].Count > 0)
        {
            text = text.Replace(" ", "").Replace("\n", "");
            int targetColumn = lastPointingColumn;
            if (targetColumn >= table.Count)
            {
                targetColumn = 0;
            }
            for (int i = table[0].Count - 1; i >= 0; i--)
            {
                if ((table[0].Count - 1 - i) < text.Length)
                {
                    table[targetColumn].ElementAt(i).Value.SetText(text[table[0].Count - 1 - i].ToString());
                }
                else
                {
                    table[targetColumn].ElementAt(i).Value.SetText(string.Empty);
                }
            }
            //for (int i = 0; i < text.Length; i++)
            //{
            //    if (targetRow == -1)
            //    {
            //        //targetColumn++;
            //        break;
            //        //targetRow = table[0].Count - 1;
            //    }
            //    table[targetColumn].ElementAt(targetRow).Value.SetText(text[i].ToString());
            //    targetRow--;
            //    //Debug.Log($"Written {text[i]} at {targetColumn},{targetRow}");
            //}
            targetColumn++;
            lastPointingColumn = targetColumn;
        }
    }


    #endregion

}
