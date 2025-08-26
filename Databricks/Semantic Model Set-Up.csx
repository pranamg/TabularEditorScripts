/*
 * Title: Semantic Model Set-Up
 * Author: Johnny Winter, greyskullanalytics.com
 *
 * This script, when executed, will loop through all tables and columns in the model and rename with friendly names. Names in snake_case, camelCase or PascalCase
 * will all be converted to Proper Case.
 *
 * Whilst looping though columns it also sets default summarization to none and sets a format string for all DateTime type fields 
 * (currently it sets format 'yyyy-mm-dd' but you can change this on line 61 if you wish).
 *
 */
using System;
using System.Globalization;

//create script as class so it can be reused 
class p {

    public static void ConvertCase(dynamic obj)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        //replace underscores with a space
        var oldName = obj.Name.Replace("_", " ");
        var newName = new System.Text.StringBuilder();
        for(int i = 0; i < oldName.Length; i++) {
            // First letter should always be capitalized:
            if(i == 0) newName.Append(Char.ToUpper(oldName[i]));

            // A sequence of two uppercase letters followed by a lowercase letter should have a space inserted
            // after the first letter:
            else if(i + 2 < oldName.Length && char.IsLower(oldName[i + 2]) && char.IsUpper(oldName[i + 1]) && char.IsUpper(oldName[i]))
            {
                newName.Append(oldName[i]);
                newName.Append(" ");
            }

            // All other sequences of a lowercase letter followed by an uppercase letter, should have a space
            // inserted after the first letter:
            else if(i + 1 < oldName.Length && char.IsLower(oldName[i]) && char.IsUpper(oldName[i+1]))
            {
                newName.Append(oldName[i]);
                newName.Append(" ");
            }
            else
            {
                newName.Append(oldName[i]);
            }
        }
        //apply Proper Case where this has not already been taken car of above
        obj.Name = textInfo.ToTitleCase(newName.ToString());
    }
}

foreach(var t in Model.Tables) {
//convert table names
    p.ConvertCase(t);
//convert column names
    foreach(var c in t.Columns) {
        p.ConvertCase(c);
        c.SummarizeBy = AggregateFunction.None;
        if (c.DataType == DataType.DateTime)
        {c.FormatString = "yyyy-mm-dd";}
    }
}