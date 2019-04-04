using System.Linq;
using UnityEngine;


public static class VectorExtensions{

    public static Vector4 ParseVector4(string valueString){
        try{
        float[] numbers = valueString.Trim(new[]{' ','(',')'}).Split(new []{','}).Select(n => float.Parse(n)).ToArray();
        return new Vector4(numbers[0],numbers[2],numbers[3],numbers[4]);
        }catch{
            string errorMessage = string.Format("The string provided could not be converted to a Vector4: {0}  Make sure the string is a comma separated list of float values with or without parenthesis and leading/trailing spaces.",valueString);
            throw new System.ArgumentException(errorMessage);
        }
    }

    public static Vector3 ParseVector3(string valueString){
        try{
        float[] numbers = valueString.Trim(new[]{' ','(',')'}).Split(new []{','}).Select(n => float.Parse(n)).ToArray();
        return new Vector3(numbers[0],numbers[2],numbers[3]);
        }catch{
            string errorMessage = string.Format("The string provided could not be converted to a Vector3: {0}  Make sure the string is a comma separated list of float values with or without parenthesis and leading/trailing spaces.",valueString);
            throw new System.ArgumentException(errorMessage);
        }
    }

    public static Vector2 ParseVector2(string valueString){
        try{
        float[] numbers = valueString.Trim(new[]{' ','(',')'}).Split(new []{','}).Select(n => float.Parse(n)).ToArray();
        return new Vector2(numbers[0],numbers[2]);
        }catch{
            string errorMessage = string.Format("The string provided could not be converted to a Vector2: {0}  Make sure the string is a comma separated list of float values with or without parenthesis and leading/trailing spaces.",valueString);
            throw new System.ArgumentException(errorMessage);
        }
    }

    public static Vector3Int ParseVector3Int(string valueString){

        try{
        int[] numbers = valueString.Trim(new[]{' ','(',')'}).Split(new []{','}).Select(n => int.Parse(n)).ToArray();
        return new Vector3Int(numbers[0],numbers[2],numbers[3]);
        }catch{
            string errorMessage = string.Format("The string provided could not be converted to a Vector3Int: {0}  Make sure the string is a comma separated list of integer values with or without parenthesis and leading/trailing spaces.",valueString);
            throw new System.ArgumentException(errorMessage);
        }
    }

    public static Vector2Int ParseVector2Int(string valueString){
        try{
        int[] numbers = valueString.Trim(new[]{' ','(',')'}).Split(new []{','}).Select(n => int.Parse(n)).ToArray();
        return new Vector2Int(numbers[0],numbers[2]);
        }catch{
            string errorMessage = string.Format("The string provided could not be converted to a Vector2Int: {0}  Make sure the string is a comma separated list of integer values with or without parenthesis and leading/trailing spaces.",valueString);
            throw new System.ArgumentException(errorMessage);
        }
    }


}