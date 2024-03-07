/*********************************************************************************
 * Nombre del Archivo:     AndroidPermission.cs
 * Descripci�n:            Solicitamos los permisos que sera necesario 
 *                         para acceder al sistema de archivos del usuario
 *                         
 * Autor:                  Javier
 * Organizaci�n:           ReivaxCorp.
 *
 * Derechos de Autor (c) [2024] ReivaxCorp
 * 
 * Permiso es otorgado, sin cargo, para que cualquier persona obtenga una copia
 * de este software y de los archivos de documentaci�n asociados (el "Software"),
 * para tratar en el Software sin restricci�n, incluyendo sin limitaci�n los
 * derechos para usar, copiar, modificar, fusionar, publicar, distribuir,
 * sublicenciar, y/o vender copias del Software, y para permitir a las personas a
 * quienes pertenezca el Software, sujeto a las siguientes condiciones:
 *
 * El aviso de derechos de autor anterior y este aviso de permiso se incluir�n en
 * todas las copias o partes sustanciales del Software.
 *
 * EL SOFTWARE SE PROPORCIONA "TAL CUAL", SIN GARANT�A DE NING�N TIPO, EXPRESA O
 * IMPL�CITA, INCLUYENDO PERO NO LIMITADO A LAS GARANT�AS DE COMERCIABILIDAD,
 * IDONEIDAD PARA UN PROP�SITO PARTICULAR Y NO INFRACCI�N. EN NING�N CASO LOS
 * AUTORES O TITULARES DE DERECHOS DE AUTOR SER�N RESPONSABLES DE CUALQUIER
 * RECLAMACI�N, DA�O O OTRA RESPONSABILIDAD, YA SEA EN UNA ACCI�N DE CONTRATO, AGRAVIO
 * O DE OTRO MODO, DERIVADAS DE, FUERA DE O EN CONEXI�N CON EL SOFTWARE O EL USO U OTROS
 * TRATOS EN EL SOFTWARE.
 *********************************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public enum PermissionStatus
{
    Granted,
    Denied
}

public class AndroidPermission : MonoBehaviour
{
    public delegate void PermissionCallback(PermissionStatus status);
    public event PermissionCallback OnPermissionResult;

    private const string StoragePermission = Permission.ExternalStorageRead;

    public void RequestStoragePermission()
    {
        StartCoroutine(RequestStoragePermissionCoroutine());
    }

    private IEnumerator RequestStoragePermissionCoroutine()
    {
        if (!Permission.HasUserAuthorizedPermission(StoragePermission))
        {
            Permission.RequestUserPermission(StoragePermission);

            float elapsedTime = 0f;
            float timeout = 10f; // Tiempo para aceptar los permisos

            while (!Permission.HasUserAuthorizedPermission(StoragePermission) && elapsedTime < timeout)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            // Una vez pasado el tiempo, preguntamos si el  usuario confirmo los permisos
            if (Permission.HasUserAuthorizedPermission(StoragePermission))
            {
                OnPermissionResult?.Invoke(PermissionStatus.Granted);
            }
            else
            {
                OnPermissionResult?.Invoke(PermissionStatus.Denied);
            }
        }
        else
        {
            OnPermissionResult?.Invoke(PermissionStatus.Granted);
        }
    }

}
