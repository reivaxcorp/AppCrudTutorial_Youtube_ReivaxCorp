/*********************************************************************************
 * Nombre del Archivo:     ReceiverMessagesFromAndroid.cs 
 * Descripci�n:            Esta clase es responsable, de recibir los resultados del sistema de android, 
 *                         especificamente en el OnActivityResult, cuando el usuario elige una imag�n,
 *                         los bytes de esa imagen son recibidos por esta clase, en su correspondiente metodo.
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor � [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentaci�n asociados (el �Software�), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, espec�ficamente en las carpetas �Assets/Scripts�.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D est�n sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, as� como a las pol�ticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA �TAL CUAL�, SIN GARANT�A DE NING�N TIPO, EXPRESA O IMPL�CITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANT�AS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SER�N RESPONSABLES DE CUALQUIER RECLAMACI�N, DA�O U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCI�N DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXI�N CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using System.Collections;
using UnityEngine;

public class ReceiverMessagesFromAndroid : MonoBehaviour
{
    private MenuCrud currentMenu;

    /// <summary>
    /// Puede ser el MenuAddItem � el MenuUpdate Item
    /// </summary>
    /// <param name="menu"></param>
    public void SetCurrentMenu(MenuCrud menu)
    {
        this.currentMenu = menu;    
    }

    // El nombre de la funci�n debe ser la misma que se llama
    // desde la activity personalizada "CrudUnityPlayerActivity"
    public void ReceiveDataFromAndroid(string fileNameWithBase64)
    {
        if (!string.IsNullOrEmpty(fileNameWithBase64))
        {
            StartCoroutine(SetImageFromPathFromUriCoroutine(fileNameWithBase64));
        }
    }

    private IEnumerator SetImageFromPathFromUriCoroutine(string fileNameWithBase64)
    {
        yield return new WaitForSeconds(1.0f); // Esperar que volvamos del selector de archivos.

        // Separar el nombre del archivo y los datos en Base64
        string[] parts = fileNameWithBase64.Split('|');

        if (parts.Length == 2)
        {
            string fileName = parts[0];
            string base64Data = parts[1];

            // Convertir la cadena Base64 a bytes
            byte[] imageData = System.Convert.FromBase64String(base64Data);

            // Hacer algo con los bytes de la imagen (por ejemplo, convertirlos a una textura)
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);

            Debug.Log("Carga terminada");

            if (currentMenu != null)
            {
                currentMenu.SetImagePreview(texture);
                currentMenu.SetImageChange(true);
                currentMenu.fileManager.DeletePreviousCopyImage(); // borramos la imag�n anterior seleccionada
                currentMenu.fileManager.SetCurrentImageName(fileName);
                currentMenu.fileManager.SaveFileInternalExtorage(texture, fileName); // salvamos una copia la imag�n que selecciono
            }
            else
            {
                Debug.LogWarning("CurrentMenu es Null");
            }
        }
        else
        {
            Debug.LogError("Datos invalidos para fileNameWithBase64");
        }
    }

}
