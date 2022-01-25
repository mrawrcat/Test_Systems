using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flexible_Grid_Layout : LayoutGroup
{

    public enum FitType
    {
        Uniform,
        Width,
        Height,
        Fixed_Rows,
        Fixed_Columns
    }

    public FitType fit_Type;
    public int rows;
    public int columns;
    public Vector2 cell_Size;
    public Vector2 spacing;
    public bool Fit_X;
    public bool Fit_Y;
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if(fit_Type == FitType.Width || fit_Type == FitType.Height || fit_Type == FitType.Uniform)
        {
            Fit_X = true;
            Fit_Y = true;
            float sqr_Rt = Mathf.Sqrt(transform.childCount);
            rows = Mathf.CeilToInt(sqr_Rt);
            columns = Mathf.CeilToInt(sqr_Rt);
        }
        
    
        if(fit_Type == FitType.Width || fit_Type == FitType.Fixed_Columns)
        {
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if(fit_Type == FitType.Height || fit_Type == FitType.Fixed_Rows)
        {
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }

        float parent_Width = rectTransform.rect.width;
        float parent_Height = rectTransform.rect.height;

        float cell_Width = (parent_Width / (float)columns) - ((spacing.x / (float)columns * (columns -1))) - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cell_Height = (parent_Height / (float)rows) - ((spacing.y / (float)rows * (rows - 1))) - (padding.top / (float)rows) - (padding.bottom / (float)rows);

        if(rows <= 0)
        {
            rows = 1;
        }
        if (columns <= 0)
        {
            columns = 1;
        }

        cell_Size.x = Fit_X ? cell_Width : cell_Size.x;
        cell_Size.y = Fit_Y ? cell_Height : cell_Size.y;

        int row_Count = 0;
        int column_Count = 0;

        for(int i = 0; i < rectChildren.Count; i++)
        {
            row_Count = i / columns;
            column_Count = i % columns;

            var item = rectChildren[i];
            var x_pos = (cell_Size.x * column_Count) + (spacing.x * column_Count) + padding.left;
            var y_pos = (cell_Size.y * row_Count) + (spacing.y * row_Count) + padding.top;

            SetChildAlongAxis(item, 0, x_pos, cell_Size.x);
            SetChildAlongAxis(item, 1, y_pos, cell_Size.y);
        }

    }
    public override void CalculateLayoutInputVertical()
    {
        
    }

    public override void SetLayoutHorizontal()
    {
        
    }

    public override void SetLayoutVertical()
    {
        
    }

    
}
