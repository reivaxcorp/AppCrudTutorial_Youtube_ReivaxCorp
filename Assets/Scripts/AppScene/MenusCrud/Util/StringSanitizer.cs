/*
 * C�digo de ejemplo para sanitizar cadenas en C#
 * Autor: OpenAI GPT-3 (https://www.openai.com/)
 * 
 * Este c�digo es proporcionado como un ejemplo y no est� asociado con ning�n proyecto espec�fico.
 * Puedes utilizar, modificar y distribuir este c�digo de acuerdo con los t�rminos de la licencia en tu proyecto.
 */

using System.Text.RegularExpressions;

public class StringSanitizer
{
    public static string SanitizeString(string input)
    {
        string allowedCharacters = "A-Za-z0-9_\\-\\.";

        // Eliminar caracteres no permitidos
        string sanitizedString = Regex.Replace(input, $"[^{Regex.Escape(allowedCharacters)}]", "");

        return sanitizedString;
    }
 
}
