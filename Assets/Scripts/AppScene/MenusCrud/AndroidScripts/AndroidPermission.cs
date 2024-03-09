/*********************************************************************************
 * Nombre del Archivo:     AndroidPermission.cs
 * Descripción:            Solicitamos los permisos que sera necesario 
 *                         para acceder al sistema de archivos del usuario
 *                         
 * Autor:                  Javier
 * Organización:           ReivaxCorp.
 *
 * Derechos de Autor © [2024] ReivaxCorp
 
 * Se otorga permiso, sin cargo, a cualquier persona para obtener una copia de este software y de los archivos de
 * documentación asociados (el “Software”), para tratar con el Software sin restricciones, incluyendo, pero no
 * limitado a, los derechos para usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias 
 * del Software, y para permitir a las personas a quienes pertenezca el Software hacer lo mismo, sujeto a las
 * siguientes condiciones:
 
 * El aviso de derechos de autor anterior y este aviso de permiso se incluirán en todas las copias o partes
 * sustanciales del Software realizadas por el desarrollador, específicamente en las carpetas “Assets/Scripts”.
 
 * Las partes de plugins y recursos provenientes de la Asset Store de Unity 3D están sujetas a los derechos de autor 
 * de los respectivos desarrolladores o artistas, así como a las políticas de Unity 3D.
 
 * EL SOFTWARE SE PROPORCIONA “TAL CUAL”, SIN GARANTÍA DE NINGÚN TIPO, EXPRESA O IMPLÍCITA, 
 * INCLUYENDO, PERO NO LIMITADO A, LAS GARANTÍAS DE COMERCIABILIDAD, IDONEIDAD PARA UN 
 * PROPÓSITO PARTICULAR Y NO INFRACCIÓN. EN NINGÚN CASO LOS AUTORES O TITULARES DE DERECHOS DE 
 * AUTOR SERÁN RESPONSABLES DE CUALQUIER RECLAMACIÓN, DAÑO U OTRA RESPONSABILIDAD, YA SEA EN 
 * UNA ACCIÓN DE CONTRATO, AGRAVIO U OTRO MOTIVO, DERIVADA DE, FUERA DE O EN CONEXIÓN CON EL
 * SOFTWARE O EL USO U OTROS TRATOS EN EL SOFTWARE.
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
