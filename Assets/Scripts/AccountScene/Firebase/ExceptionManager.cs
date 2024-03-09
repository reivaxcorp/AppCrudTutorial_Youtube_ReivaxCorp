/*********************************************************************************
 * Nombre del Archivo:     ExceptionManager.cs
 * Descripción:            Los errores que nos pueda lanzar firebase auth, los errores, serán 
 *                         notificados en la Ui. 
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

using System;
using System.Threading.Tasks;
using UnityEngine;

public class ExceptionManager
{
    /// <summary>
    /// Manage firebase Exception
    /// </summary>
    /// <param name="task"></param>
    public string ManageExceptionForm(Task task)
    {
        AggregateException exception = task.Exception.Flatten();

        foreach (Exception innerException in exception.InnerExceptions)
        {
            Firebase.FirebaseException firebaseException = innerException as Firebase.FirebaseException;

            if (firebaseException != null)
            {
                // Check if the email address is already in use
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.EmailAlreadyInUse)
                {
                    return "El correo electronico ya se esta usando en otra cuenta";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.WrongPassword)
                {
                    return "Contraseña incorrecta";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.UserNotFound)
                {
                    return "Cuenta no encontrada";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.UserMismatch)
                {
                    return "Usuario no coincidente";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.UserDisabled)
                {
                    return "Usuario deshabilitado!";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.InvalidCredential)
                {
                    return "Credenciales invalidas!";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.InvalidAppCredential)
                {
                    return "Credenciales de aplicación invalidas!";
                }
                if (firebaseException.ErrorCode == (int)Firebase.Auth.AuthError.RejectedCredential)
                {
                    return "Credencial rechazada!";
                }
            }
        }

        Debug.LogError("Error. inténtelo nuevamente : " + task.Exception);
        return "Error. inténtelo nuevamente";
    }
}
